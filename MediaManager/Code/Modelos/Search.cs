using Newtonsoft.Json;

namespace MediaManager.Code.Searches
{
    public class Poster
    {
        [JsonProperty(PropertyName = "full")]
        public string full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string thumb { get; set; }
    }

    public class Fanart
    {
        [JsonProperty(PropertyName = "full")]
        public string full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string thumb { get; set; }
    }

    public class Images
    {
        [JsonProperty(PropertyName = "poster")]
        public Poster poster { get; set; }

        [JsonProperty(PropertyName = "fanart")]
        public Fanart fanart { get; set; }
    }

    public class Ids
    {
        [JsonProperty(PropertyName = "trakt")]
        public string trakt { get; set; }

        [JsonProperty(PropertyName = "slug")]
        public string slug { get; set; }

        [JsonProperty(PropertyName = "tvdb")]
        public string tvdb { get; set; }

        [JsonProperty(PropertyName = "imdb")]
        public string imdb { get; set; }

        [JsonProperty(PropertyName = "tmdb")]
        public string tmdb { get; set; }

        [JsonProperty(PropertyName = "tvrage")]
        public string tvrage { get; set; }
    }

    public class Show
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string overview { get; set; }

        [JsonProperty(PropertyName = "year")]
        public string year { get; set; }

        [JsonProperty(PropertyName = "images")]
        public Images images { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public Ids ids { get; set; }
    }

    public class Search
    {
        [JsonProperty(PropertyName = "type")]
        public string type { get; set; }

        [JsonProperty(PropertyName = "score")]
        public double score { get; set; }

        [JsonProperty(PropertyName = "show")]
        public Show show { get; set; }
    }
}