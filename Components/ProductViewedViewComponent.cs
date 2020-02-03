using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.SegmentAnalytics.Helpers;
using Nop.Plugin.Widgets.SegmentAnalytics.Services;
using Nop.Services.Logging;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Checkout;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Components
{
    [ViewComponent(Name = ComponentName.PRODUCT_VIEWED)]
    public class ProductViewedViewComponent : NopViewComponent
    {
        #region Fields
        private readonly ISegmentService _segmentService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public ProductViewedViewComponent(
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
                script = _segmentService.GetProductViewedScript(additionalData as ProductDetailsModel);
            }
            catch (Exception ex)
            {
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, "Error creating scripts for Segment eCommerce tracking", ex.ToString());
            }
            return View("~/Plugins/Widgets.SegmentAnalytics/Views/PublicInfo.cshtml", script);
        }

        #endregion

    }
}