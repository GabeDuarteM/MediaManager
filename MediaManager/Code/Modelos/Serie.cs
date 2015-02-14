using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Code.Modelos
{
    public class Serie
    {
        [Key]
        public int IDSerie { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int year { get; set; }

        [JsonProperty("ids")]
        public virtual Ids ids { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("first_aired", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime first_aired { get; set; }

        [JsonProperty("runtime", NullValueHandling = NullValueHandling.Ignore)]
        public int runtime { get; set; }

        [JsonProperty("certification")]
        public string certification { get; set; }

        [JsonProperty("network")]
        public string network { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("trailer")]
        public object trailer { get; set; }

        [JsonProperty("homepage")]
        public object homepage { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public double rating { get; set; }

        [JsonProperty("votes", NullValueHandling = NullValueHandling.Ignore)]
        public int votes { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime updated_at { get; set; }

        [JsonProperty("language")]
        public string language { get; set; }

        [JsonProperty("available_translations")]
        public IList<string> available_translations { get; set; }

        [JsonProperty("genres")]
        public IList<string> genres { get; set; }

        [JsonProperty("aired_episodes", NullValueHandling = NullValueHandling.Ignore)]
        public int aired_episodes { get; set; }

        [JsonProperty("images")]
        public virtual Images images { get; set; }

        [JsonProperty("day")]
        public string airDay { get; set; }

        [JsonProperty("time")]
        public string airTime { get; set; }

        [JsonProperty("timezone")]
        public string airTimezone { get; set; }
    }
}