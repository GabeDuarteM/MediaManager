using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

	public class Serie
	{
		[JsonProperty("title")]
		public string title { get; set; }

		[JsonProperty("year")]
		public string year { get; set; }

		[JsonProperty("ids")]
		public Ids ids { get; set; }

		[JsonProperty("images")]
		public Images images { get; set; }
        
	}
	
	public class Translations {
		
        [JsonProperty("title")]
        public string title { get; set; }
        [JsonProperty("overview")]
        public string overview { get; set; }
        [JsonProperty("language")]
        public string language { get; set; }
	}
}