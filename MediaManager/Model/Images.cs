using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaManager.Model
{

    [Table("Images")]
    public class Images
    {
        [Key]
        public int IDImages { get; set; }

        //[ForeignKey("Serie")]
        //public int IDSerie { get; set; }

        //[ForeignKey("Series")]
        //public virtual Serie Serie { get; set; }
        
        //[ForeignKey("Filme")]
        //public int IDFilme { get; set; }

        //[ForeignKey("Filmes")]
        //public virtual Filme Filme { get; set; }

        [JsonProperty("fanart")]
        public virtual Fanart fanart { get; set; }

        [JsonProperty("poster")]
        public virtual Poster poster { get; set; }

        [JsonProperty("logo")]
        public virtual Logo logo { get; set; }

        [JsonProperty("clearart")]
        public virtual Clearart clearart { get; set; }

        [JsonProperty("banner")]
        public virtual Banner banner { get; set; }

        [JsonProperty("thumb")]
        public virtual Thumb thumb { get; set; }

        [NotMapped]
        [JsonProperty("avatar")]
        public virtual Avatar avatar { get; set; }
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

    public class Avatar
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }


}