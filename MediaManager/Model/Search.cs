using Newtonsoft.Json;

namespace MediaManager.Model
{
    public class Show
    {
        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int year { get; set; }

        [JsonProperty("images")]
        public Images images { get; set; }

        [JsonProperty("ids")]
        public Ids ids { get; set; }
    }

    public class Search
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public double score { get; set; }

        [JsonProperty("show")]
        public Show show { get; set; }

        [JsonProperty("movie")]
        public Show movie { get; set; }
    }
}