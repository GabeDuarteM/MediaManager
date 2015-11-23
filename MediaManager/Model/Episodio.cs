using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    [DebuggerDisplay("{nNrTemporada}x{nNrEpisodio} ({sNrAbsoluto}) - {sDsEpisodio}")]
    public class Episodio : INotifyPropertyChanged
    {
        [XmlIgnore, NotMapped]
        private bool _bFlSelecionado;

        [XmlIgnore, NotMapped]
        public bool bFlSelecionado { get { return _bFlSelecionado; } set { _bFlSelecionado = value; OnPropertyChanged(); } }

        [XmlIgnore, NotMapped]
        public Enums.TipoConteudo nIdTipoConteudo { get; set; }

        [XmlElement("absolute_number")]
        public string _sNrAbsoluto;

        [XmlIgnore, Column(Order = 4)]
        public int? sNrAbsoluto
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sNrAbsoluto) && int.TryParse(_sNrAbsoluto, out retval) ? (int?)retval : null;
            }
            set { _sNrAbsoluto = value.ToString(); }
        }

        [XmlIgnore, NotMapped]
        public List<string> ListaStrEpisodios { get; set; }  // Para quando tiver mais de um episódio (nome.do.episodio.S00E00E01E02E03E04)

        [XmlIgnore, NotMapped]
        public List<string> ListaStrEpisodiosAbsolutos { get; set; } // Para quando tiver mais de um episódio absoluto

        [XmlElement("airsafter_season")]
        public string _sNrEstreiaDepoisTemporada;

        [XmlIgnore]
        public int? nNrEstreiaDepoisTemporada
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sNrEstreiaDepoisTemporada) && int.TryParse(_sNrEstreiaDepoisTemporada, out retval) ? (int?)retval : null;
            }
            set
            {
                _sNrEstreiaDepoisTemporada = value.ToString();
            }
        }

        [XmlElement("airsbefore_episode")]
        public string _sNrEstreiaAntesEpisodio;

        [XmlIgnore]
        public int? nNrEstreiaAntesEpisodio
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sNrEstreiaAntesEpisodio) && int.TryParse(_sNrEstreiaAntesEpisodio, out retval) ? (int?)retval : null;
            }
            set
            {
                _sNrEstreiaAntesEpisodio = value.ToString();
            }
        }

        [XmlElement("airsbefore_season")]
        public string _sNrEstreiaAntesTemporada;

        [XmlIgnore]
        public int? nNrEstreiaAntesTemporada
        {
            get
            {
                int retval;

                return !string.IsNullOrWhiteSpace(_sNrEstreiaAntesTemporada) && int.TryParse(_sNrEstreiaAntesTemporada, out retval) ? (int?)retval : null;
            }
            set
            {
                _sNrEstreiaAntesTemporada = value.ToString();
            }
        }

        [XmlIgnore]
        private string _sLkArtwork;

        [XmlElement("filename", IsNullable = true)]
        public string sLkArtwork
        {
            get { return _sLkArtwork; }
            set
            {
                if (value.StartsWith("http"))
                    _sLkArtwork = value;
                else
                {
                    _sLkArtwork = string.IsNullOrWhiteSpace(value) ?
                        "pack://application:,,,/MediaManager;component/Resources/IMG_FanartDefault.png"
                        : Properties.Settings.Default.API_UrlTheTVDB + "/banners/" + value;
                }
            }
        }

        [XmlElement("EpisodeName"), Column(Order = 1)]
        public string sDsEpisodio { get; set; }

        [XmlElement("EpisodeNumber"), Column(Order = 2)]
        public int nNrEpisodio { get; set; }

        private Enums.EstadoEpisodio _nIdEstadoEpisodio;

        public Enums.EstadoEpisodio nIdEstadoEpisodio { get { return _nIdEstadoEpisodio; } set { _nIdEstadoEpisodio = value; OnPropertyChanged(); } }

        [XmlElement("FirstAired")]
        public string _sDtEstreia;

        [XmlIgnore]
        public DateTime? tDtEstreia
        {
            get { return _sDtEstreia != "" ? DateTime.Parse(_sDtEstreia) : default(DateTime?); }
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

        [XmlElement("lastupdated", IsNullable = true)]
        public string sNrUltimaAtualizacao { get; set; }

        [XmlIgnore]
        private string _sDsFilepath;

        [XmlIgnore]
        public string sDsFilepath { get { return _sDsFilepath; } set { _sDsFilepath = value; OnPropertyChanged(); OnPropertyChanged("sDsFilenameRenomeado"); } }

        [XmlIgnore]
        public string sDsFilepathOriginal { get; set; }

        [XmlElement("Overview", IsNullable = true)]
        public string sDsSinopse { get; set; }

        [XmlElement("Rating")]
        public string _sNrAvaliacao;

        [XmlIgnore]
        public double? dNrAvaliacao
        {
            get
            {
                double retval;
                return !string.IsNullOrWhiteSpace(_sNrAvaliacao) && double.TryParse(_sNrAvaliacao, NumberStyles.Number, CultureInfo.InvariantCulture, out retval) ? (double?)retval : null;
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
            set
            {
                _sQtAvaliacao = value.ToString();
            }
        }

        [XmlElement("SeasonNumber"), Column(Order = 3)]
        public int nNrTemporada { get; set; }

        [Column(Order = 5), ForeignKey("nCdVideo")]
        public Serie oSerie { get; set; }

        public string sDsFilenameRenomeado { get { return Path.GetFileName(sDsFilepath); } }

        public Episodio()
        {
            nIdEstadoEpisodio = Enums.EstadoEpisodio.Ignorado;
            ListaStrEpisodios = new List<string>();
            ListaStrEpisodiosAbsolutos = new List<string>();
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

        public bool GetEpisode()
        {
            Helper.RegexEpisodio regexes = new Helper.RegexEpisodio();

            var FilenameTratado = Path.GetFileNameWithoutExtension(sDsFilepath).Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();

            if (oSerie == null)
            {
                oSerie = new Serie();
            }

            try
            {
                if (regexes.regex_S00E00.IsMatch(FilenameTratado))
                {
                    var match = regexes.regex_S00E00.Match(FilenameTratado);
                    oSerie.sDsTitulo = match.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                    List<SerieAlias> listaAlias = DBHelper.GetAllAliases();
                    SerieAlias alias = listaAlias.Where(x => x.sDsAlias.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim() == oSerie.sDsTitulo).FirstOrDefault();

                    if (alias != null)
                    {
                        oSerie = DBHelper.GetSeriePorID(alias.nCdVideo);
                    }

                    if (oSerie.nCdVideo == 0)
                    {
                        var serieTemp = DBHelper.GetSerieOuAnimePorLevenshtein(oSerie.sDsTitulo);

                        if (serieTemp != null)
                        {
                            oSerie = serieTemp;
                        }
                    }

                    if (oSerie.nCdVideo > 0)
                    {
                        List<Episodio> listaEpisodios = DBHelper.GetEpisodes(oSerie);

                        // Para quando se tem multi-episódios
                        char separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value) ? default(char) : match.Groups["separador"].Value.ToCharArray()[0];
                        nNrTemporada = int.Parse(match.Groups["season"].Value);
                        ListaStrEpisodios = new List<string>(match.Groups["episodes"].Value.Split(separador));
                        nNrEpisodio = int.Parse(ListaStrEpisodios[0]);

                        if (alias != null)
                        {
                            Episodio episodio = listaEpisodios.FirstOrDefault(x => x.nNrTemporada == alias.nNrTemporada + nNrTemporada - 1 && x.nNrEpisodio == alias.nNrEpisodio + nNrEpisodio - 1);

                            if (episodio == null)
                            {
                                return false;
                            }

                            episodio.ListaStrEpisodios = ListaStrEpisodios;
                            episodio.ListaStrEpisodiosAbsolutos = ListaStrEpisodiosAbsolutos;
                            episodio.oSerie = oSerie;
                            episodio.nIdTipoConteudo = episodio.oSerie.nIdTipoConteudo;

                            for (int i = 0; i < episodio.ListaStrEpisodios.Count; i++)
                            {
                                // Ajuste no numero do episodio caso este seja de um alias que não comece no primeiro episodio da primeira temporada.
                                episodio.ListaStrEpisodios[i] = alias.nNrEpisodio + nNrEpisodio - 1 + "";
                                episodio.ListaStrEpisodiosAbsolutos.Add(listaEpisodios.Where(x => x.nNrTemporada == episodio.nNrTemporada && x.nNrEpisodio == int.Parse(episodio.ListaStrEpisodios[i])).FirstOrDefault().sNrAbsoluto + "");

                                if (i == 0)
                                {
                                    continue;
                                }

                                Episodio episodioTemp = listaEpisodios.FirstOrDefault(x => x.nNrTemporada == episodio.nNrTemporada && x.nNrEpisodio == int.Parse(episodio.ListaStrEpisodios[i])); // TODO Testar pra ver se funfa

                                if (episodioTemp != null)
                                {
                                    episodio.sDsEpisodio += " & " + episodioTemp.sDsEpisodio;
                                }
                            }
                            Clone(episodio);
                        }
                        else
                        {
                            Episodio episodio = listaEpisodios.Where(x => x.nNrTemporada == nNrTemporada && x.nNrEpisodio == nNrEpisodio).FirstOrDefault();

                            if (episodio == null)
                            {
                                return false;
                            }

                            episodio.ListaStrEpisodios = ListaStrEpisodios;
                            episodio.ListaStrEpisodiosAbsolutos = ListaStrEpisodiosAbsolutos;
                            episodio.oSerie = oSerie;
                            episodio.nIdTipoConteudo = episodio.oSerie.nIdTipoConteudo;

                            for (int i = 0; i < episodio.ListaStrEpisodios.Count; i++)
                            {
                                // Ajuste no numero do episodio caso este seja de um alias que não comece no primeiro episodio da primeira temporada.
                                episodio.ListaStrEpisodiosAbsolutos.Add(listaEpisodios.Where(x => x.nNrTemporada == episodio.nNrTemporada && x.nNrEpisodio == int.Parse(episodio.ListaStrEpisodios[i])).FirstOrDefault().sNrAbsoluto + "");

                                if (i == 0)
                                {
                                    continue;
                                }

                                Episodio episodioTemp = listaEpisodios.Where(x => x.nNrTemporada == nNrTemporada && x.nNrEpisodio == int.Parse(episodio.ListaStrEpisodios[i])).FirstOrDefault(); // TODO Testar pra ver se funfa

                                if (episodioTemp != null)
                                {
                                    episodio.sDsEpisodio += " & " + episodioTemp.sDsEpisodio;
                                }
                            }
                            Clone(episodio);
                        }
                        return true;
                    }
                }
                else if (regexes.regex_0x00.IsMatch(FilenameTratado)) // TODO Fazer funcionar com alias
                {
                    var match = regexes.regex_0x00.Match(FilenameTratado);
                    oSerie.sDsTitulo = match.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                    List<SerieAlias> listaAlias = DBHelper.GetAllAliases();
                    SerieAlias alias = listaAlias.Where(x => x.sDsAlias.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim() == oSerie.sDsTitulo).FirstOrDefault();

                    if (alias != null)
                    {
                        oSerie = DBHelper.GetSeriePorID(alias.nCdVideo);
                    }

                    if (oSerie.nCdVideo == 0)
                    {
                        var serieTemp = DBHelper.GetSerieOuAnimePorLevenshtein(oSerie.sDsTitulo);

                        if (serieTemp != null)
                        {
                            oSerie = serieTemp;
                        }
                    }

                    if (oSerie.nCdVideo > 0)
                    {
                        List<Episodio> listaEpisodios = DBHelper.GetEpisodes(oSerie);

                        // Para quando se tem multi-episódios
                        char separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value) ? default(char) : match.Groups["separador"].Value.ToCharArray()[0];
                        nNrTemporada = int.Parse(match.Groups["season"].Value);
                        ListaStrEpisodios = new List<string>(match.Groups["episodes"].Value.Split(separador));
                        nNrEpisodio = int.Parse(ListaStrEpisodios[0]);
                        if (alias != null)
                        {
                            Episodio episodio = listaEpisodios.Where(x => x.nNrTemporada == nNrTemporada && x.nNrEpisodio == nNrEpisodio).FirstOrDefault();

                            if (episodio == null)
                            {
                                return false;
                            }

                            episodio.ListaStrEpisodios = ListaStrEpisodios;
                            episodio.ListaStrEpisodiosAbsolutos = ListaStrEpisodiosAbsolutos;
                            episodio.oSerie = oSerie;
                            episodio.nIdTipoConteudo = episodio.oSerie.nIdTipoConteudo;

                            for (int i = 0; i < episodio.ListaStrEpisodios.Count; i++)
                            {
                                // Ajuste no numero do episodio caso este seja de um alias que não comece no primeiro episodio da primeira temporada.
                                episodio.ListaStrEpisodios[i] = alias.nNrEpisodio + nNrEpisodio - 1 + "";
                                episodio.ListaStrEpisodiosAbsolutos.Add(listaEpisodios.Where(x => x.nNrTemporada == episodio.nNrTemporada && x.nNrEpisodio == int.Parse(episodio.ListaStrEpisodios[i])).FirstOrDefault().sNrAbsoluto + "");

                                if (i == 0)
                                {
                                    continue;
                                }

                                Episodio episodioTemp = listaEpisodios.Where(x => x.nNrTemporada == episodio.nNrTemporada && x.nNrEpisodio == int.Parse(episodio.ListaStrEpisodios[i])).FirstOrDefault(); // TODO Testar pra ver se funfa

                                if (episodioTemp != null)
                                {
                                    episodio.sDsEpisodio += " & " + episodioTemp.sDsEpisodio;
                                }
                            }
                        }
                        else
                        {
                            Episodio episodio = listaEpisodios.Where(x => x.nNrTemporada == nNrTemporada && x.nNrEpisodio == nNrEpisodio).FirstOrDefault();

                            if (episodio == null)
                            {
                                return false;
                            }

                            episodio.ListaStrEpisodios = ListaStrEpisodios;
                            episodio.ListaStrEpisodiosAbsolutos = ListaStrEpisodiosAbsolutos;
                            episodio.oSerie = oSerie;
                            episodio.nIdTipoConteudo = episodio.oSerie.nIdTipoConteudo;

                            for (int i = 0; i < episodio.ListaStrEpisodios.Count; i++)
                            {
                                episodio.ListaStrEpisodiosAbsolutos.Add(listaEpisodios.Where(x => x.nNrTemporada == episodio.nNrTemporada && x.nNrEpisodio == int.Parse(episodio.ListaStrEpisodios[i])).FirstOrDefault().sNrAbsoluto + "");

                                if (i == 0)
                                {
                                    continue;
                                }

                                Episodio episodioTemp = listaEpisodios.Where(x => x.nNrTemporada == nNrTemporada && x.nNrEpisodio == int.Parse(episodio.ListaStrEpisodios[i])).FirstOrDefault(); // TODO Testar pra ver se funfa

                                if (episodioTemp != null)
                                {
                                    episodio.sDsEpisodio += " & " + episodioTemp.sDsEpisodio;
                                }
                            }
                            Clone(episodio);
                        }
                    }
                    return true;
                }
                else if (regexes.regex_Fansub0000.IsMatch(FilenameTratado))
                {
                    var match = regexes.regex_Fansub0000.Match(FilenameTratado);
                    oSerie.sDsTitulo = match.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                    List<SerieAlias> listaAlias = DBHelper.GetAllAliases();
                    SerieAlias alias = listaAlias.Where(x => x.sDsAlias.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim() == oSerie.sDsTitulo).FirstOrDefault();
                    if (alias != null)
                    {
                        oSerie = DBHelper.GetSeriePorID(alias.nCdVideo);
                    }

                    if (oSerie.nCdVideo == 0)
                    {
                        var serieTemp = DBHelper.GetSerieOuAnimePorLevenshtein(oSerie.sDsTitulo);

                        if (serieTemp != null)
                        {
                            oSerie = serieTemp;
                        }
                    }

                    if (oSerie.nCdVideo > 0)
                    {
                        List<Episodio> listaEpisodios = DBHelper.GetEpisodes(oSerie);

                        // Para quando se tem multi-episódios
                        char separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value) ? default(char) : match.Groups["separador"].Value.ToCharArray()[0];
                        ListaStrEpisodiosAbsolutos = new List<string>(match.Groups["episodes"].Value.Split(separador));
                        sNrAbsoluto = int.Parse(ListaStrEpisodios[0]);

                        if (alias != null) // Entra se estiver utilizando alias para identificar a serie
                        {
                            Episodio primeiroEpisodioAlias = listaEpisodios.FirstOrDefault(x => x.nNrTemporada == alias.nNrTemporada && x.nNrEpisodio == alias.nNrEpisodio);
                            Episodio episodio = listaEpisodios.FirstOrDefault(x => x.sNrAbsoluto == primeiroEpisodioAlias.sNrAbsoluto + sNrAbsoluto - 1);

                            if (episodio.nCdEpisodio == 0)
                            {
                                return false;
                            }

                            episodio.ListaStrEpisodios = ListaStrEpisodios;
                            episodio.ListaStrEpisodiosAbsolutos = ListaStrEpisodiosAbsolutos;
                            episodio.oSerie = oSerie;
                            episodio.nIdTipoConteudo = episodio.oSerie.nIdTipoConteudo;

                            for (int i = 0; i < episodio.ListaStrEpisodiosAbsolutos.Count; i++)
                            {
                                // Ajuste no numero do episodio caso este seja de um alias que não comece no primeiro episodio da primeira temporada.
                                episodio.ListaStrEpisodiosAbsolutos[i] = primeiroEpisodioAlias.sNrAbsoluto + int.Parse(episodio.ListaStrEpisodiosAbsolutos[i]) - 1 + "";
                                Episodio episodioTemp = listaEpisodios.FirstOrDefault(x => x.sNrAbsoluto == int.Parse(episodio.ListaStrEpisodiosAbsolutos[i]));
                                episodio.ListaStrEpisodios.Add(episodioTemp.nNrEpisodio + "");

                                if (i == 0)
                                {
                                    continue;
                                }

                                if (episodioTemp != null)
                                {
                                    episodio.sDsEpisodio += " & " + episodioTemp.sDsEpisodio;
                                }
                            }
                            Clone(episodio);
                        }
                        else
                        {
                            Episodio episodio = listaEpisodios.FirstOrDefault(x => x.sNrAbsoluto == sNrAbsoluto);

                            if (episodio == null)
                            {
                                return false;
                            }

                            episodio.ListaStrEpisodios = ListaStrEpisodios;
                            episodio.ListaStrEpisodiosAbsolutos = ListaStrEpisodiosAbsolutos;
                            episodio.sNrAbsoluto = sNrAbsoluto;
                            episodio.oSerie = oSerie;
                            episodio.nIdTipoConteudo = episodio.oSerie.nIdTipoConteudo;

                            for (int i = 0; i < episodio.ListaStrEpisodiosAbsolutos.Count; i++)
                            {
                                // Ajuste no numero do episodio caso este seja de um alias que não comece no primeiro episodio da primeira temporada.
                                Episodio episodioTemp = listaEpisodios.FirstOrDefault(x => x.sNrAbsoluto == int.Parse(episodio.ListaStrEpisodiosAbsolutos[i]));
                                episodio.ListaStrEpisodios.Add(episodioTemp.nNrEpisodio + "");

                                if (i == 0)
                                {
                                    continue;
                                }

                                if (episodioTemp != null)
                                {
                                    episodio.sDsEpisodio += " & " + episodioTemp.sDsEpisodio;
                                }
                            }
                            Clone(episodio);
                        }
                        return true;
                    }
                }
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao reconhecer o episódio " + sDsFilepath, true); }
            return false;
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