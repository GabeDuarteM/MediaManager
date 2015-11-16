using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MediaManager.Helpers;
using MediaManager.Properties;

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
        public override Enums.ContentType ContentType { get { return _ContentType; } set { _ContentType = value; OnPropertyChanged(); _IsAnime = (value == Enums.ContentType.Anime) ? true : false; } }

        [XmlIgnore]
        public List<Episode> Episodes { get; set; }

        [XmlIgnore]
        public DateTime? FirstAired { get { return (string.IsNullOrWhiteSpace(_FirstAired)) ? (DateTime?)null : DateTime.Parse(_FirstAired); } set { _FirstAired = value.ToString(); } }

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
                    OnPropertyChanged();

                    BitmapImage bmp = new BitmapImage(new Uri(value));
                    BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        ImgPosterCache = ms.ToArray();
                    }
                }
                else if (File.Exists(value))
                {
                    _ImgPoster = value;
                    OnPropertyChanged();
                    ImgPosterCache = (ImgPoster == "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                                            ? (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]))
                                            : File.ReadAllBytes(ImgPoster);
                }
                else
                {
                    _ImgPoster = string.IsNullOrWhiteSpace(value) ?
                        _ImgPoster = ("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                        : Properties.Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                    OnPropertyChanged();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        BitmapImage bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.UriSource = new Uri(ImgPoster);
                        bmp.EndInit();

                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bmp));
                        encoder.Save(ms);
                        ImgPosterCache = ms.ToArray();
                    }
                }
            }
        }

        [XmlIgnore]
        public bool IsAnime { get { return _IsAnime; } set { _IsAnime = value; if (value == true) { _ContentType = Enums.ContentType.Anime; } } }

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
                return !string.IsNullOrWhiteSpace(_Rating) && double.TryParse(_Rating.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out retval) ? (double?)retval : null;
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

        private bool _bFlEditado;
        private bool _bFlNaoEncontrado;

        [XmlIgnore, NotMapped]
        public bool bFlEditado { get { return _bFlEditado; } set { _bFlEditado = value; OnPropertyChanged(); } }

        [XmlIgnore, NotMapped]
        public bool bFlNaoEncontrado { get { return _bFlNaoEncontrado; } set { _bFlNaoEncontrado = value; OnPropertyChanged(); if (value == true) { Title = "Sem resultados..."; Overview = "Sem resultados..."; } } }

        public void SetDefaultFolderPath()
        {
            FolderPath = (ContentType == Enums.ContentType.Anime) ?
                Path.Combine(Properties.Settings.Default.pref_PastaAnimes, Helper.RetirarCaracteresInvalidos(Title))
                : Path.Combine(Properties.Settings.Default.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(Title));
        }
    }
}