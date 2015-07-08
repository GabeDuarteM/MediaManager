using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaManager.Model
{
    [Table("Ids")]
    public class Ids
    {
        [Key]
        public int IDIds { get; set; }

        //public int? IDSerie { get; set; }

        //public virtual Serie Serie { get; set; }

        //public int? IDFilme { get; set; }

        //public virtual Filme Filme { get; set; }

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