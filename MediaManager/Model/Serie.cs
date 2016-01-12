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
        [XmlElement("Actors", IsNullable = true)]
        public string sNmAtores { get; set; }

        [XmlElement("Airs_DayOfWeek", IsNullable = true)]
        public string sDsEstreiaDiaSemana { get; set; }

        [XmlElement("Airs_Time", IsNullable = true)]
        public string sDsEstreiaHorario { get; set; }

        [XmlElement("AliasNames")]
        public override string sAliases { get; set; }

        [XmlElement("ContentRating", IsNullable = true)]
        public string sDsClassificacao { get; set; }

        [XmlIgnore]
        private Enums.TipoConteudo _nIdTipoConteudo = Enums.TipoConteudo.Série;

        [NotMapped]
        public override Enums.TipoConteudo nIdTipoConteudo { get { return _nIdTipoConteudo; } set { _nIdTipoConteudo = value; OnPropertyChanged(); _bFlAnime = (value == Enums.TipoConteudo.Anime) ? true : false; } }

        [XmlIgnore]
        public List<Episodio> lstEpisodios { get; set; }

        [XmlElement("FirstAired")]
        public string _sDtEstreia;

        [XmlIgnore]
        public DateTime? tDtEstreia { get { return (string.IsNullOrWhiteSpace(_sDtEstreia)) ? (DateTime?)null : DateTime.Parse(_sDtEstreia); } set { _sDtEstreia = value.ToString(); } }

        [XmlElement("Genre", IsNullable = true)]
        public string sDsGeneros { get; set; }

        [XmlElement("id"), Column(Order = 2)]
        public override int nCdApi { get; set; }

        private int _nCdVideo;

        [XmlIgnore, Key, Column(Order = 0)]
        public override int nCdVideo { get { return _nCdVideo; } set { _nCdVideo = value; } }

        private string _sDsImgFanart;

        [XmlElement("fanart", IsNullable = true)]
        public override string sDsImgFanart
        {
            get { return _sDsImgFanart; }
            set
            {
                if (value.StartsWith("http"))
                    _sDsImgFanart = value;
                else
                {
                    _sDsImgFanart = string.IsNullOrWhiteSpace(value) ?
                        _sDsImgFanart = ("pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png")
                        : _sDsImgFanart = Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                }
                OnPropertyChanged();
            }
        }

        private string _sDsImgPoster;

        [XmlElement("poster", IsNullable = true)]
        public override string sDsImgPoster
        {
            get { return _sDsImgPoster; }
            set
            {
                try
                {
                    if (value.StartsWith("http"))
                    {
                        _sDsImgPoster = value;
                        OnPropertyChanged();

                        BitmapImage bmp = new BitmapImage(new Uri(value));
                        bmp.DownloadCompleted += (s, e) =>
                        {
                            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bmp));
                            using (MemoryStream ms = new MemoryStream())
                            {
                                encoder.Save(ms);
                                bCacheImgPoster = ms.ToArray();
                            }
                        };
                    }
                    else if (File.Exists(value))
                    {
                        _sDsImgPoster = value;
                        OnPropertyChanged();
                        bCacheImgPoster = (sDsImgPoster == "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                                                ? (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]))
                                                : File.ReadAllBytes(sDsImgPoster);
                    }
                    else
                    {
                        _sDsImgPoster = string.IsNullOrWhiteSpace(value) ?
                            _sDsImgPoster = ("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png")
                            : Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                        OnPropertyChanged();
                        MemoryStream ms = new MemoryStream();
                        BitmapImage bmp = new BitmapImage(new Uri(sDsImgPoster));
                        bmp.DownloadCompleted += (s, e) =>
                        {
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bmp));
                            encoder.Save(ms);
                            bCacheImgPoster = ms.ToArray();
                            ms.Close();
                        };
                    }
                }
                catch
                {
                    _sDsImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
                    bCacheImgPoster = (byte[])new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]));
                }
            }
        }

        [XmlIgnore]
        private bool _bFlAnime;

        [XmlIgnore]
        public bool bFlAnime { get { return _bFlAnime; } set { _bFlAnime = value; if (value == true) { _nIdTipoConteudo = Enums.TipoConteudo.Anime; } } }

        [XmlElement("Language")]
        public override string sDsIdioma { get; set; }

        [XmlElement("lastupdated", IsNullable = true)]
        public override string sNrUltimaAtualizacao { get; set; }

        [XmlElement("Network", IsNullable = true)]
        public string sDsEmissora { get; set; }

        [XmlIgnore]
        private string _sDsSinopse;

        [XmlElement("Overview", IsNullable = true)]
        public override string sDsSinopse { get { return _sDsSinopse; } set { _sDsSinopse = value; OnPropertyChanged(); } }

        [XmlElement("Rating")]
        public string _sNrAvaliacao;

        [XmlIgnore]
        public double? dNrAvaliacao
        {
            get
            {
                double retval;
                return !string.IsNullOrWhiteSpace(_sNrAvaliacao) && double.TryParse(_sNrAvaliacao.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out retval) ? (double?)retval : null;
            }
            set { _sNrAvaliacao = value.ToString(); }
        }

        [XmlElement("RatingCount")]
        public string _sQtAvaliacao;

        [XmlIgnore]
        public int? nQtAvaliacao
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sQtAvaliacao) && int.TryParse(_sQtAvaliacao, out retval) ? (int?)retval : null;
            }
            set { _sQtAvaliacao = value.ToString(); }
        }

        [XmlElement("Runtime")]
        public string _sQtDuracaoEpisodio;

        [XmlIgnore]
        public int? nQtDuracaoEpisodio
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sQtDuracaoEpisodio) && int.TryParse(_sQtDuracaoEpisodio, out retval) ? (int?)retval : null;
            }
            set { _sQtDuracaoEpisodio = value.ToString(); }
        }

        [XmlElement("Status", IsNullable = true)]
        public string sDsStatus { get; set; }

        [XmlIgnore]
        private string _sDsTitulo;

        [XmlElement("SeriesName", IsNullable = true), Column(Order = 1)]
        public override string sDsTitulo { get { return _sDsTitulo; } set { _sDsTitulo = value; OnPropertyChanged(); } }

        private bool _bFlEditado;

        [XmlIgnore, NotMapped]
        public bool bFlEditado { get { return _bFlEditado; } set { _bFlEditado = value; OnPropertyChanged(); } }

        private bool _bIsParado;

        public bool bIsParado { get { return _bIsParado; } set { _bIsParado = value; OnPropertyChanged(); } }

        public Serie()
        {
            _sDsImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
            _sDsImgFanart = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
        }

        public Serie(Serie serie)
        {
            Clone(serie);
        }

        public void SetDefaultFolderPath()
        {
            sDsPasta = (nIdTipoConteudo == Enums.TipoConteudo.Anime) ?
                Path.Combine(Settings.Default.pref_PastaAnimes, Helper.RetirarCaracteresInvalidos(sDsTitulo))
                : Path.Combine(Settings.Default.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(sDsTitulo));
        }

        public void SetEstadoEpisodio()
        {
            if (!bIsParado)
            {
                lstEpisodios.ForEach(x =>
                {
                    if (x.tDtEstreia > DateTime.Now && x.nIdEstadoEpisodio != Enums.EstadoEpisodio.Baixado)
                    {
                        x.nIdEstadoEpisodio = Enums.EstadoEpisodio.Desejado;
                    }
                });
            }
        }
    }
}