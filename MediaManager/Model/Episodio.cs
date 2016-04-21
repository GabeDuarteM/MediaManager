// Developed by: Gabriel Duarte
// 
// Created at: 25/01/2016 21:13

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Localizacao;
using MediaManager.Services;

namespace MediaManager.Model
{
    // ReSharper disable once LocalizableElement
    [DebuggerDisplay("{nNrTemporada}x{nNrEpisodio} ({nNrAbsoluto}) - {sDsEpisodio}")]
    public class Episodio : ModelBase
    {
        [XmlIgnore, NotMapped] private bool _bFlSelecionado;

        private Enums.EstadoEpisodio _nIdEstadoEpisodio;

        [XmlIgnore] private string _sDsFilepath;

        [XmlElement("EpisodeName"), Column(Order = 1)]
        public string sDsEpisodio { get; set; }
        [XmlElement("EpisodeNumber"), Column(Order = 2)]
        public int nNrEpisodio { get; set; }

        [XmlElement("FirstAired")] public string _sDtEstreia;

        [XmlIgnore] private string _sLkArtwork;

        [XmlElement("absolute_number")] public string _sNrAbsoluto;

        [XmlElement("lastupdated", IsNullable = true)]
        public string sNrUltimaAtualizacao { get; set; }
        [XmlElement("Overview", IsNullable = true)]
        public string sDsSinopse { get; set; }

        [XmlElement("Rating")] public string _sNrAvaliacao;

        [XmlElement("airsafter_season")] public string _sNrEstreiaDepoisTemporada;

        [XmlElement("airsbefore_episode")] public string _sNrEstreiaAntesEpisodio;

        [XmlElement("airsbefore_season")] public string _sNrEstreiaAntesTemporada;

        [XmlElement("RatingCount")] public string _sQtAvaliacao;

        public Episodio()
        {
            nIdEstadoEpisodio = Enums.EstadoEpisodio.Novo;
            lstIntEpisodios = new List<int>();
            lstIntEpisodiosAbsolutos = new List<int>();
        }

        public Episodio(Episodio episodio)
        {
            Clone(episodio);
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
        public Enums.TipoConteudo nIdTipoConteudo { get; set; }

        [XmlIgnore, Column(Order = 4)]
        public int? nNrAbsoluto
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sNrAbsoluto) && int.TryParse(_sNrAbsoluto, out retval)
                           ? (int?) retval
                           : null;
            }
            set { _sNrAbsoluto = value.ToString(); }
        }

        [XmlIgnore, NotMapped]
        public List<int> lstIntEpisodios { get; set; }

        // Para quando tiver mais de um episódio (nome.do.episodio.S00E00E01E02E03E04)

        [XmlIgnore, NotMapped]
        public List<int> lstIntEpisodiosAbsolutos { get; set; } // Para quando tiver mais de um episódio absoluto

        [XmlIgnore]
        public int? nNrEstreiaDepoisTemporada
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sNrEstreiaDepoisTemporada) &&
                       int.TryParse(_sNrEstreiaDepoisTemporada, out retval)
                           ? (int?) retval
                           : null;
            }
            set { _sNrEstreiaDepoisTemporada = value.ToString(); }
        }

        [XmlIgnore]
        public int? nNrEstreiaAntesEpisodio
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sNrEstreiaAntesEpisodio) &&
                       int.TryParse(_sNrEstreiaAntesEpisodio, out retval)
                           ? (int?) retval
                           : null;
            }
            set { _sNrEstreiaAntesEpisodio = value.ToString(); }
        }

        [XmlIgnore]
        public int? nNrEstreiaAntesTemporada
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sNrEstreiaAntesTemporada) &&
                       int.TryParse(_sNrEstreiaAntesTemporada, out retval)
                           ? (int?) retval
                           : null;
            }
            set { _sNrEstreiaAntesTemporada = value.ToString(); }
        }

        [XmlElement("filename", IsNullable = true)]
        public string sLkArtwork
        {
            get { return _sLkArtwork; }
            set
            {
                if (value.StartsWith("http"))
                {
                    _sLkArtwork = value;
                }
                else
                {
                    _sLkArtwork = string.IsNullOrWhiteSpace(value)
                                      ? "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png"
                                      : Properties.Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                }
            }
        }

        public Enums.EstadoEpisodio nIdEstadoEpisodio
        {
            get { return _nIdEstadoEpisodio; }
            set
            {
                _nIdEstadoEpisodio = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public DateTime? tDtEstreia
        {
            get
            {
                return _sDtEstreia != ""
                           ? DateTime.Parse(_sDtEstreia)
                           : default(DateTime?);
            }
            set { _sDtEstreia = value.ToString(); }
        }

        [XmlIgnore, Key, Column(Order = 0)]
        public int nCdEpisodio { get; set; }

        [XmlElement("id")]
        public int nCdEpisodioAPI { get; set; }

        [XmlElement("seasonid")]
        public int nCdTemporadaAPI { get; set; }

        [XmlIgnore, Required]
        public int nCdVideo { get; set; }

        [XmlElement("seriesid")]
        public int nCdVideoAPI { get; set; }

        [XmlIgnore]
        public bool bFlRenomeado { get; set; }

        [XmlElement("Language")]
        public string sDsIdioma { get; set; }

        [XmlIgnore]
        public string sDsFilepath
        {
            get { return _sDsFilepath; }
            set
            {
                _sDsFilepath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(sDsFilenameRenomeado));
            }
        }

        [XmlIgnore]
        public string sDsFilepathOriginal { get; set; }

        [XmlIgnore]
        public double? dNrAvaliacao
        {
            get
            {
                double retval;
                return !string.IsNullOrWhiteSpace(_sNrAvaliacao) &&
                       double.TryParse(_sNrAvaliacao, NumberStyles.Number, CultureInfo.InvariantCulture, out retval)
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

        [XmlElement("SeasonNumber"), Column(Order = 3)]
        public int nNrTemporada { get; set; }

        [Column(Order = 5), ForeignKey("nCdVideo")]
        public Serie oSerie { get; set; }

        public string sDsFilenameRenomeado => Path.GetFileName(sDsFilepath);

        public Enums.eQualidadeDownload eQualidadeDownload { get; set; }

        public bool IdentificarEpisodio()
        {
            if (oSerie == null)
            {
                oSerie = new Serie();
            }

            try
            {
                sDsFilepath = Helper.RetirarCaracteresInvalidos(sDsFilepath, false);

                string filenameTratado =
                    Path.GetFileNameWithoutExtension(sDsFilepath)
                        .Replace(".", " ")
                        .Replace("_", " ")
                        .Replace("'", "")
                        .Trim();

                if (Helper.RegexEpisodio.regex_S00E00.IsMatch(filenameTratado))
                {
                    Match match = Helper.RegexEpisodio.regex_S00E00.Match(filenameTratado);
                    string sDsTituloSerieTemp = oSerie.sDsTitulo;
                    oSerie.sDsTitulo =
                        match.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                    oSerie.sDsTitulo = Regex.Replace(oSerie.sDsTitulo, @"[^a-zA-Z0-9\s]", "");
                    // Retira caracteres especiais e deixa somente numero, letra e espaços. Facilita o reconhecimento.

                    // Para quando se tem multi-episódios
                    string[] separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value)
                                             ? default(string[])
                                             : new string[1] {match.Groups["separador"].Value};
                    nNrTemporada = int.Parse(match.Groups["season"].Value);
                    lstIntEpisodios = new List<int>();

                    foreach (
                        string item in
                            match.Groups["episodes"].Value.Split(separador, StringSplitOptions.RemoveEmptyEntries))
                    {
                        lstIntEpisodios.Add(int.Parse(Regex.Replace(item, @"[^\d]", "")));
                    }

                    nNrEpisodio = lstIntEpisodios[0];

                    bool retorno = SetarAtributosEpisodioIdentificado();

                    if (!string.IsNullOrWhiteSpace(sDsTituloSerieTemp))
                    {
                        oSerie.sDsTitulo = sDsTituloSerieTemp;
                    }

                    return retorno;
                }
                else if (Helper.RegexEpisodio.regex_0x00.IsMatch(filenameTratado)) // TODO Fazer funcionar com alias
                {
                    Match match = Helper.RegexEpisodio.regex_0x00.Match(filenameTratado);
                    string sDsTituloSerieTemp = oSerie.sDsTitulo;
                    oSerie.sDsTitulo =
                        match.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                    oSerie.sDsTitulo = Regex.Replace(oSerie.sDsTitulo, @"[^a-zA-Z0-9\s]", "");
                    // Retira caracteres especiais e deixa somente numero, letra e espaços. Facilita o reconhecimento.

                    // Para quando se tem multi-episódios
                    string[] separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value)
                                             ? default(string[])
                                             : new string[1] {match.Groups["separador"].Value};
                    nNrTemporada = int.Parse(match.Groups["season"].Value);
                    lstIntEpisodios = new List<int>();

                    foreach (
                        string item in
                            match.Groups["episodes"].Value.Split(separador, StringSplitOptions.RemoveEmptyEntries))
                    {
                        lstIntEpisodios.Add(int.Parse(Regex.Replace(item, @"[^\d]", "")));
                    }

                    nNrEpisodio = lstIntEpisodios[0];

                    bool retorno = SetarAtributosEpisodioIdentificado();

                    if (!string.IsNullOrWhiteSpace(sDsTituloSerieTemp))
                    {
                        oSerie.sDsTitulo = sDsTituloSerieTemp;
                    }

                    return retorno;
                }
                else if (Helper.RegexEpisodio.regex_Fansub0000.IsMatch(filenameTratado))
                {
                    Match match = Helper.RegexEpisodio.regex_Fansub0000.Match(filenameTratado);
                    string sDsTituloSerieTemp = oSerie.sDsTitulo;
                    oSerie.sDsTitulo =
                        match.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                    oSerie.sDsTitulo = Regex.Replace(oSerie.sDsTitulo, @"[^a-zA-Z0-9\s]", "");
                    // Retira caracteres especiais e deixa somente numero, letra e espaços. Facilita o reconhecimento.

                    // Para quando se tem multi-episódios
                    string[] separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value)
                                             ? default(string[])
                                             : new string[1] {match.Groups["separador"].Value};
                    lstIntEpisodiosAbsolutos = new List<int>();

                    foreach (
                        string item in
                            match.Groups["episodes"].Value.Split(separador, StringSplitOptions.RemoveEmptyEntries))
                    {
                        lstIntEpisodiosAbsolutos.Add(int.Parse(Regex.Replace(item, @"[^\d]", "")));
                    }

                    nNrAbsoluto = lstIntEpisodiosAbsolutos[0];

                    bool retorno = SetarAtributosEpisodioIdentificado();

                    if (!string.IsNullOrWhiteSpace(sDsTituloSerieTemp))
                    {
                        oSerie.sDsTitulo = sDsTituloSerieTemp;
                    }

                    return retorno;
                }
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(Mensagens.Ocorreu_um_erro_ao_reconhecer_o_episódio + sDsFilepath);
            }

            return false;
        }

        private bool SetarAtributosEpisodioIdentificado()
        {
            var serieAliasService = App.Container.Resolve<SerieAliasService>();
            var seriesService = App.Container.Resolve<SeriesService>();
            var episodiosService = App.Container.Resolve<EpisodiosService>();

            List<SerieAlias> lstAlias = serieAliasService.GetLista();
            SerieAlias alias =
                lstAlias.FirstOrDefault(
                                        x =>
                                        Regex.Replace(
                                                      x.sDsAlias.Replace(".", " ")
                                                       .Replace("_", " ")
                                                       .Replace("'", "")
                                                       .Trim(),
                                                      @"[^a-zA-Z0-9\s]", "") == oSerie.sDsTitulo);

            if (alias != null)
            {
                oSerie = seriesService.Get(alias.nCdVideo);
            }

            if (oSerie.nCdVideo == 0)
            {
                Serie oSerieTemp = seriesService.GetSerieOuAnimePorLevenshtein(oSerie.sDsTitulo);

                if (oSerieTemp != null)
                {
                    oSerie = oSerieTemp;
                }
            }

            if (oSerie.nCdVideo > 0)
            {
                List<Episodio> lstEpisodios = episodiosService.GetLista(oSerie);

                if (alias != null)
                {
                    Episodio episodio;
                    Episodio primeiroEpisodioAbsolutoAlias = null;

                    if (nNrAbsoluto > 0)
                    {
                        primeiroEpisodioAbsolutoAlias =
                            lstEpisodios.FirstOrDefault(
                                                        x =>
                                                        x.nNrTemporada == alias.nNrTemporada &&
                                                        x.nNrEpisodio == alias.nNrEpisodio);
                        if (primeiroEpisodioAbsolutoAlias != null)
                        {
                            episodio =
                                lstEpisodios.FirstOrDefault(
                                                            x =>
                                                            x.nNrAbsoluto ==
                                                            primeiroEpisodioAbsolutoAlias.nNrAbsoluto + nNrAbsoluto - 1);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        episodio =
                            lstEpisodios.FirstOrDefault(
                                                        x =>
                                                        x.nNrTemporada == alias.nNrTemporada + nNrTemporada - 1 &&
                                                        x.nNrEpisodio == alias.nNrEpisodio + nNrEpisodio - 1);
                    }

                    if (episodio == null) // Se não encontrou o episódio retorna falso.
                    {
                        return false;
                    }

                    episodio.lstIntEpisodios = lstIntEpisodios;
                    episodio.lstIntEpisodiosAbsolutos = lstIntEpisodiosAbsolutos;
                    episodio.oSerie = oSerie;
                    episodio.nIdTipoConteudo = episodio.oSerie.nIdTipoConteudo;

                    for (var i = 0;
                         i < (nNrAbsoluto > 0
                                  ? episodio.lstIntEpisodiosAbsolutos.Count
                                  : episodio.lstIntEpisodios.Count);
                         i++)
                    {
                        Episodio episodioTemp = null;

                        // Ajuste no numero do episodio caso este seja de um alias que não comece no primeiro episodio da primeira temporada.
                        if (nNrAbsoluto > 0)
                        {
                            episodio.lstIntEpisodiosAbsolutos[i] = (int) primeiroEpisodioAbsolutoAlias.nNrAbsoluto +
                                                                   episodio.lstIntEpisodiosAbsolutos[i] - 1;
                            episodioTemp =
                                lstEpisodios.FirstOrDefault(x => x.nNrAbsoluto == episodio.lstIntEpisodiosAbsolutos[i]);
                            if (episodioTemp != null)
                            {
                                episodio.lstIntEpisodios.Add(episodioTemp.nNrEpisodio);
                            }
                        }
                        else
                        {
                            episodio.lstIntEpisodios[i] = alias.nNrEpisodio + lstIntEpisodios[i] - 1;
                            episodioTemp =
                                lstEpisodios.FirstOrDefault(
                                                            x =>
                                                            x.nNrTemporada == episodio.nNrTemporada &&
                                                            x.nNrEpisodio == episodio.lstIntEpisodios[i]);
                            if (episodioTemp?.nNrAbsoluto != null)
                            {
                                episodio.lstIntEpisodiosAbsolutos.Add((int) episodioTemp.nNrAbsoluto);
                            }
                        }

                        if (i != 0 && episodioTemp != null)
                        {
                            episodio.sDsEpisodio += " & " + episodioTemp.sDsEpisodio;
                        }
                    }

                    Clone(episodio);
                }
                else
                {
                    Episodio episodio = nNrAbsoluto > 0
                                            ? lstEpisodios.FirstOrDefault(x => x.nNrAbsoluto == nNrAbsoluto)
                                            : lstEpisodios.FirstOrDefault(
                                                                          x =>
                                                                          x.nNrTemporada == nNrTemporada &&
                                                                          x.nNrEpisodio == nNrEpisodio);

                    if (episodio == null)
                    {
                        return false;
                    }

                    episodio.lstIntEpisodios = lstIntEpisodios;
                    episodio.lstIntEpisodiosAbsolutos = lstIntEpisodiosAbsolutos;
                    episodio.oSerie = oSerie;
                    episodio.nIdTipoConteudo = episodio.oSerie.nIdTipoConteudo;

                    if (nNrAbsoluto > 0) // TODO Testar pra ver o pq desse if :D
                    {
                        episodio.nNrAbsoluto = nNrAbsoluto;
                    }

                    for (var i = 0;
                         i < (nNrAbsoluto > 0
                                  ? episodio.lstIntEpisodiosAbsolutos.Count
                                  : episodio.lstIntEpisodios.Count);
                         i++)
                    {
                        Episodio episodioTemp;
                        // Ajuste no numero do episodio caso este seja de um alias que não comece no primeiro episodio da primeira temporada.
                        if (nNrAbsoluto > 0)
                        {
                            episodioTemp =
                                lstEpisodios.FirstOrDefault(x => x.nNrAbsoluto == episodio.lstIntEpisodiosAbsolutos[i]);
                            if (episodioTemp != null)
                            {
                                episodio.lstIntEpisodios.Add(episodioTemp.nNrEpisodio);
                            }
                        }
                        else
                        {
                            episodioTemp =
                                lstEpisodios.FirstOrDefault(
                                                            x =>
                                                            x.nNrTemporada == nNrTemporada &&
                                                            x.nNrEpisodio == episodio.lstIntEpisodios[i]);
                            // TODO Testar pra ver se funfa
                            if (episodioTemp?.nNrAbsoluto != null)
                            {
                                episodio.lstIntEpisodiosAbsolutos.Add((int) episodioTemp.nNrAbsoluto);
                            }
                        }

                        if (i != 0 && episodioTemp != null)
                        {
                            episodio.sDsEpisodio += " & " + episodioTemp.sDsEpisodio;
                        }
                    }

                    Clone(episodio);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EncaminharParaDownload(string link)
        {
            string path = Properties.Settings.Default.pref_PastaBlackhole;
            string nomeEpisodio = $"{oSerie.sDsTitulo} {nNrTemporada}x{nNrEpisodio}.torrent";
            if (!string.IsNullOrWhiteSpace(path))
            {
                var rgxHash = new Regex(@"magnet:.*?btih:(.*?)(?:&|$)");
                var rgxTitulo = new Regex(@"magnet:.*?[?:&]dn=(.*?)(?:&|$)");

                string fileName = null;
                string sLinkCache = null;

                try
                {
                    if (rgxHash.IsMatch(link))
                    {
                        string hash = rgxHash.Match(link).Groups[1].Value.ToUpper();

                        sLinkCache = @"http://torcache.net/torrent/" + hash + ".torrent";

                        if (rgxTitulo.IsMatch(link))
                        {
                            string sTituloTorrent = rgxTitulo.Match(link).Groups[1].Value;
                            fileName = HttpUtility.UrlDecode(sTituloTorrent);
                            if (!fileName.EndsWith(".torrent"))
                            {
                                fileName += ".torrent";
                            }
                        }
                    }

                    using (var wc = new Helper.MyWebClient())
                    {
                        byte[] data = Helper.Retry(() => wc.DownloadData(sLinkCache ?? link), TimeSpan.FromSeconds(3), 5);

                        // Try to extract the filename from the Content-Disposition header
                        if (string.IsNullOrWhiteSpace(fileName) &&
                            !string.IsNullOrEmpty(wc.ResponseHeaders["Content-Disposition"]))
                        {
                            fileName =
                                wc.ResponseHeaders["Content-Disposition"].Substring(
                                                                                    wc.ResponseHeaders[
                                                                                                       "Content-Disposition"
                                                                                        ].IndexOf("filename=") + 10)
                                                                         .Replace("\"", "");
                        }

                        string torrentPath = Path.Combine(path, fileName ?? nomeEpisodio);

                        if (File.Exists(torrentPath))
                        {
                            File.Delete(torrentPath);
                        }

                        File.WriteAllBytes(torrentPath, data);

                        return true;
                    }
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_baixar_o_episódio_do_link_0_, link));
                    return false;
                }
            }

            return false;
        }
    }
}
