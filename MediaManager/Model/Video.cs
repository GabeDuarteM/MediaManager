namespace MediaManager.Model
{
    /// <summary>
    /// NOVO = Recém criado (new Video()), Simples = Video com as informações basicas vindas da API, Completo = Video com as informações completas vindas da API.
    /// </summary>
    public enum Estado
    {
        NOVO,
        SIMPLES,
        COMPLETO
    }

    [System.Diagnostics.DebuggerDisplay("{IDApi} - {Title} - {Language}")]
    public abstract class Video : System.ComponentModel.INotifyPropertyChanged
    {
        private Helpers.Helper.Enums.ContentType _ContentType;
        private string _FolderPath;
        private string _ImgFanart = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
        private string _ImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
        private string _Overview;
        private string _Title;

        [System.Xml.Serialization.XmlIgnore]
        public string AliasNames { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped, System.Xml.Serialization.XmlIgnore]
        public virtual Helpers.Helper.Enums.ContentType ContentType { get { return _ContentType; } set { _ContentType = value; OnPropertyChanged("ContentType"); } }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped, System.Xml.Serialization.XmlIgnore]
        public virtual string ContentTypeString { get { return Helpers.Helper.Enums.ToString(ContentType); } }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped, System.Xml.Serialization.XmlIgnore]
        public Estado Estado { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public virtual string FolderMetadata
        {
            get
            {
                switch (ContentType)
                {
                    case Helpers.Helper.Enums.ContentType.movie:
                        return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata",
                            "Filmes", Helpers.Helper.RetirarCaracteresInvalidos(Title));

                    case Helpers.Helper.Enums.ContentType.show:
                        return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata",
                            "Séries", Helpers.Helper.RetirarCaracteresInvalidos(Title));

                    case Helpers.Helper.Enums.ContentType.anime:
                        return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata",
                            "Animes", Helpers.Helper.RetirarCaracteresInvalidos(Title));

                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException();
                }
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public virtual string FolderPath { get { return _FolderPath; } set { _FolderPath = value; OnPropertyChanged("FolderPath"); } }

        [System.Xml.Serialization.XmlIgnore]
        public virtual int IDApi { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public virtual int IDBanco { get; set; }

        [System.Xml.Serialization.XmlIgnore]
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

        [System.Xml.Serialization.XmlIgnore]
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

        [System.Xml.Serialization.XmlIgnore]
        public virtual string Language { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public virtual string LastUpdated { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public virtual string Overview { get { return _Overview; } set { _Overview = value; OnPropertyChanged("Overview"); } }

        [System.Xml.Serialization.XmlIgnore]
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
                case Helpers.Helper.Enums.ContentType.movie:
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_PastaFilmes))
                        FolderPath = System.IO.Path.Combine(Properties.Settings.Default.pref_PastaFilmes, Helpers.Helper.RetirarCaracteresInvalidos(Title));
                    break;

                case Helpers.Helper.Enums.ContentType.show:
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_PastaSeries))
                        FolderPath = System.IO.Path.Combine(Properties.Settings.Default.pref_PastaSeries, Helpers.Helper.RetirarCaracteresInvalidos(Title));
                    break;

                case Helpers.Helper.Enums.ContentType.anime:
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.pref_PastaAnimes))
                        FolderPath = System.IO.Path.Combine(Properties.Settings.Default.pref_PastaAnimes, Helpers.Helper.RetirarCaracteresInvalidos(Title));
                    break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }
    }
}