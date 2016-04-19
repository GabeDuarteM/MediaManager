using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MediaManager.Helpers;
using MediaManager.Properties;

namespace MediaManager.Model
{
    [DebuggerDisplay("{nCdApi} - {sDsTitulo} - {sDsIdioma}")]
    public abstract class Video : ModelBase
    {
        private byte[] _bCacheImgPoster;

        private bool _bFlNaoEncontrado;

        private bool _bFlSelecionado;

        private ObservableCollection<SerieAlias> _lstSerieAlias;

        private Enums.TipoConteudo _nIdTipoConteudo;
        private string _sAliases;

        private string _sDsImgFanart;

        private string _sDsImgPoster;

        private string _sDsPasta;

        private string _sDsSinopse;

        private string _sDsTitulo;

        private string _sFormatoRenomeioPersonalizado;

        public Video()
        {
            _sDsImgFanart = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
            _sDsImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
        }

        [XmlIgnore, NotMapped]
        public virtual string sAliases
        {
            get { return _sAliases; }
            set
            {
                _sAliases = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public ObservableCollection<SerieAlias> lstSerieAlias
        {
            get { return _lstSerieAlias; }
            set
            {
                _lstSerieAlias = value;
                OnPropertyChanged();
            }
        }

        [NotMapped, XmlIgnore]
        public virtual Enums.TipoConteudo nIdTipoConteudo
        {
            get { return _nIdTipoConteudo; }
            set
            {
                _nIdTipoConteudo = value;
                OnPropertyChanged();
            }
        }

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
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            Settings.Default.AppName, "Metadata", "Filmes", Helper.RetirarCaracteresInvalidos(sDsTitulo));

                    case Enums.TipoConteudo.Série:
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            Settings.Default.AppName, "Metadata", "Séries", Helper.RetirarCaracteresInvalidos(sDsTitulo));

                    case Enums.TipoConteudo.Anime:
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            Settings.Default.AppName, "Metadata", "Animes", Helper.RetirarCaracteresInvalidos(sDsTitulo));

                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException();
                }
            }
        }

        [XmlIgnore]
        public virtual string sDsPasta
        {
            get { return _sDsPasta; }
            set
            {
                _sDsPasta = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public virtual int nCdApi { get; set; }

        [XmlIgnore]
        public virtual int nCdVideo { get; set; }

        [XmlIgnore]
        public virtual string sDsImgFanart
        {
            get { return _sDsImgFanart; }
            set
            {
                _sDsImgFanart = string.IsNullOrWhiteSpace(value)
                    ? "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png"
                    : value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public virtual string sDsImgPoster
        {
            get { return _sDsImgPoster; }
            set
            {
                _sDsImgPoster = string.IsNullOrWhiteSpace(value)
                    ? "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png"
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
                    bCacheImgPoster = sDsImgPoster ==
                                      "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png"
                        ? (byte[]) new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]))
                        : File.ReadAllBytes(sDsImgPoster);
                }
            }
        }

        [XmlIgnore, NotMapped]
        public byte[] bCacheImgPoster
        {
            get { return _bCacheImgPoster; }
            set
            {
                _bCacheImgPoster = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public virtual string sDsIdioma { get; set; }

        [XmlIgnore]
        public virtual string sNrUltimaAtualizacao { get; set; }

        [XmlIgnore]
        public virtual string sDsSinopse
        {
            get { return _sDsSinopse; }
            set
            {
                _sDsSinopse = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public virtual string sDsTitulo
        {
            get { return _sDsTitulo; }
            set
            {
                _sDsTitulo = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore, NotMapped]
        public bool bFlSelecionado
        {
            get { return _bFlSelecionado; }
            set
            {
                _bFlSelecionado = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore, NotMapped]
        public bool bFlNaoEncontrado
        {
            get { return _bFlNaoEncontrado; }
            set
            {
                _bFlNaoEncontrado = value;
                OnPropertyChanged();
                if (value == true)
                {
                    sDsTitulo = "Sem resultados...";
                    sDsSinopse = "Sem resultados...";
                }
            }
        }

        public string sFormatoRenomeioPersonalizado
        {
            get { return _sFormatoRenomeioPersonalizado; }
            set
            {
                _sFormatoRenomeioPersonalizado = value;
                OnPropertyChanged();
            }
        }

        private void SetDefaultFolderPath()
        {
            switch (nIdTipoConteudo)
            {
                case Enums.TipoConteudo.Filme:
                    if (!string.IsNullOrWhiteSpace(Settings.Default.pref_PastaFilmes))
                        sDsPasta = Path.Combine(Settings.Default.pref_PastaFilmes,
                            Helper.RetirarCaracteresInvalidos(sDsTitulo));
                    break;

                case Enums.TipoConteudo.Série:
                    if (!string.IsNullOrWhiteSpace(Settings.Default.pref_PastaSeries))
                        sDsPasta = Path.Combine(Settings.Default.pref_PastaSeries,
                            Helper.RetirarCaracteresInvalidos(sDsTitulo));
                    break;

                case Enums.TipoConteudo.Anime:
                    if (!string.IsNullOrWhiteSpace(Settings.Default.pref_PastaAnimes))
                        sDsPasta = Path.Combine(Settings.Default.pref_PastaAnimes,
                            Helper.RetirarCaracteresInvalidos(sDsTitulo));
                    break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }
    }
}