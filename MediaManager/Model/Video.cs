using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MediaManager.Helpers;
using MediaManager.Properties;

namespace MediaManager.Model
{
    [DebuggerDisplay("{IDApi} - {Title} - {Language}")]
    public abstract class Video : System.ComponentModel.INotifyPropertyChanged
    {
        private Enums.ContentType _ContentType;
        private string _FolderPath;
        private string _ImgFanart;
        private string _ImgPoster;
        private string _Overview;
        private string _Title;
        private string _SerieAliasStr;
        private ObservableCollection<SerieAlias> _SerieAlias;

        [XmlIgnore, NotMapped]
        public virtual string SerieAliasStr { get { return _SerieAliasStr; } set { _SerieAliasStr = value; OnPropertyChanged(); } }

        [XmlIgnore]
        public ObservableCollection<SerieAlias> SerieAlias { get { return _SerieAlias; } set { _SerieAlias = value; OnPropertyChanged(); } }

        [NotMapped, XmlIgnore]
        public virtual Enums.ContentType ContentType { get { return _ContentType; } set { _ContentType = value; OnPropertyChanged(); } }

        [NotMapped, XmlIgnore]
        public virtual string ContentTypeString { get { return Enums.ToString(ContentType); } }

        [NotMapped, XmlIgnore]
        public Enums.Estado Estado { get; set; }

        [XmlIgnore]
        public virtual string FolderMetadata
        {
            get
            {
                switch (ContentType)
                {
                    case Enums.ContentType.Filme:
                        return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata",
                            "Filmes", Helper.RetirarCaracteresInvalidos(Title));

                    case Enums.ContentType.Série:
                        return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata",
                            "Séries", Helper.RetirarCaracteresInvalidos(Title));

                    case Enums.ContentType.Anime:
                        return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata",
                            "Animes", Helper.RetirarCaracteresInvalidos(Title));

                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException();
                }
            }
        }

        [XmlIgnore]
        public virtual string FolderPath { get { return _FolderPath; } set { _FolderPath = value; OnPropertyChanged(); } }

        [XmlIgnore]
        public virtual int IDApi { get; set; }

        [XmlIgnore]
        public virtual int IDBanco { get; set; }

        [XmlIgnore]
        public virtual string ImgFanart
        {
            get { return _ImgFanart; }
            set
            {
                _ImgFanart = string.IsNullOrWhiteSpace(value)
                    ? ("pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png")
                    : value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public virtual string ImgPoster
        {
            get { return _ImgPoster; }
            set
            {
                _ImgPoster = string.IsNullOrWhiteSpace(value)
                    ? ("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                    : value;

                if (File.Exists(value) || value == "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                {
                    ImgPosterCache = ImgPoster == "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png"
                        ? (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]))
                        : File.ReadAllBytes(ImgPoster);
                }

                OnPropertyChanged();
            }
        }

        private byte[] _ImgPosterCache;

        public byte[] ImgPosterCache { get { return _ImgPosterCache; } private set { _ImgPosterCache = value; OnPropertyChanged(); } }

        [XmlIgnore]
        public virtual string Language { get; set; }

        [XmlIgnore]
        public virtual string LastUpdated { get; set; }

        [XmlIgnore]
        public virtual string Overview { get { return _Overview; } set { _Overview = value; OnPropertyChanged(); } }

        [XmlIgnore]
        public virtual string Title { get { return _Title; } set { _Title = value; OnPropertyChanged(); } }

        public Video()
        {
            _ImgFanart = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
            _ImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
            _ImgPosterCache = (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]));
        }

        public abstract void Clone(object objectToClone);

        private void SetDefaultFolderPath()
        {
            switch (ContentType)
            {
                case Enums.ContentType.Filme:
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_PastaFilmes))
                        FolderPath = System.IO.Path.Combine(Properties.Settings.Default.pref_PastaFilmes, Helper.RetirarCaracteresInvalidos(Title));
                    break;

                case Enums.ContentType.Série:
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_PastaSeries))
                        FolderPath = System.IO.Path.Combine(Properties.Settings.Default.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(Title));
                    break;

                case Enums.ContentType.Anime:
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_PastaAnimes))
                        FolderPath = System.IO.Path.Combine(Properties.Settings.Default.pref_PastaAnimes, Helper.RetirarCaracteresInvalidos(Title));
                    break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}