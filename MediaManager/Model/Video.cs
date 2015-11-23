using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MediaManager.Helpers;
using MediaManager.Properties;

namespace MediaManager.Model
{
    [DebuggerDisplay("{nCdApi} - {sDsTitulo} - {sDsIdioma}")]
    public abstract class Video : System.ComponentModel.INotifyPropertyChanged
    {
        private string _sAliases;

        [XmlIgnore, NotMapped]
        public virtual string sAliases { get { return _sAliases; } set { _sAliases = value; OnPropertyChanged(); } }

        private ObservableCollection<SerieAlias> _ListaSerieAlias;

        [XmlIgnore]
        public ObservableCollection<SerieAlias> ListaSerieAlias { get { return _ListaSerieAlias; } set { _ListaSerieAlias = value; OnPropertyChanged(); } }

        private Enums.TipoConteudo _nIdTipoConteudo;

        [NotMapped, XmlIgnore]
        public virtual Enums.TipoConteudo nIdTipoConteudo { get { return _nIdTipoConteudo; } set { _nIdTipoConteudo = value; OnPropertyChanged(); } }

        [NotMapped, XmlIgnore]
        public virtual string sDsTipoConteudo { get { return nIdTipoConteudo.ToString(); } }

        [NotMapped, XmlIgnore]
        public Enums.Estado nIdEstado { get; set; }

        [XmlIgnore]
        public virtual string sDsMetadata
        {
            get
            {
                switch (nIdTipoConteudo)
                {
                    case Enums.TipoConteudo.Filme:
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.Default.AppName, "Metadata", "Filmes", Helper.RetirarCaracteresInvalidos(sDsTitulo));

                    case Enums.TipoConteudo.Série:
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.Default.AppName, "Metadata", "Séries", Helper.RetirarCaracteresInvalidos(sDsTitulo));

                    case Enums.TipoConteudo.Anime:
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.Default.AppName, "Metadata", "Animes", Helper.RetirarCaracteresInvalidos(sDsTitulo));

                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException();
                }
            }
        }

        private string _sDsPasta;

        [XmlIgnore]
        public virtual string sDsPasta { get { return _sDsPasta; } set { _sDsPasta = value; OnPropertyChanged(); } }

        [XmlIgnore]
        public virtual int nCdApi { get; set; }

        [XmlIgnore]
        public virtual int nCdVideo { get; set; }

        private string _sDsImgFanart;

        [XmlIgnore]
        public virtual string sDsImgFanart
        {
            get { return _sDsImgFanart; }
            set
            {
                _sDsImgFanart = string.IsNullOrWhiteSpace(value)
                    ? ("pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png")
                    : value;
                OnPropertyChanged();
            }
        }

        private string _sDsImgPoster;

        [XmlIgnore]
        public virtual string sDsImgPoster
        {
            get { return _sDsImgPoster; }
            set
            {
                _sDsImgPoster = string.IsNullOrWhiteSpace(value)
                    ? ("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                    : value;
                OnPropertyChanged();

                if (sDsImgPoster.StartsWith("http"))
                {
                    BitmapImage bmp = new BitmapImage(new Uri(value));
                    bmp.DownloadCompleted += (s, e) =>
                    {
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bmp));
                        using (MemoryStream ms = new MemoryStream())
                        {
                            encoder.Save(ms);
                            bCacheImgPoster = ms.ToArray();
                        }
                    };
                }
                else
                {
                    bCacheImgPoster = (sDsImgPoster == "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                                            ? (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]))
                                            : File.ReadAllBytes(sDsImgPoster);
                }
            }
        }

        private byte[] _bCacheImgPoster;

        [XmlIgnore, NotMapped]
        public byte[] bCacheImgPoster { get { return _bCacheImgPoster; } set { _bCacheImgPoster = value; OnPropertyChanged(); } }

        [XmlIgnore]
        public virtual string sDsIdioma { get; set; }

        [XmlIgnore]
        public virtual string sNrUltimaAtualizacao { get; set; }

        private string _sDsSinopse;

        [XmlIgnore]
        public virtual string sDsSinopse { get { return _sDsSinopse; } set { _sDsSinopse = value; OnPropertyChanged(); } }

        private string _sDsTitulo;

        [XmlIgnore]
        public virtual string sDsTitulo { get { return _sDsTitulo; } set { _sDsTitulo = value; OnPropertyChanged(); } }

        private bool _bFlSelecionado;

        [XmlIgnore, NotMapped]
        public bool bFlSelecionado { get { return _bFlSelecionado; } set { _bFlSelecionado = value; OnPropertyChanged(); } }

        public Video()
        {
            _sDsImgFanart = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
            _sDsImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
        }

        public void Clone(object objOrigem)
        {
            PropertyInfo[] variaveisObjOrigem = objOrigem.GetType().GetProperties();
            PropertyInfo[] variaveisObjAtual = GetType().GetProperties();

            foreach (PropertyInfo item in variaveisObjOrigem)
            {
                PropertyInfo variavelIgual = variaveisObjAtual.FirstOrDefault(x => x.Name == item.Name && x.PropertyType == item.PropertyType);

                if (variavelIgual != null && variavelIgual.CanWrite)
                {
                    variavelIgual.SetValue(this, item.GetValue(objOrigem, null));
                }
            }

            return;
        }

        private void SetDefaultFolderPath()
        {
            switch (nIdTipoConteudo)
            {
                case Enums.TipoConteudo.Filme:
                    if (!string.IsNullOrWhiteSpace(Settings.Default.pref_PastaFilmes))
                        sDsPasta = Path.Combine(Settings.Default.pref_PastaFilmes, Helper.RetirarCaracteresInvalidos(sDsTitulo));
                    break;

                case Enums.TipoConteudo.Série:
                    if (!string.IsNullOrWhiteSpace(Settings.Default.pref_PastaSeries))
                        sDsPasta = Path.Combine(Settings.Default.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(sDsTitulo));
                    break;

                case Enums.TipoConteudo.Anime:
                    if (!string.IsNullOrWhiteSpace(Settings.Default.pref_PastaAnimes))
                        sDsPasta = Path.Combine(Settings.Default.pref_PastaAnimes, Helper.RetirarCaracteresInvalidos(sDsTitulo));
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