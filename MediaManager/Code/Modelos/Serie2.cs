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

    public class Airs
    {
        [JsonProperty("day")]
        public string day { get; set; }

        [JsonProperty("time")]
        public string time { get; set; }

        [JsonProperty("timezone")]
        public string timezone { get; set; }
    }

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

    public class Serie2
    {
        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int year { get; set; }

        [JsonProperty("ids")]
        public Ids ids { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("first_aired")]
        public DateTime first_aired { get; set; }

        [JsonProperty("airs")]
        public Airs airs { get; set; }

        [JsonProperty("runtime", NullValueHandling = NullValueHandling.Ignore)]
        public int runtime { get; set; }

        [JsonProperty("certification")]
        public string certification { get; set; }

        [JsonProperty("network")]
        public string network { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("trailer")]
        public string trailer { get; set; }

        [JsonProperty("homepage")]
        public object homepage { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("rating")]
        public double rating { get; set; }

        [JsonProperty("votes", NullValueHandling = NullValueHandling.Ignore)]
        public int votes { get; set; }

        [JsonProperty("updated_at")]
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
        public Images images { get; set; }
    }
}