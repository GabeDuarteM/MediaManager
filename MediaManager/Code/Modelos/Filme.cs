using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Code.Modelos
{
    public class Filme
    {
        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int year { get; set; }

        [JsonProperty("ids")]
        public Ids ids { get; set; }

        [JsonProperty("tagline")]
        public string tagline { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("released")]
        public string released { get; set; }

        [JsonProperty("runtime", NullValueHandling = NullValueHandling.Ignore)]
        public int runtime { get; set; }

        [JsonProperty("trailer", NullValueHandling = NullValueHandling.Ignore)]
        public object trailer { get; set; }

        [JsonProperty("homepage", NullValueHandling = NullValueHandling.Ignore)]
        public object homepage { get; set; }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public double rating { get; set; }

        [JsonProperty("votes", NullValueHandling = NullValueHandling.Ignore)]
        public int votes { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime updated_at { get; set; }

        [JsonProperty("language")]
        public string language { get; set; }

        [JsonProperty("available_translations", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> available_translations { get; set; }

        [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> genres { get; set; }

        [JsonProperty("certification")]
        public string certification { get; set; }

        [JsonProperty("images")]
        public Images images { get; set; }
    }
}