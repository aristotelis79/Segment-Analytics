using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.SegmentAnalytics.Helpers;
using Nop.Plugin.Widgets.SegmentAnalytics.Services;
using Nop.Services.Logging;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Checkout;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Components
{
    [ViewComponent(Name = ComponentName.ORDER_COMPLETED)]
    public class OrderCompletedViewComponent : NopViewComponent
    {
        #region Fields
        private readonly ISegmentService _segmentService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public OrderCompletedViewComponent(
            ILogger logger,
            ISegmentService segmentService)
        {
            _logger = logger;
            _segmentService = segmentService;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var script = string.Empty;
            try
            {
                script = _segmentService.GetOrderCompletedScript(additionalData as CheckoutCompletedModel);
            }
            catch (Exception ex)
            {
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, "Error creating scripts for Segment eCommerce tracking", ex.ToString());
            }
            return View("~/Plugins/Widgets.SegmentAnalytics/Views/PublicInfo.cshtml", script);
        }

        #endregion

        #region DELETE?
        //private string OrderCompletedAnalyticsTrackingScript(Order order, SegmentAnalyticsSettings segmentAnalyticsSettings)
        //{
        //    var usCulture = new CultureInfo("en-US");

        //    var analyticsTrackingScript = $@"analytics.track('Order Completed', {{
        //                                        ""order_id"": {order.CustomOrderNumber.FixIllegalJavaScriptChars()},
        //                                        ""affiliation"": {_storeContext.CurrentStore.Name.FixIllegalJavaScriptChars()},
        //                                        ""total"": {order.OrderTotal.ToString("0.00", usCulture)},
        //                                        ""currency"": {_currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode},
        //                                        ""tax"": {order.OrderTax.ToString("0.00", usCulture)},
        //                                        ""shipping"": {(segmentAnalyticsSettings.IncludingTax ? order.OrderShippingInclTax : order.OrderShippingExclTax).ToString("0.00", usCulture)},
        //                                        ""products"": [ {{DETAILS}} ]
        //                                        }});";

        //    var sb = new StringBuilder();
        //    var listingPosition = 1;
        //    foreach (var item in order.OrderItems)
        //    {
        //        if (!string.IsNullOrEmpty(sb.ToString()))
        //            sb.AppendLine(",");


        //        var analyticsEcommerceDetailScript = $@"{{
        //                                'product_id': '{item.SegmentProduct.Id.ToString().FixIllegalJavaScriptChars()}',
        //                                'sku': '{_productService.FormatSku(item.SegmentProduct, item.AttributesXml).FixIllegalJavaScriptChars()}',
        //                                'name': '{item.SegmentProduct.Name.FixIllegalJavaScriptChars()}',
        //                                'category': '{_categoryService.GetProductCategoriesByProductId(item.ProductId).FirstOrDefault()?.Category.Name.FixIllegalJavaScriptChars()}',
        //                                'position': {listingPosition.ToString()},
        //                                'quantity': {item.Quantity.ToString()},
        //                                'price': '{(segmentAnalyticsSettings.IncludingTax ? item.UnitPriceInclTax : item.UnitPriceExclTax).ToString("0.00", usCulture)}'
        //                                }}";

        //        sb.AppendLine(analyticsEcommerceDetailScript);

        //        listingPosition++;
        //    }

        //    analyticsTrackingScript = analyticsTrackingScript.Replace("{DETAILS}", sb.ToString());
        //    return analyticsTrackingScript;
        //}
        #endregion
    }
}