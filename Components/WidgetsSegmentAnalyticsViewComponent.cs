using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.SegmentAnalytics.Helpers;
using Nop.Web.Framework.Components;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Widgets.SegmentAnalytics.Models;
using Nop.Plugin.Widgets.SegmentAnalytics.Services;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Components
{
    [ViewComponent(Name = ComponentName.WIDGETS_SEGMENT_ANALYTICS)]
    public class WidgetsSegmentAnalyticsViewComponent : NopViewComponent
    {
        private readonly SegmentAnalyticsSettings _segmentAnalyticsSettings;


        public WidgetsSegmentAnalyticsViewComponent(SegmentAnalyticsSettings segmentAnalyticsSettings)
        {
            _segmentAnalyticsSettings = segmentAnalyticsSettings;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var analyticsTrackingScript = _segmentAnalyticsSettings.TrackingScript + Environment.NewLine;

            AddIdentifyScriptIfExistToSession(ref analyticsTrackingScript);

            AddProductAddToCartScriptIfExistToSession(ref analyticsTrackingScript);

            return View("~/Plugins/Widgets.SegmentAnalytics/Views/PublicInfo.cshtml", analyticsTrackingScript);
        }

        private void AddProductAddToCartScriptIfExistToSession(ref string analyticsTrackingScript)
        {
            var productAddToCart = this.HttpContext.Session.Get<SegmentProduct>(SessionObjectsName.PRODUCT_ADD);

            if(productAddToCart == null) return;

            analyticsTrackingScript += productAddToCart.GetScript(EcommerceEvent.PRODUCT_ADDED);

            this.HttpContext.Session.Remove(SessionObjectsName.PRODUCT_ADD);
        }


        private void AddIdentifyScriptIfExistToSession(ref string  analyticsTrackingScript)
        {
            var identify = this.HttpContext.Session.Get<SegmentIdentify>(SessionObjectsName.IDENTIFY);

            if (identify == null || string.IsNullOrWhiteSpace(identify.UserId)) return;

            analyticsTrackingScript += identify.GetScript();

            this.HttpContext.Session.Remove(SessionObjectsName.IDENTIFY);
        }
    }
}