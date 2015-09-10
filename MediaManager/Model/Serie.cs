using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Xml.Serialization;

namespace MediaManager.Model
{
    [System.Diagnostics.DebuggerDisplay("{SeasonNumber}x{EpisodeNumber} ({AbsoluteNumber}) - {EpisodeName}")]
    public class Episode : INotifyPropertyChanged
    {
        [XmlElement("absolute_number")]
        public string _AbsoluteNumber;

        [XmlElement("airsafter_season")]
        public string _AirsAfterSeason;

        [XmlElement("airsbefore_episode")]
        public string _AirsBeforeEpisode;

        [XmlElement("airsbefore_season")]
        public string _AirsBeforeSeason;

        [XmlElement("FirstAired")]
        public string _FirstAired;

        [XmlElement("Rating")]
        public string _Rating;

        [XmlElement("RatingCount")]
        public string _RatingCount;

        [XmlElement("thumb_added", IsNullable = true)]
        public string _ThumbAddedDate;

        [XmlIgnore]
        private string _Artwork;

        [XmlIgnore]
        private string _FilePath;

        [XmlIgnore]
        private string _FolderPath;

        [XmlIgnore, Column(Order = 4)]
        public int? AbsoluteNumber
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_AbsoluteNumber) && int.TryParse(_AbsoluteNumber, out retval) ? (int?)retval : null;
            }
            set { _AbsoluteNumber = value.ToString(); }
        }

        [XmlIgnore]
        public int? AirsAfterSeason
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_AirsAfterSeason) && int.TryParse(_AirsAfterSeason, out retval) ? (int?)retval : null;
            }
            set
            {
                _AirsAfterSeason = value.ToString();
            }
        }

        [XmlIgnore]
        public int? AirsBeforeEpisode
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_AirsBeforeEpisode) && int.TryParse(_AirsBeforeEpisode, out retval) ? (int?)retval : null;
            }
            set
            {
                _AirsBeforeEpisode = value.ToString();
            }
        }

        [XmlIgnore]
        public int? AirsBeforeSeason
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_AirsBeforeSeason) && int.TryParse(_AirsBeforeSeason, out retval) ? (int?)retval : null;
            }
            set
            {
                _AirsBeforeSeason = value.ToString();
            }
        }

        [XmlElement("filename", IsNullable = true)]
        public string Artwork
        {
            get { return _Artwork; }
            set
            {
                if (value.StartsWith("http"))
                    _Artwork = value;
                else
                {
                    _Artwork = string.IsNullOrWhiteSpace(value) ?
                        "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png"
                        : Properties.Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                }
            }
        }

        [XmlElement("EpisodeName"), Column(Order = 1)]
        public string EpisodeName { get; set; }

        [XmlElement("EpisodeNumber"), Column(Order = 2)]
        public int EpisodeNumber { get; set; }

        [XmlIgnore]
        public string FilePath { get { return _FilePath; } set { _FilePath = value; OnPropertyChanged("FilePath"); } }

        [XmlIgnore]
        public DateTime? FirstAired
        {
            get { return _FirstAired != "" ? DateTime.Parse(_FirstAired) : default(DateTime?); }
            set { _FirstAired = value.ToString(); }
        }

        [XmlIgnore]
        public string FolderPath { get { return _FolderPath; } set { _FolderPath = value; OnPropertyChanged("FolderPath"); } }

        [XmlIgnore, Key, Column("ID", Order = 0)]
        public int IDBanco { get; set; }

        [XmlElement("seasonid")]
        public int IDSeasonTvdb { get; set; }

        [XmlIgnore, Required]
        public int IDSerie { get; set; }

        [XmlElement("seriesid")]
        public int IDSeriesTvdb { get; set; }

        [XmlElement("id")]
        public int IDTvdb { get; set; }

        [XmlIgnore]
        public bool IsRenamed { get; set; }

        [XmlElement("Language")]
        public string Language { get; set; }

        [XmlElement("lastupdated", IsNullable = true)]
        public string LastUpdated { get; set; }

        [XmlIgnore]
        public string OriginalFilePath { get; set; }

        [XmlElement("Overview", IsNullable = true)]
        public string Overview { get; set; }

        [XmlIgnore]
        public double? Rating
        {
            get
            {
                double retval;
                return !string.IsNullOrWhiteSpace(_Rating) && double.TryParse(_Rating, NumberStyles.Number, CultureInfo.InvariantCulture, out retval) ? (double?)retval : null;
            }
            set { _Rating = value.ToString(); }
        }

        [XmlIgnore]
        public int? RatingCount
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_RatingCount) && int.TryParse(_RatingCount, out retval) ? (int?)retval : null;
            }
            set
            {
                _RatingCount = value.ToString();
            }
        }

        [XmlElement("SeasonNumber"), Column(Order = 3)]
        public int SeasonNumber { get; set; }

        [Column(Order = 5), ForeignKey("IDSerie")]
        public virtual Serie Serie { get; set; }

        [XmlIgnore]
        public DateTime? ThumbAddedDate
        {
            get { return _ThumbAddedDate != "" ? DateTime.Parse(_ThumbAddedDate) : default(DateTime?); }
            set { _ThumbAddedDate = value.ToString(); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }

    public class Serie : Video
    {
        [XmlElement("FirstAired")]
        public string _FirstAired;

        [XmlElement("Rating")]
        public string _Rating;

        [XmlElement("RatingCount")]
        public string _RatingCount;

        [XmlElement("Runtime")]
        public string _Runtime;

        [XmlIgnore]
        private Helpers.Helper.Enums.ContentType _ContentType = Helpers.Helper.Enums.ContentType.show;

        private string _ImgFanart = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
        private string _ImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";

        [XmlIgnore]
        private bool _IsAnime;

        [XmlIgnore]
        private string _Overview;

        [XmlIgnore]
        private string _Title;

        public Serie()
        {
        }

        public Serie(PosterGrid posterGrid)
        {
            AliasNames = posterGrid.AliasNames;
            ContentType = posterGrid.ContentType;
            Estado = posterGrid.Estado;
            FolderPath = posterGrid.FolderPath;
            IDApi = posterGrid.IDApi;
            IDBanco = posterGrid.IDBanco;
            ImgFanart = posterGrid.ImgFanart;
            ImgPoster = posterGrid.ImgPoster;
            Language = posterGrid.Language;
            LastUpdated = posterGrid.LastUpdated;
            Overview = posterGrid.Overview;
            Title = posterGrid.Title;
        }

        [XmlElement("Actors", IsNullable = true)]
        public string Actors { get; set; }

        [XmlElement("Airs_DayOfWeek", IsNullable = true)]
        public string Airs_DayOfWeek { get; set; }

        [XmlElement("Airs_Time", IsNullable = true)]
        public string Airs_Time { get; set; }

        [XmlElement("AliasNames")]
        public override string AliasNames { get; set; }

        [XmlElement("ContentRating", IsNullable = true)]
        public string ContentRating { get; set; }

        [NotMapped]
        public override Helpers.Helper.Enums.ContentType ContentType { get { return _ContentType; } set { _ContentType = value; OnPropertyChanged("ContentType"); } }

        [XmlIgnore, NotMapped]
        public Episode[] Episodes { get; set; }

        [XmlIgnore]
        public DateTime? FirstAired { get { return DateTime.Parse(_FirstAired); } set { _FirstAired = value.ToString(); } }

        [XmlElement("Genre", IsNullable = true)]
        public string Genre { get; set; }

        [XmlElement("id"), Column(Order = 2)]
        public override int IDApi { get; set; }

        [XmlIgnore, Key, Column("ID", Order = 0)]
        public override int IDBanco { get; set; }

        [XmlElement("fanart", IsNullable = true)]
        public override string ImgFanart
        {
            get { return _ImgFanart; }
            set
            {
                if (value.StartsWith("http"))
                    _ImgFanart = value;
                else
                {
                    _ImgFanart = string.IsNullOrWhiteSpace(value) ?
                        _ImgFanart = ("pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png")
                        : _ImgFanart = Properties.Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                }
                OnPropertyChanged("ImgFanart");
            }
        }

        [XmlElement("poster", IsNullable = true)]
        public override string ImgPoster
        {
            get { return _ImgPoster; }
            set
            {
                if (value.StartsWith("http"))
                    _ImgPoster = value;
                else
                {
                    _ImgPoster = string.IsNullOrWhiteSpace(value) ?
                    _ImgPoster = ("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                    : Properties.Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                    OnPropertyChanged("ImgPoster");
                }
            }
        }

        [XmlIgnore]
        public bool IsAnime { get { return _IsAnime; } set { _IsAnime = value; ContentType = (value == true) ? Helpers.Helper.Enums.ContentType.anime : ContentType; } }

        [XmlElement("Language")]
        public override string Language { get; set; }

        [XmlElement("lastupdated", IsNullable = true)]
        public override string LastUpdated { get; set; }

        [XmlElement("Network", IsNullable = true)]
        public string Network { get; set; }

        [XmlElement("Overview", IsNullable = true)]
        public override string Overview { get { return _Overview; } set { _Overview = value; OnPropertyChanged("Overview"); } }

        [XmlIgnore]
        public double? Rating
        {
            get
            {
                double retval;
                return !string.IsNullOrWhiteSpace(_Rating) && double.TryParse(_Rating, NumberStyles.Number, CultureInfo.InvariantCulture, out retval) ? (double?)retval : null;
            }
            set { _Rating = value.ToString(); }
        }

        [XmlIgnore]
        public int? RatingCount
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_RatingCount) && int.TryParse(_RatingCount, out retval) ? (int?)retval : null;
            }
            set { _RatingCount = value.ToString(); }
        }

        [XmlIgnore]
        public int? Runtime
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_Runtime) && int.TryParse(_Runtime, out retval) ? (int?)retval : null;
            }
            set { _Runtime = value.ToString(); }
        }

        [XmlElement("Status", IsNullable = true)]
        public string Status { get; set; }

        [XmlElement("SeriesName", IsNullable = true), Column(Order = 1)]
        public override string Title { get { return _Title; } set { _Title = value; OnPropertyChanged("Title"); } }

        public override void Clone(object objectToClone)
        {
            Serie serie = objectToClone as Serie;

            Actors = serie.Actors;
            Airs_DayOfWeek = serie.Airs_DayOfWeek;
            Airs_Time = serie.Airs_Time;
            AliasNames = serie.AliasNames;
            ContentRating = serie.ContentRating;
            ContentType = serie.ContentType;
            FirstAired = serie.FirstAired;
            FolderPath = serie.FolderPath;
            Genre = serie.Genre;
            IDApi = serie.IDApi;
            IDBanco = serie.IDBanco;
            ImgFanart = serie.ImgFanart;
            ImgPoster = serie.ImgPoster;
            IsAnime = serie.IsAnime;
            Language = serie.Language;
            LastUpdated = serie.LastUpdated;
            Network = serie.Network;
            Overview = serie.Overview;
            Rating = serie.Rating;
            RatingCount = serie.RatingCount;
            Runtime = serie.Runtime;
            Status = serie.Status;
            Title = serie.Title;
            Estado = serie.Estado;
            Episodes = serie.Episodes;
        }

        public void SetDefaultFolderPath()
        {
            FolderPath = (ContentType == Helpers.Helper.Enums.ContentType.anime) ?
                System.IO.Path.Combine(Properties.Settings.Default.pref_PastaAnimes, Helpers.Helper.RetirarCaracteresInvalidos(Title))
                : System.IO.Path.Combine(Properties.Settings.Default.pref_PastaSeries, Helpers.Helper.RetirarCaracteresInvalidos(Title));
        }

        //public Video ToVideo()
        //{
        //    Video video = new Serie();

        //    video.FolderPath = FolderPath;
        //    video.IDBanco = IDBanco;
        //    video.IDApi = IDApi;
        //    video.ImgFanart = ImgFanart;
        //    video.ImgPoster = ImgPoster;
        //    video.Language = Language;
        //    video.LastUpdated = LastUpdated;
        //    video.Overview = Overview;
        //    video.Title = Title;
        //    video.ContentType = ContentType;

        //    return video;
        //}
    }

    [XmlRoot("Data", Namespace = "", IsNullable = false)]
    public class SeriesData
    {
        [XmlElement("Episode")]
        public Episode[] Episodes { get; set; }

        [XmlElement("Series")]
        public Serie[] Series { get; set; }
    }
}