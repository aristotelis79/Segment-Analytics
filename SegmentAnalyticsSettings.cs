using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.SegmentAnalytics
{
    public class SegmentAnalyticsSettings : ISettings
    {
        public string SegmentId { get; set; }
        public string TrackingScript { get; set; }
        public bool IncludingTax { get; set; }
        
        public static string ORDER_ALREADY_PROCESSED_ATTRIBUTE_NAME = "SegmentAnalytics.OrderAlreadyProcessed";}
}