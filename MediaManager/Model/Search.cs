using System;
using System.IO;
using MediaManager.Helpers;
using MediaManager.Properties;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    public class Search
    {
        private Show _video;

        public bool isAnime { get; set; }

        [JsonProperty("movie")]
        public virtual Show movie { set { Video = value; } }

        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public double score { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("show")]
        public Show Video { get { return _video; } set { if (value != null) _video = value; } }

        public Video ToVideo()
        {
            Video video;
            switch (type)
            {
                case "show":
                    {
                        video = new Serie();
                        video.Title = Video.title;
                        video.Images = Video.images;
                        video.Overview = Video.overview;
                        video.Ids = Video.ids;
                        if (isAnime)
                        {
                            video.FolderPath = !string.IsNullOrWhiteSpace(Settings.Default.pref_PastaAnimes) ? Path.Combine(Settings.Default.pref_PastaAnimes, video.Title) : null;
                            video.MetadataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    Settings.Default.AppName, "Metadata", "Animes", Helper.RetirarCaracteresInvalidos(video.Title));
                        }
                        else
                        {
                            video.FolderPath = !string.IsNullOrWhiteSpace(Settings.Default.pref_PastaSeries) ? Path.Combine(Settings.Default.pref_PastaSeries, video.Title) : null;
                            video.MetadataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    Settings.Default.AppName, "Metadata", "Séries", Helper.RetirarCaracteresInvalidos(video.Title));
                        }
                        return video;
                    }
                case "movie":
                    {
                        video = new Filme();
                        video.Title = Video.title;
                        video.Images = Video.images;
                        video.Overview = Video.overview;
                        video.Ids = Video.ids;
                        video.MetadataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            Settings.Default.AppName, "Metadata", "Filmes", Helper.RetirarCaracteresInvalidos(video.Title));
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