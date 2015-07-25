using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    public class Avatar
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }

    public class Banner
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }

    public class Clearart
    {
        [JsonProperty("full")]
        public string full { get; set; }
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

    [Table("Images")]
    public class Images
    {
        [NotMapped]
        [JsonProperty("avatar")]
        public virtual Avatar avatar { get; set; }

        [JsonProperty("banner")]
        public virtual Banner banner { get; set; }

        [JsonProperty("clearart")]
        public virtual Clearart clearart { get; set; }

        [JsonProperty("fanart")]
        public virtual Fanart fanart { get; set; }

        public virtual Filme Filme { get; set; }

        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [JsonProperty("logo")]
        public virtual Logo logo { get; set; }

        [JsonProperty("poster")]
        public virtual Poster poster { get; set; }

        public virtual Serie Serie { get; set; }

        [JsonProperty("thumb")]
        public virtual Thumb thumb { get; set; }
    }

    public class Logo
    {
        [JsonProperty("full")]
        public string full { get; set; }
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

    public class Thumb
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }
}