using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using MediaManager.Helpers;

namespace MediaManager.Model
{
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
        private Enums.ContentType _ContentType = Enums.ContentType.Série;

        private string _ImgFanart = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
        private string _ImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";

        [XmlIgnore]
        private bool _IsAnime;

        [XmlIgnore]
        private string _Overview;

        [XmlIgnore]
        private string _Title;

        [XmlElement("Actors", IsNullable = true)]
        public string Actors { get; set; }

        [XmlElement("Airs_DayOfWeek", IsNullable = true)]
        public string Airs_DayOfWeek { get; set; }

        [XmlElement("Airs_Time", IsNullable = true)]
        public string Airs_Time { get; set; }

        [XmlElement("AliasNames")]
        public override string SerieAliasStr { get; set; }

        [XmlElement("ContentRating", IsNullable = true)]
        public string ContentRating { get; set; }

        [NotMapped]
        public override Enums.ContentType ContentType { get { return _ContentType; } set { _ContentType = value; OnPropertyChanged("ContentType"); _IsAnime = value == Enums.ContentType.Anime ? true : false; } }

        [XmlIgnore]
        public List<Episode> Episodes { get; set; }

        [XmlIgnore]
        public DateTime? FirstAired { get { return DateTime.Parse(_FirstAired); } set { _FirstAired = value.ToString(); } }

        [XmlElement("Genre", IsNullable = true)]
        public string Genre { get; set; }

        [XmlElement("id"), Column(Order = 2)]
        public override int IDApi { get; set; }

        private int _IDBanco;

        [XmlIgnore, Key, Column("ID", Order = 0)]
        public override int IDBanco { get { return _IDBanco; } set { _IDBanco = value; /*if (value > 0 && AliasNames == null) AliasNames = new ObservableCollection<SerieAlias>(DBHelper.GetSerieAliases(this));*/ } }

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
                OnPropertyChanged();
            }
        }

        [XmlElement("poster", IsNullable = true)]
        public override string ImgPoster
        {
            get { return _ImgPoster; }
            set
            {
                if (value.StartsWith("http"))
                {
                    _ImgPoster = value;
                }
                else if (File.Exists(value))
                {
                    _ImgPoster = value;
                    OnPropertyChanged();
                }
                else
                {
                    _ImgPoster = string.IsNullOrWhiteSpace(value) ?
                        _ImgPoster = ("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                        : Properties.Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                    OnPropertyChanged();
                }
            }
        }

        [XmlIgnore]
        public bool IsAnime { get { return _IsAnime; } set { _IsAnime = value; _ContentType = (value == true) ? Enums.ContentType.Anime : ContentType; } }

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

        public Serie()
        {
        }

        public Serie(PosterGrid posterGrid)
        {
            SerieAliasStr = posterGrid.SerieAliasStr;
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
            SerieAlias = posterGrid.SerieAlias;
        }

        public override void Clone(object objectToClone)
        {
            Serie serie = objectToClone as Serie;

            Actors = serie.Actors;
            Airs_DayOfWeek = serie.Airs_DayOfWeek;
            Airs_Time = serie.Airs_Time;
            SerieAliasStr = serie.SerieAliasStr;
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
            SerieAlias = serie.SerieAlias;
        }

        public void SetDefaultFolderPath()
        {
            FolderPath = (ContentType == Enums.ContentType.Anime) ?
                System.IO.Path.Combine(Properties.Settings.Default.pref_PastaAnimes, Helper.RetirarCaracteresInvalidos(Title))
                : System.IO.Path.Combine(Properties.Settings.Default.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(Title));
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
}