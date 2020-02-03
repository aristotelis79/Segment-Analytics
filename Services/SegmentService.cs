using System.Linq;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Widgets.SegmentAnalytics.Helpers;
using Nop.Plugin.Widgets.SegmentAnalytics.Models;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Checkout;
using NUglify.Helpers;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Services
{
    public class SegmentService : ISegmentService
    {
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStoreContext _storeContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CurrencySettings _currencySettings;
        private readonly SegmentAnalyticsSettings _segmentAnalyticsSettings;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICurrencyService _currencyService;
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;

        public SegmentService(
                                IGenericAttributeService genericAttributeService,
                                IStoreContext storeContext, 
                                IHttpContextAccessor httpContextAccessor,
                                CurrencySettings currencySettings,
                                SegmentAnalyticsSettings segmentAnalyticsSettings,
                                ICategoryService categoryService,
                                ICurrencyService currencyService,
                                ILogger logger,
                                IOrderService orderService,
                                IProductService productService,
                                ISettingService settingService,
                                IWorkContext workContext, IManufacturerService manufacturerService)
        {
            _genericAttributeService = genericAttributeService;
            _storeContext = storeContext;
            _httpContextAccessor = httpContextAccessor;
            _currencySettings = currencySettings;
            _segmentAnalyticsSettings = segmentAnalyticsSettings;
            _categoryService = categoryService;
            _currencyService = currencyService;
            _logger = logger;
            _orderService = orderService;
            _productService = productService;
            _settingService = settingService;
            _workContext = workContext;
            _manufacturerService = manufacturerService;
        }

        public void SetIdentifyToSession(Customer customer)
        {
            
            var firstName = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.FirstNameAttribute);
            var lastName = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.LastNameAttribute);

            var traits = new SegmentIdentify
            {
                UserId = $"{customer.Id}",
                Traits = new SegmentTraits
                {
                    Id = $"{customer.Id}",
                    Email = customer.Email,
                    Website = _storeContext.CurrentStore.Url,
                    Name = $"{firstName} {lastName}"
                }
            };

            _httpContextAccessor.HttpContext.Session.Set(SessionObjectsName.IDENTIFY, traits);
            
            //TODO DON'T WORK JAVASCRIPT AND .NET TOGETHER
            //Analytics.Client.SetIdentifyToSession($"{customer.Id}",traits);
        }


        public string GetOrderCompletedScript(CheckoutCompletedModel model)
        {
            //var analyticsTrackingScript = _segmentAnalyticsSettings.TrackingScript + Environment.NewLine;
            var order = GetOrder(model);

            if (order == null || _genericAttributeService.GetAttribute<bool>(order, SegmentAnalyticsSettings.ORDER_ALREADY_PROCESSED_ATTRIBUTE_NAME))
                return null;

            var segmentAnalyticsSettings = _settingService.LoadSetting<SegmentAnalyticsSettings>(_storeContext.CurrentStore.Id);

            var orderCompleted = new SegmentOrderCompleted
            {
                OrderId = order.CustomOrderNumber,
                Affiliation = _storeContext.CurrentStore.Name,
                Currency = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode,
                Total = order.OrderTotal,
                Tax = order.OrderTax,
                Shipping = segmentAnalyticsSettings.IncludingTax
                    ? order.OrderShippingInclTax
                    : order.OrderShippingExclTax
            };

            var position = 1;

            order.OrderItems.ForEach(oi =>
            {
                orderCompleted.Products.Add(new SegmentProduct
                {
                    ProductId = oi.ProductId.ToString(),
                    Sku = _productService.FormatSku(oi.Product, oi.AttributesXml),
                    Name = oi.Product.Name,
                    Category = _categoryService.GetProductCategoriesByProductId(oi.ProductId).FirstOrDefault()?.Category.Name,
                    Price = segmentAnalyticsSettings.IncludingTax ? oi.UnitPriceInclTax : oi.UnitPriceExclTax,
                    Quantity = oi.Quantity,
                    Position = position++
                });
            });

            _genericAttributeService.SaveAttribute(order, SegmentAnalyticsSettings.ORDER_ALREADY_PROCESSED_ATTRIBUTE_NAME, true);

            return orderCompleted.GetScript();
        }


        public void SetUpdateCartItemToSession(ShoppingCartItem sci)
        {
            if(sci.ShoppingCartType == ShoppingCartType.Wishlist) return;

            var segmentProduct = new SegmentProduct
            {
                CartId = $"{sci.Id}",
                ProductId = $"{sci.ProductId}",
                Sku = sci.Product.Sku,
                Category = _categoryService.GetProductCategoriesByProductId(sci.ProductId).FirstOrDefault()?.Category.Name,
                Name = sci.Product.Name,
                Brand = _manufacturerService.GetProductManufacturersByProductId(sci.ProductId).FirstOrDefault()?.Manufacturer.Name,
                Price = sci.Product.Price,
                Quantity = sci.Quantity,
                Value = sci.Quantity * sci.Product.Price
            };

            _httpContextAccessor.HttpContext.Session.Set(SessionObjectsName.PRODUCT_ADD, segmentProduct);
        }


        public string GetProductViewedScript(ProductDetailsModel model)
        {
            var segmentProduct = new SegmentProduct
            {
                ProductId = $"{model.Id}",
                Sku = model.Sku,
                Category = _categoryService.GetProductCategoriesByProductId(model.Id).FirstOrDefault()?.Category.Name,
                Name = model.Name,
                Brand = model.ProductManufacturers.FirstOrDefault(x=>x.IsActive)?.Name,
                Price = model.ProductPrice.PriceValue,
                Currency = model.ProductPrice.CurrencyCode,
                ImageUrl = model.DefaultPictureModel.ImageUrl,
                Url = model.SeName
            };

            return segmentProduct.GetScript(EcommerceEvent.PRODUCT_VIEWED);
        }


        private Order GetOrder(CheckoutCompletedModel model)
        {
            return  (model == null) 
                ? _orderService.SearchOrders(storeId: _storeContext.CurrentStore.Id,customerId: _workContext.CurrentCustomer.Id, pageSize: 1).FirstOrDefault() 
                : _orderService.GetOrderById(model.OrderId);
        }
    }
}