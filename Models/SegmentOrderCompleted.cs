using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Models
{
    public class SegmentOrderCompleted
    {
        [JsonProperty("checkout_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckoutId { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("affiliation", NullValueHandling = NullValueHandling.Ignore)]
        public string Affiliation { get; set; }

        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Total { get; set; }

        [JsonProperty("revenue", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Revenue { get; set; }

        [JsonProperty("shipping", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Shipping { get; set; }

        [JsonProperty("tax", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Tax { get; set; }

        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Discount { get; set; }

        [JsonProperty("coupon", NullValueHandling = NullValueHandling.Ignore)]
        public string Coupon { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("products", NullValueHandling = NullValueHandling.Ignore)]
        public List<SegmentProduct> Products { get; set; }


        public string GetScript()
        {
            return $@"<script> analytics.track('Order Completed', {JsonConvert.SerializeObject(this)} ); </script>";
        }
    }
}