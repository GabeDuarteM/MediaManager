// Developed by: Gabriel Duarte
// 
// Created at: 26/07/2015 17:32
// Last update: 19/04/2016 02:47

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MediaManager.Helpers;
using MediaManager.Properties;

namespace MediaManager.Model
{
    public class Serie : Video
    {
        [XmlIgnore] private bool _bFlAnime;

        private bool _bFlEditado;

        private bool _bIsParado;

        [XmlIgnore] private Enums.TipoConteudo _nIdTipoConteudo = Enums.TipoConteudo.Série;

        private string _sDsImgFanart;

        private string _sDsImgPoster;

        [XmlIgnore] private string _sDsSinopse;

        [XmlIgnore] private string _sDsTitulo;

        [XmlElement("AliasNames")]
        public override string sAliases { get; set; }
        [XmlElement("ContentRating", IsNullable = true)]
        public string sDsClassificacao { get; set; }

        [XmlElement("FirstAired")] public string _sDtEstreia;

        [XmlElement("Network", IsNullable = true)]
        public string sDsEmissora { get; set; }

        [XmlElement("Overview", IsNullable = true)]
        public override string sDsSinopse
        {
            get { return _sDsSinopse; }
            set
            {
                _sDsSinopse = value;
                OnPropertyChanged();
            }
        }

        [XmlElement("Rating")] public string _sNrAvaliacao;

        [XmlElement("RatingCount")] public string _sQtAvaliacao;

        [XmlElement("Runtime")] public string _sQtDuracaoEpisodio;

        public Serie()
        {
            _sDsImgPoster = "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png";
            _sDsImgFanart = "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png";
        }

        public Serie(Serie serie)
        {
            Clone(serie);
        }

        [XmlElement("Actors", IsNullable = true)]
        public string sNmAtores { get; set; }

        [XmlElement("Airs_DayOfWeek", IsNullable = true)]
        public string sDsEstreiaDiaSemana { get; set; }

        [XmlElement("Airs_Time", IsNullable = true)]
        public string sDsEstreiaHorario { get; set; }

        [NotMapped]
        public override Enums.TipoConteudo nIdTipoConteudo
        {
            get { return _nIdTipoConteudo; }
            set
            {
                _nIdTipoConteudo = value;
                OnPropertyChanged();
                _bFlAnime = value == Enums.TipoConteudo.Anime
                                ? true
                                : false;
            }
        }

        [XmlIgnore]
        public List<Episodio> lstEpisodios { get; set; }

        [XmlIgnore]
        public DateTime? tDtEstreia
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sDtEstreia)
                           ? (DateTime?) null
                           : DateTime.Parse(_sDtEstreia);
            }
            set { _sDtEstreia = value.ToString(); }
        }

        [XmlElement("Genre", IsNullable = true)]
        public string sDsGeneros { get; set; }

        [XmlElement("id"), Column(Order = 2)]
        public override int nCdApi { get; set; }

        [XmlIgnore, Key, Column(Order = 0)]
        public override int nCdVideo { get; set; }

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
                    _sDsImgFanart = string.IsNullOrWhiteSpace(value)
                                        ? _sDsImgFanart =
                                          "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png"
                                        : _sDsImgFanart = Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                }
                OnPropertyChanged();
            }
        }

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

                        var bmp = new BitmapImage(new Uri(value));
                        bmp.DownloadCompleted += (s, e) =>
                        {
                            var encoder = new BmpBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bmp));
                            using (var ms = new MemoryStream())
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
                        bCacheImgPoster = sDsImgPoster ==
                                          "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png"
                                              ? (byte[])
                                                new ImageConverter().ConvertTo(Resources.IMG_PosterDefault,
                                                                               typeof(byte[]))
                                              : File.ReadAllBytes(sDsImgPoster);
                    }
                    else
                    {
                        _sDsImgPoster = string.IsNullOrWhiteSpace(value)
                                            ? _sDsImgPoster =
                                              "pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png"
                                            : Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                        OnPropertyChanged();
                        var ms = new MemoryStream();
                        var bmp = new BitmapImage(new Uri(sDsImgPoster));
                        bmp.DownloadCompleted += (s, e) =>
                        {
                            var encoder = new JpegBitmapEncoder();
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
                    bCacheImgPoster =
                        (byte[]) new ImageConverter().ConvertTo(Resources.IMG_PosterDefault, typeof(byte[]));
                }
            }
        }

        [XmlIgnore]
        public bool bFlAnime
        {
            get { return _bFlAnime; }
            set
            {
                _bFlAnime = value;
                if (value)
                {
                    _nIdTipoConteudo = Enums.TipoConteudo.Anime;
                }
            }
        }

        [XmlElement("Language")]
        public override string sDsIdioma { get; set; }

        [XmlElement("lastupdated", IsNullable = true)]
        public override string sNrUltimaAtualizacao { get; set; }

        [XmlIgnore]
        public double? dNrAvaliacao
        {
            get
            {
                double retval;
                return !string.IsNullOrWhiteSpace(_sNrAvaliacao) &&
                       double.TryParse(_sNrAvaliacao.Replace(",", "."), NumberStyles.Number,
                                       CultureInfo.InvariantCulture, out retval)
                           ? (double?) retval
                           : null;
            }
            set { _sNrAvaliacao = value.ToString(); }
        }

        [XmlIgnore]
        public int? nQtAvaliacao
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sQtAvaliacao) && int.TryParse(_sQtAvaliacao, out retval)
                           ? (int?) retval
                           : null;
            }
            set { _sQtAvaliacao = value.ToString(); }
        }

        [XmlIgnore]
        public int? nQtDuracaoEpisodio
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sQtDuracaoEpisodio) && int.TryParse(_sQtDuracaoEpisodio, out retval)
                           ? (int?) retval
                           : null;
            }
            set { _sQtDuracaoEpisodio = value.ToString(); }
        }

        [XmlElement("Status", IsNullable = true)]
        public string sDsStatus { get; set; }

        [XmlElement("SeriesName", IsNullable = true), Column(Order = 1)]
        public override string sDsTitulo
        {
            get { return _sDsTitulo; }
            set
            {
                _sDsTitulo = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore, NotMapped]
        public bool bFlEditado
        {
            get { return _bFlEditado; }
            set
            {
                _bFlEditado = value;
                OnPropertyChanged();
            }
        }

        public bool bIsParado
        {
            get { return _bIsParado; }
            set
            {
                _bIsParado = value;
                OnPropertyChanged();
            }
        }

        public void SetDefaultFolderPath()
        {
            sDsPasta = nIdTipoConteudo == Enums.TipoConteudo.Anime
                           ? Path.Combine(Settings.Default.pref_PastaAnimes,
                                          Helper.RetirarCaracteresInvalidos(sDsTitulo))
                           : Path.Combine(Settings.Default.pref_PastaSeries,
                                          Helper.RetirarCaracteresInvalidos(sDsTitulo));
        }

        public void SetEstadoEpisodio()
        {
            if (!bIsParado)
            {
                //var oUltimoEpBaixado = lstEpisodios.LastOrDefault(x => x.nIdEstadoEpisodio == Enums.EstadoEpisodio.Baixado);

                //if (oUltimoEpBaixado != null)
                //{
                //    lstEpisodios
                //        .Where(x => x.nNrTemporada >= oUltimoEpBaixado.nNrTemporada && x.nNrEpisodio > oUltimoEpBaixado.nNrEpisodio).ToList()
                //        .ForEach(x => x.nIdEstadoEpisodio = Enums.EstadoEpisodio.Desejado);
                //}

                lstEpisodios.ForEach(x =>
                {
                    if (x.tDtEstreia > DateTime.Now)
                    {
                        x.nIdEstadoEpisodio = Enums.EstadoEpisodio.Desejado;
                    }
                    else if (x.tDtEstreia != null)
                    {
                        x.nIdEstadoEpisodio = Enums.EstadoEpisodio.Ignorado;
                    }
                });
            }
        }
    }
}
