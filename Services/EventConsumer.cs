using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Events;
using Nop.Services.Events;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Services
{
    /// <summary>
    /// Represents event consumer
    /// </summary>
    public class EventConsumer : IConsumer<CustomerLoggedinEvent>,
                                IConsumer<EntityUpdatedEvent<ShoppingCartItem>>,
                                IConsumer<EntityInsertedEvent<ShoppingCartItem>>
    {
        #region Fields

        private readonly ISegmentService _segmentService;

        #endregion

        #region Ctor

        public EventConsumer(ISegmentService segmentService)
        {
            _segmentService = segmentService;
        }

        #endregion

        #region Methods

        public void HandleEvent(CustomerLoggedinEvent eventMessage)
        {
            _segmentService.SetIdentifyToSession(eventMessage.Customer);
        }
        
        #endregion

        public void HandleEvent(EntityUpdatedEvent<ShoppingCartItem> eventMessage)
        {
            _segmentService.SetUpdateCartItemToSession(eventMessage.Entity);
        }

        public void HandleEvent(EntityInsertedEvent<ShoppingCartItem> eventMessage)
        {
            _segmentService.SetUpdateCartItemToSession(eventMessage.Entity);
        }
    }
}