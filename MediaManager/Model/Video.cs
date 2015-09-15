using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Xml.Serialization;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    /// <summary>
    /// NOVO = Recém criado (new Video()), Simples = Video com as informações basicas vindas da API, Completo = Video com as informações completas vindas da API.
    /// </summary>
    public enum Estado
    {
        Novo,
        Simples,
        Completo,
        CompletoSemForeignKeys
    }

    [DebuggerDisplay("{IDApi} - {Title} - {Language}")]
    public abstract class Video : System.ComponentModel.INotifyPropertyChanged
    {
        private Enums.ContentType _ContentType;
        private string _FolderPath;
        private string _ImgFanart = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
        private string _ImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
        private string _Overview;
        private string _Title;
        private string _SerieAliasStr;
        private ObservableCollection<SerieAlias> _SerieAlias;

        [XmlIgnore, NotMapped]
        public virtual string SerieAliasStr
        {
            get { return _SerieAliasStr; }
            set
            {
                _SerieAliasStr = value;
                //if (!string.IsNullOrWhiteSpace(value))
                //{
                //    foreach (var item in value.Split('|'))
                //    {
                //        SerieAlias alias = new SerieAlias(item);
                //        if (SerieAlias != null)
                //            SerieAlias.Add(alias);
                //        else
                //        {
                //            SerieAlias = new ObservableCollection<SerieAlias>();
                //            SerieAlias.Add(alias);
                //        }
                //    }
                //}
                OnPropertyChanged("SerieAliasStr");
            }
        }

        [XmlIgnore]
        public ObservableCollection<SerieAlias> SerieAlias { get { return _SerieAlias; } set { _SerieAlias = value; OnPropertyChanged("SerieAlias"); } }

        [NotMapped, XmlIgnore]
        public virtual Enums.ContentType ContentType { get { return _ContentType; } set { _ContentType = value; OnPropertyChanged("ContentType"); } }

        [NotMapped, XmlIgnore]
        public virtual string ContentTypeString { get { return Enums.ToString(ContentType); } }

        [NotMapped, XmlIgnore]
        public Estado Estado { get; set; }

        [XmlIgnore]
        public virtual string FolderMetadata
        {
            get
            {
                switch (ContentType)
                {
                    case Enums.ContentType.movie:
                        return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata",
                            "Filmes", Helpers.Helper.RetirarCaracteresInvalidos(Title));

                    case Enums.ContentType.show:
                        return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata",
                            "Séries", Helpers.Helper.RetirarCaracteresInvalidos(Title));

                    case Enums.ContentType.anime:
                        return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata",
                            "Animes", Helpers.Helper.RetirarCaracteresInvalidos(Title));

                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException();
                }
            }
        }

        [XmlIgnore]
        public virtual string FolderPath { get { return _FolderPath; } set { _FolderPath = value; OnPropertyChanged("FolderPath"); } }

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
                OnPropertyChanged("ImgFanart");
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
                OnPropertyChanged("ImgPoster");
            }
        }

        [XmlIgnore]
        public virtual string Language { get; set; }

        [XmlIgnore]
        public virtual string LastUpdated { get; set; }

        [XmlIgnore]
        public virtual string Overview { get { return _Overview; } set { _Overview = value; OnPropertyChanged("Overview"); } }

        [XmlIgnore]
        public virtual string Title { get { return _Title; } set { _Title = value; OnPropertyChanged("Title"); } }

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members

        public abstract void Clone(object objectToClone);

        private void SetDefaultFolderPath()
        {
            switch (ContentType)
            {
                case Enums.ContentType.movie:
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_PastaFilmes))
                        FolderPath = System.IO.Path.Combine(Properties.Settings.Default.pref_PastaFilmes, Helpers.Helper.RetirarCaracteresInvalidos(Title));
                    break;

                case Enums.ContentType.show:
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_PastaSeries))
                        FolderPath = System.IO.Path.Combine(Properties.Settings.Default.pref_PastaSeries, Helpers.Helper.RetirarCaracteresInvalidos(Title));
                    break;

                case Enums.ContentType.anime:
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_PastaAnimes))
                        FolderPath = System.IO.Path.Combine(Properties.Settings.Default.pref_PastaAnimes, Helpers.Helper.RetirarCaracteresInvalidos(Title));
                    break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }
    }
}