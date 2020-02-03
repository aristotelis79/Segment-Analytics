using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.SegmentAnalytics.Helpers;
using Nop.Services.Logging;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Components
{
    [ViewComponent(Name = ComponentName.FOOTERSEGMENTANALYTICS)]
    public class FooterSegmentAnalytics : NopViewComponent
    {
        private readonly ILogger _logger;

        public FooterSegmentAnalytics(ILogger logger)
        {
            _logger = logger;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var script = string.Empty;
            var routeData = Url.ActionContext.RouteData;

            try
            {
                var controller = routeData.Values["controller"];
                var action = routeData.Values["action"];

                if (controller == null || action == null)
                    return Content("");

                var isOrderCompletedPage = controller.ToString().Equals("checkout", StringComparison.InvariantCultureIgnoreCase) &&
                                           action.ToString().Equals("completed", StringComparison.InvariantCultureIgnoreCase);

            }
            catch (Exception ex)
            {
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, "Error creating scripts for Segment eCommerce tracking", ex.ToString());
            }
            return View("~/Plugins/Widgets.SegmentAnalytics/Views/PublicInfo.cshtml", script);
        }
    }
}