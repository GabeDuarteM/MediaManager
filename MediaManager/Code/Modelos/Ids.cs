using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Code.Modelos
{
    public class Ids
    {
        [JsonProperty("trakt", NullValueHandling = NullValueHandling.Ignore)]
        public int trakt { get; set; }

        [JsonProperty("slug")]
        public string slug { get; set; }

        [JsonProperty("tvdb", NullValueHandling = NullValueHandling.Ignore)]
        public int tvdb { get; set; }

        [JsonProperty("imdb")]
        public string imdb { get; set; }

        [JsonProperty("tmdb", NullValueHandling = NullValueHandling.Ignore)]
        public int tmdb { get; set; }

        [JsonProperty("tvrage", NullValueHandling = NullValueHandling.Ignore)]
        public int tvrage { get; set; }
    }
}