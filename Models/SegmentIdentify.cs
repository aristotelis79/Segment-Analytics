using Newtonsoft.Json;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Models
{

    public class SegmentIdentify
    {
        [JsonProperty("traits", NullValueHandling = NullValueHandling.Ignore)]
        public SegmentTraits Traits { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }


        public string GetScript() => $@"<script> analytics.identify( {this.UserId} ,  {JsonConvert.SerializeObject(this.Traits)} ); </script>";
        
    }

    public class SegmentTraits
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("plan", NullValueHandling = NullValueHandling.Ignore)]
        public string Plan { get; set; }

        [JsonProperty("logins", NullValueHandling = NullValueHandling.Ignore)]
        public int? Logins { get; set; }

        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        public string Website { get; set; }
    }
}