using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Code.Modelos
{
    public class Fanart
    {
        [JsonProperty("full")]
        public string full { get; set; }

        [JsonProperty("medium")]
        public string medium { get; set; }

        [JsonProperty("thumb")]
        public string thumb { get; set; }
    }

    public class Poster
    {
        [JsonProperty("full")]
        public string full { get; set; }

        [JsonProperty("medium")]
        public string medium { get; set; }

        [JsonProperty("thumb")]
        public string thumb { get; set; }
    }

    public class Logo
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }

    public class Clearart
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }

    public class Banner
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }

    public class Thumb
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }

    public class Images
    {
        [JsonProperty("fanart")]
        public Fanart fanart { get; set; }

        [JsonProperty("poster")]
        public Poster poster { get; set; }

        [JsonProperty("logo")]
        public Logo logo { get; set; }

        [JsonProperty("clearart")]
        public Clearart clearart { get; set; }

        [JsonProperty("banner")]
        public Banner banner { get; set; }

        [JsonProperty("thumb")]
        public Thumb thumb { get; set; }
    }
}