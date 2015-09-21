using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;

namespace MediaManager.ViewModel
{
    public class PreferenciasViewModel : INotifyPropertyChanged
    {
        private string _PastaSeries;

        public string PastaSeries { get { return _PastaSeries; } set { _PastaSeries = value; OnPropertyChanged("PastaSeries"); } }

        public ICommand CommandEscolherPastaSeries { get; set; }

        private string _PastaFilmes;

        public string PastaFilmes { get { return _PastaFilmes; } set { _PastaFilmes = value; OnPropertyChanged("PastaFilmes"); } }

        public ICommand CommandEscolherPastaFilmes { get; set; }

        private string _PastaAnimes;

        public string PastaAnimes { get { return _PastaAnimes; } set { _PastaAnimes = value; OnPropertyChanged("PastaAnimes"); } }

        public ICommand CommandEscolherPastaAnimes { get; set; }

        private string _PastaDownloads;

        public string PastaDownloads { get { return _PastaDownloads; } set { _PastaDownloads = value; OnPropertyChanged("PastaDownloads"); } }

        public ICommand CommandEscolherPastaDownloads { get; set; }

        private int _IntervaloDeProcuraConteudoNovo;

        public int IntervaloDeProcuraConteudoNovo { get { return _IntervaloDeProcuraConteudoNovo; } set { _IntervaloDeProcuraConteudoNovo = value; OnPropertyChanged("IntervaloDeProcuraConteudoNovo"); } }

        private Dictionary<string, string> _ListaIdiomaPesquisa;

        public Dictionary<string, string> ListaIdiomaPesquisa { get { return _ListaIdiomaPesquisa; } set { _ListaIdiomaPesquisa = value; OnPropertyChanged("ListaIdiomaPesquisa"); } }

        private string _IdiomaSelecionado;

        public string IdiomaSelecionado { get { return _IdiomaSelecionado; } set { _IdiomaSelecionado = value; OnPropertyChanged("IdiomaSelecionado"); } }

        private Array _EnumMetodosDeProcessamento;

        public Array EnumMetodosDeProcessamento { get { return _EnumMetodosDeProcessamento; } set { _EnumMetodosDeProcessamento = value; OnPropertyChanged("EnumMetodosDeProcessamento"); } }

        private Enums.MetodoDeProcessamento _MetodoDeProcessamentoSelecionado;

        public Enums.MetodoDeProcessamento MetodoDeProcessamentoSelecionado { get { return _MetodoDeProcessamentoSelecionado; } set { _MetodoDeProcessamentoSelecionado = value; OnPropertyChanged("MetodoDeProcessamentoSelecionado"); } }

        public ICommand CommandLimparBancoDeDados { get; set; }

        private string _FormatoParaSeries;

        public string FormatoParaSeries { get { return _FormatoParaSeries; } set { _FormatoParaSeries = value; OnPropertyChanged("FormatoParaSeries"); } }

        private string _FormatoParaFilmes;

        public string FormatoParaFilmes { get { return _FormatoParaFilmes; } set { _FormatoParaFilmes = value; OnPropertyChanged("FormatoParaFilmes"); } }

        private string _FormatoParaAnimes;

        public string FormatoParaAnimes { get { return _FormatoParaAnimes; } set { _FormatoParaAnimes = value; OnPropertyChanged("FormatoParaAnimes"); } }

        private string _VisualizacaoFormatoParaSeries;

        public string VisualizacaoFormatoParaSeries { get { return _VisualizacaoFormatoParaSeries; } set { _VisualizacaoFormatoParaSeries = value; OnPropertyChanged("VisualizacaoFormatoParaSeries"); } }

        private string _VisualizacaoFormatoParaFilmes;

        public string VisualizacaoFormatoParaFilmes { get { return _VisualizacaoFormatoParaFilmes; } set { _VisualizacaoFormatoParaFilmes = value; OnPropertyChanged("VisualizacaoFormatoParaFilmes"); } }

        private string _VisualizacaoFormatoParaAnimes;

        public string VisualizacaoFormatoParaAnimes { get { return _VisualizacaoFormatoParaAnimes; } set { _VisualizacaoFormatoParaAnimes = value; OnPropertyChanged("VisualizacaoFormatoParaAnimes"); } }

        public ICommand CommandSalvar { get; set; }

        public Action CloseAction { get; set; }

        public PreferenciasViewModel()
        {
            FormatoParaAnimes = Properties.Settings.Default.pref_FormatoAnimes;
            FormatoParaFilmes = Properties.Settings.Default.pref_FormatoFilmes;
            FormatoParaSeries = Properties.Settings.Default.pref_FormatoSeries;
            ListaIdiomaPesquisa = SetarIdiomas();
            IdiomaSelecionado = Properties.Settings.Default.pref_IdiomaPesquisa;
            EnumMetodosDeProcessamento = Enum.GetValues(typeof(Enums.MetodoDeProcessamento));
            MetodoDeProcessamentoSelecionado = (Enums.MetodoDeProcessamento)Properties.Settings.Default.pref_MetodoDeProcessamento;
            IntervaloDeProcuraConteudoNovo = Properties.Settings.Default.pref_IntervaloDeProcuraConteudoNovo;
            PastaAnimes = Properties.Settings.Default.pref_PastaAnimes;
            PastaDownloads = Properties.Settings.Default.pref_PastaDownloads;
            PastaFilmes = Properties.Settings.Default.pref_PastaFilmes;
            PastaSeries = Properties.Settings.Default.pref_PastaSeries;
            VisualizacaoFormatoParaAnimes = "O Nome do Anime - 000 - Título do episódio"; // TODO Visualizacao Animes
            VisualizacaoFormatoParaFilmes = "Nome do filme, O (2015)"; // TODO Visualizacao Filmes
            VisualizacaoFormatoParaSeries = "O Nome da Série - S01E01 - Título da série"; // TODO Visualizacao Series

            CommandEscolherPastaAnimes = new CommandsPreferencias.CommandEscolherPastaAnimes();
            CommandEscolherPastaDownloads = new CommandsPreferencias.CommandEscolherPastaDownloads();
            CommandEscolherPastaFilmes = new CommandsPreferencias.CommandEscolherPastaFilmes();
            CommandEscolherPastaSeries = new CommandsPreferencias.CommandEscolherPastaSeries();
            CommandLimparBancoDeDados = new CommandsPreferencias.CommandLimparBancoDeDados();
            CommandSalvar = new CommandsPreferencias.CommandSalvar();
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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
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