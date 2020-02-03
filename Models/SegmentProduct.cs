using System;
using Newtonsoft.Json;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Models
{
    public class SegmentProduct
    {
        [JsonProperty("product_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ProductId { get; set; }

        [JsonProperty("cart_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CartId { get; set; }

        [JsonProperty("sku", NullValueHandling = NullValueHandling.Ignore)]
        public string Sku { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("brand", NullValueHandling = NullValueHandling.Ignore)]
        public string Brand { get; set; }

        [JsonProperty("variant", NullValueHandling = NullValueHandling.Ignore)]
        public string Variant { get; set; }

        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Price { get; set; }

        [JsonProperty("quantity", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Quantity { get; set; }

        [JsonProperty("coupon", NullValueHandling = NullValueHandling.Ignore)]
        public string Coupon { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Value { get; set; }

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public int? Position { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("image_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageUrl { get; set; }


        public string GetScript(string name) =>  $@"<script> analytics.track( '{name}', {JsonConvert.SerializeObject(this)} ); </script>";

    }
}