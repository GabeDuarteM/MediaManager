using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    [DebuggerDisplay("{SeasonNumber}x{EpisodeNumber} ({AbsoluteNumber}) - {EpisodeName}")]
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

        [XmlIgnore, NotMapped]
        private bool _isSelected;

        [XmlIgnore, NotMapped]
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; OnPropertyChanged(); } }

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

        private Enums.EstadoEpisodio _EstadoEpisodio;

        public Enums.EstadoEpisodio EstadoEpisodio { get { return _EstadoEpisodio; } set { _EstadoEpisodio = value; OnPropertyChanged(); } }

        [XmlIgnore]
        public string FilePath { get { return _FilePath; } set { _FilePath = value; OnPropertyChanged(); } }

        [XmlIgnore]
        public DateTime? FirstAired
        {
            get { return _FirstAired != "" ? DateTime.Parse(_FirstAired) : default(DateTime?); }
            set { _FirstAired = value.ToString(); }
        }

        [XmlIgnore]
        public string FolderPath { get { return _FolderPath; } set { _FolderPath = value; OnPropertyChanged(); } }

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
        public Serie Serie { get; set; }

        [XmlIgnore]
        public DateTime? ThumbAddedDate
        {
            get { return _ThumbAddedDate != "" ? DateTime.Parse(_ThumbAddedDate) : default(DateTime?); }
            set { _ThumbAddedDate = value.ToString(); }
        }

        public Episode()
        {
            EstadoEpisodio = Enums.EstadoEpisodio.Ignorado;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}