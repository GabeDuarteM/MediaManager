using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Code.Series
{
    public class Ids
    {
        [JsonProperty("trakt")]
        public string trakt { get; set; }

        [JsonProperty("slug")]
        public string slug { get; set; }

        [JsonProperty("tvdb")]
        public string tvdb { get; set; }

        [JsonProperty("imdb")]
        public string imdb { get; set; }

        [JsonProperty("tmdb")]
        public string tmdb { get; set; }

        [JsonProperty("tvrage")]
        public string tvrage { get; set; }
    }

    public class Image
    {
        [JsonProperty("full")]
        public string full { get; set; }

        [JsonProperty("medium")]
        public string medium { get; set; }

        [JsonProperty("thumb")]
        public string thumb { get; set; }
    }

    public class Images
    {
        [JsonProperty("fanart")]
        public Image fanart { get; set; }

        [JsonProperty("poster")]
        public Image poster { get; set; }

        [JsonProperty("logo")]
        public Image logo { get; set; }

        [JsonProperty("clearart")]
        public Image clearart { get; set; }

        [JsonProperty("banner")]
        public Image banner { get; set; }

        [JsonProperty("thumb")]
        public Image thumb { get; set; }
    }

    public class Serie
    {
        [Key]
        public int IDSerie { get; set; }

        public string idTrakt { get; set; }

        public string slugTrakt { get; set; }

        public string idImdb { get; set; }

        public byte[] poster { get; set; }

        public string folderPath { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int year { get; set; }

        [NotMapped]
        [JsonProperty("language")]
        public string language { get; set; }

        [NotMapped]
        [JsonProperty("ids")]
        public Ids ids { get; set; }

        [NotMapped]
        [JsonProperty("images")]
        public Images images { get; set; }

        [NotMapped]
        [JsonProperty("type")]
        public string type { get; set; }

        [NotMapped]
        [JsonProperty("score")]
        public double score { get; set; }

        [NotMapped]
        [JsonProperty("show")]
        public Serie show { get; set; }
    }

    public class SerieContext : DbContext
    {
        public DbSet<Serie> Serie { get; set; }

        public DbSet<Ids> Ids { get; set; }
    }
}