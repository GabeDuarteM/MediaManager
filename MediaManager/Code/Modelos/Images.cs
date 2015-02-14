using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    public class Avatar
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }

    public class Images
    {
        [Key]
        public int IDImages { get; set; }

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
}