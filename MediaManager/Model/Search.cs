using System.IO;
using MediaManager.Helpers;
using MediaManager.Properties;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    public class Search
    {
        public bool isAnime { get; set; }

        [JsonProperty("movie")]
        public virtual Show movie { get; set; }

        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public double score { get; set; }

        [JsonProperty("show")]
        public virtual Show show { get; set; }

        public string Title { get { return (show != null) ? show.title : movie.title; } }

        [JsonProperty("type")]
        public string type { get; set; }

        public Video ToVideo()
        {
            Video video;
            switch (type)
            {
                case "show":
                    {
                        video = new Serie();
                        video.Title = Title;
                        video.Images = show.images;
                        video.Overview = show.overview;
                        video.Ids = show.ids;
                        if (isAnime)
                            video.FolderPath = !string.IsNullOrWhiteSpace(Settings.Default.pref_PastaAnimes) ? Path.Combine(Settings.Default.pref_PastaAnimes, video.Title) : null;
                        else
                            video.FolderPath = !string.IsNullOrWhiteSpace(Settings.Default.pref_PastaSeries) ? Path.Combine(Settings.Default.pref_PastaSeries, video.Title) : null;
                        return video;
                    }
                case "movie":
                    {
                        video = new Filme();
                        video.Title = Title;
                        video.Images = movie.images;
                        video.Overview = movie.overview;
                        video.Ids = movie.ids;
                        video.FolderPath = !string.IsNullOrWhiteSpace(Settings.Default.pref_PastaFilmes) ? Path.Combine(Settings.Default.pref_PastaFilmes, video.Title) : null;
                        return video;
                    }
                default:
                    return null;
            }
        }
    }

    public class Show
    {
        [JsonProperty("ids")]
        public Ids ids { get; set; }

        [JsonProperty("images")]
        public Images images { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int year { get; set; }
    }
}