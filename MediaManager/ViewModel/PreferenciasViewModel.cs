using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class PreferenciasViewModel : ViewModelBase
    {
        private ListaFeedsViewModel _oFeedsViewModel;

        public ListaFeedsViewModel oFeedsViewModel { get { return _oFeedsViewModel; } set { _oFeedsViewModel = value; OnPropertyChanged(); } }

        private ListaFeedsPesquisaViewModel _oListaFeedsPesquisaViewModel;

        public ListaFeedsPesquisaViewModel oListaFeedsPesquisaViewModel { get { return _oListaFeedsPesquisaViewModel; } set { _oListaFeedsPesquisaViewModel = value; OnPropertyChanged(); } }

        private string _sPastaSeries;

        public string sPastaSeries { get { return _sPastaSeries; } set { PerguntarAlterarParaTodosVideosExistentes(Enums.TipoConteudo.Série, value); _sPastaSeries = value; OnPropertyChanged(); } }

        public ICommand CommandEscolherPastaSeries { get; set; }

        private string _sPastaFilmes;

        public string sPastaFilmes { get { return _sPastaFilmes; } set { PerguntarAlterarParaTodosVideosExistentes(Enums.TipoConteudo.Filme, value); _sPastaFilmes = value; OnPropertyChanged(); } }

        public ICommand CommandEscolherPastaFilmes { get; set; }

        private string _sPastaAnimes;

        public string sPastaAnimes { get { return _sPastaAnimes; } set { PerguntarAlterarParaTodosVideosExistentes(Enums.TipoConteudo.Anime, value); _sPastaAnimes = value; OnPropertyChanged(); } }

        public ICommand CommandEscolherPastaAnimes { get; set; }

        private string _sPastaDownloads;

        public string sPastaDownloads { get { return _sPastaDownloads; } set { _sPastaDownloads = value; OnPropertyChanged(); } }

        public ICommand CommandEscolherPastaDownloads { get; set; }

        private string _sPastaBlackhole;

        public string sPastaBlackhole { get { return _sPastaBlackhole; } set { _sPastaBlackhole = value; OnPropertyChanged(); } }

        public ICommand CommandEscolherPastaBlackhole { get; set; }

        private int _nIntervaloDeProcuraConteudoNovo;

        public int nIntervaloDeProcuraConteudoNovo { get { return _nIntervaloDeProcuraConteudoNovo; } set { _nIntervaloDeProcuraConteudoNovo = value; OnPropertyChanged(); } }

        private Dictionary<string, string> _DictIdiomaPesquisa;

        public Dictionary<string, string> DictIdiomaPesquisa { get { return _DictIdiomaPesquisa; } set { _DictIdiomaPesquisa = value; OnPropertyChanged(); } }

        private string _sIdiomaSelecionado;

        public string sIdiomaSelecionado { get { return _sIdiomaSelecionado; } set { _sIdiomaSelecionado = value; OnPropertyChanged(); } }

        private Array _ArrayMetodosDeProcessamento;

        public Array ArrayMetodosDeProcessamento { get { return _ArrayMetodosDeProcessamento; } set { _ArrayMetodosDeProcessamento = value; OnPropertyChanged(); } }

        private Enums.MetodoDeProcessamento _nIdMetodoDeProcessamentoSelecionado;

        public Enums.MetodoDeProcessamento nIdMetodoDeProcessamentoSelecionado { get { return _nIdMetodoDeProcessamentoSelecionado; } set { _nIdMetodoDeProcessamentoSelecionado = value; OnPropertyChanged(); } }

        public ICommand CommandLimparBancoDeDados { get; set; }

        private string _sFormatoParaSeries;

        public string sFormatoParaSeries { get { return _sFormatoParaSeries; } set { _sFormatoParaSeries = value; OnPropertyChanged(); AlterarVisualizacaoFormato(value, Enums.TipoConteudo.Série); } }

        private string _sFormatoParaFilmes;

        public string sFormatoParaFilmes { get { return _sFormatoParaFilmes; } set { _sFormatoParaFilmes = value; OnPropertyChanged(); AlterarVisualizacaoFormato(value, Enums.TipoConteudo.Filme); } }

        private string _sFormatoParaAnimes;

        public string sFormatoParaAnimes { get { return _sFormatoParaAnimes; } set { _sFormatoParaAnimes = value; OnPropertyChanged(); AlterarVisualizacaoFormato(value, Enums.TipoConteudo.Anime); } }

        private string _sVisualizacaoFormatoParaSeries;

        public string sVisualizacaoFormatoParaSeries { get { return _sVisualizacaoFormatoParaSeries; } set { _sVisualizacaoFormatoParaSeries = value; OnPropertyChanged(); } }

        private string _sVisualizacaoFormatoParaFilmes;

        public string sVisualizacaoFormatoParaFilmes { get { return _sVisualizacaoFormatoParaFilmes; } set { _sVisualizacaoFormatoParaFilmes = value; OnPropertyChanged(); } }

        private string _sVisualizacaoFormatoParaAnimes;

        public string sVisualizacaoFormatoParaAnimes { get { return _sVisualizacaoFormatoParaAnimes; } set { _sVisualizacaoFormatoParaAnimes = value; OnPropertyChanged(); } }

        public Window Owner { get; set; }

        public ICommand CommandSalvar { get; set; }

        public Action ActionFechar { get; set; }

        public PreferenciasViewModel(Window owner = null)
        {
            if (owner != null)
            {
                Owner = owner;
            }

            oFeedsViewModel = new ListaFeedsViewModel();
            oFeedsViewModel.Owner = Owner;
            oListaFeedsPesquisaViewModel = new ListaFeedsPesquisaViewModel();
            oListaFeedsPesquisaViewModel.Owner = Owner;
            sFormatoParaAnimes = Properties.Settings.Default.pref_FormatoAnimes;
            sFormatoParaFilmes = Properties.Settings.Default.pref_FormatoFilmes;
            sFormatoParaSeries = Properties.Settings.Default.pref_FormatoSeries;
            DictIdiomaPesquisa = SetarIdiomas();
            sIdiomaSelecionado = Properties.Settings.Default.pref_IdiomaPesquisa;
            ArrayMetodosDeProcessamento = Enum.GetValues(typeof(Enums.MetodoDeProcessamento));
            nIdMetodoDeProcessamentoSelecionado = (Enums.MetodoDeProcessamento)Properties.Settings.Default.pref_MetodoDeProcessamento;
            nIntervaloDeProcuraConteudoNovo = Properties.Settings.Default.pref_IntervaloDeProcuraConteudoNovo;
            sPastaAnimes = Properties.Settings.Default.pref_PastaAnimes;
            sPastaFilmes = Properties.Settings.Default.pref_PastaFilmes;
            sPastaSeries = Properties.Settings.Default.pref_PastaSeries;
            sPastaDownloads = Properties.Settings.Default.pref_PastaDownloads;
            sPastaBlackhole = Properties.Settings.Default.pref_PastaBlackhole;
            CommandEscolherPastaAnimes = new PreferenciasCommands.CommandEscolherPastaAnimes();
            CommandEscolherPastaFilmes = new PreferenciasCommands.CommandEscolherPastaFilmes();
            CommandEscolherPastaSeries = new PreferenciasCommands.CommandEscolherPastaSeries();
            CommandEscolherPastaDownloads = new PreferenciasCommands.CommandEscolherPastaDownloads();
            CommandEscolherPastaBlackhole = new PreferenciasCommands.CommandEscolherPastaBlackhole();
            CommandLimparBancoDeDados = new PreferenciasCommands.CommandLimparBancoDeDados();
            CommandSalvar = new PreferenciasCommands.CommandSalvar();
        }

        private void PerguntarAlterarParaTodosVideosExistentes(Enums.TipoConteudo nIdTipoConteudo, string valor)
        {
        }

        private void AlterarVisualizacaoFormato(string sFormato, Enums.TipoConteudo nIdTipoConteudo)
        {
            Episodio episodioVisualizacao = new Episodio()
            {
                oSerie = new Serie() { sDsTitulo = "Exemplo de título", sFormatoRenomeioPersonalizado = sFormato },
                sDsEpisodio = "Título do episódio",
                nNrTemporada = 3,
                lstIntEpisodios = new List<int>() { 5 },
                lstIntEpisodiosAbsolutos = new List<int>() { 25 }
            };

            switch (nIdTipoConteudo)
            {
                case Enums.TipoConteudo.Filme:
                    episodioVisualizacao.nIdTipoConteudo = Enums.TipoConteudo.Filme;
                    sVisualizacaoFormatoParaFilmes = "Nome do filme, O (2015) (ainda não tem pré-visualização)"; // TODO Visualizacao Filmes
                    break;

                case Enums.TipoConteudo.Série:
                    episodioVisualizacao.nIdTipoConteudo = Enums.TipoConteudo.Série;
                    sVisualizacaoFormatoParaSeries = Helper.RenomearConformePreferencias(episodioVisualizacao);
                    break;

                case Enums.TipoConteudo.Anime:
                    episodioVisualizacao.nIdTipoConteudo = Enums.TipoConteudo.Anime;
                    sVisualizacaoFormatoParaAnimes = Helper.RenomearConformePreferencias(episodioVisualizacao);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Idiomas do TheTVDB. Segundo eles isso é tão dificil de ser alterado que pode ser hardcoded. Para verificar o xml com os idiomas a url é http://thetvdb.com/api/{APIKey}/languages.xml
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> SetarIdiomas()
        {
            Dictionary<string, string> idiomas = new Dictionary<string, string>();

            idiomas.Add("da", "Dansk");
            idiomas.Add("fi", "Suomeksi");
            idiomas.Add("nl", "Nederlands");
            idiomas.Add("de", "Deutsch");
            idiomas.Add("it", "Italiano");
            idiomas.Add("es", "Español");
            idiomas.Add("fr", "Français");
            idiomas.Add("pl", "Polski");
            idiomas.Add("hu", "Magyar");
            idiomas.Add("el", "Ελληνικά");
            idiomas.Add("tr", "Türkçe");
            idiomas.Add("ru", "русский язык");
            idiomas.Add("he", "עברית");
            idiomas.Add("ja", "日本語");
            idiomas.Add("pt", "Português");
            idiomas.Add("zh", "中文");
            idiomas.Add("cs", "čeština");
            idiomas.Add("sl", "Slovenski");
            idiomas.Add("hr", "Hrvatski");
            idiomas.Add("ko", "한국어");
            idiomas.Add("en", "English");
            idiomas.Add("sv", "Svenska");
            idiomas.Add("no", "Norsk");

            return idiomas;
        }
    }
}