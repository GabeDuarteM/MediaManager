using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    [Table("Ids")]
    public class Ids
    {
        public virtual Filme Filme { get; set; }

        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [JsonProperty("imdb")]
        public string imdb { get; set; }

        //public virtual SerieOld Serie { get; set; }

        [JsonProperty("slug")]
        public string slug { get; set; }

        [JsonProperty("tmdb", NullValueHandling = NullValueHandling.Ignore)]
        public int tmdb { get; set; }

        [JsonProperty("trakt", NullValueHandling = NullValueHandling.Ignore)]
        public int trakt { get; set; }

        [JsonProperty("tvdb", NullValueHandling = NullValueHandling.Ignore)]
        public int tvdb { get; set; }

        [JsonProperty("tvrage", NullValueHandling = NullValueHandling.Ignore)]
        public int tvrage { get; set; }
    }
}