using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.SegmentAnalytics.SegmentId")]
        public string SegmentId { get; set; }
        public bool SegmentId_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.SegmentAnalytics.TrackingScript")]
        public string TrackingScript { get; set; }
        public bool TrackingScript_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.SegmentAnalytics.IncludingTax")]
        public bool IncludingTax { get; set; }
        public bool IncludingTax_OverrideForStore { get; set; }
    }
}