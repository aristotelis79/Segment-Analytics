using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Checkout;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Services
{
    public interface ISegmentService
    {
        void SetIdentifyToSession(Customer customer);

        string GetOrderCompletedScript(CheckoutCompletedModel model);
        
        void SetUpdateCartItemToSession(ShoppingCartItem entity);
   
        string GetProductViewedScript(ProductDetailsModel model);
    }
}