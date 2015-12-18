using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class ConfigurarConteudoViewModel : ViewModelBase
    {
        private string _sDsAlias;

        public string sDsAlias { get { return _sDsAlias; } set { _sDsAlias = value; OnPropertyChanged(); } }

        public string sNrTemporada { get { return nNrTemporada >= 0 ? "S" + _nNrTemporada.ToString("00") : "S"; } set { _nNrTemporada = ValidarPossivelTexto(value, _nNrTemporada); OnPropertyChanged("nNrTemporada"); } }

        private int _nNrTemporada;

        public int nNrTemporada { get { return _nNrTemporada; } set { _nNrTemporada = value; OnPropertyChanged(); OnPropertyChanged("sNrTemporada"); } }

        public string sNrEpisodio { get { return nNrEpisodio >= 0 ? "E" + _nNrEpisodio.ToString("00") : "E"; } set { _nNrEpisodio = ValidarPossivelTexto(value, _nNrEpisodio); OnPropertyChanged("nNrEpisodio"); } }

        private int _nNrEpisodio;

        public int nNrEpisodio { get { return _nNrEpisodio; } set { _nNrEpisodio = value; OnPropertyChanged(); OnPropertyChanged("sNrEpisodio"); } }

        public bool bFlAcaoRemover { get; set; } // Para poder fechar as outras janelas ao remover.

        private SerieAlias _oAliasSelecionado;

        public SerieAlias oAliasSelecionado { get { return _oAliasSelecionado; } set { _oAliasSelecionado = value; OnPropertyChanged(); } }

        private Video _oVideo;

        public Video oVideo { get { return _oVideo; } set { _oVideo = value; OnPropertyChanged(); } }

        private IList<SerieAlias> _lstTempSerieAliases;

        public IList<SerieAlias> lstTempSerieAliases { get { return _lstTempSerieAliases; } set { _lstTempSerieAliases = value; OnPropertyChanged(); } }

        public Action ActionFechar { get; set; }

        public ICommand DoubleClickCommand { get; set; }

        public ICommand AddAlias { get; set; }

        public ICommand RemoveAlias { get; set; }

        public ICommand CommandSalvar { get; set; }

        public ICommand CommandRemoverSerie { get; set; }

        public ConfigurarConteudoViewModel(Video oVideo)
        {
            this.oVideo = oVideo;
            _nNrTemporada = 1;
            _nNrEpisodio = 1;
            this.oVideo.lstSerieAlias = Helper.PopularCampoSerieAlias(this.oVideo);

            PopularListaTempSerieAlias();

            DoubleClickCommand = new ConfigurarConteudoCommands.DoubleClickNoGridAliasCommand();
            AddAlias = new ConfigurarConteudoCommands.AddAlias();
            RemoveAlias = new ConfigurarConteudoCommands.RemoveAlias();
            CommandSalvar = new ConfigurarConteudoCommands.CommandSalvar();
            CommandRemoverSerie = new ConfigurarConteudoCommands.CommandRemoverSerie();
        }

        private void PopularListaTempSerieAlias()
        {
            lstTempSerieAliases = new ObservableCollection<SerieAlias>();
            foreach (var item in oVideo.lstSerieAlias)
            {
                lstTempSerieAliases.Add(item);
            }
        }

        private int ValidarPossivelTexto(string valor, int valorAntigo)
        {
            if (Regex.IsMatch(valor, @"^S\d{1,3}$") || Regex.IsMatch(valor, @"^E\d{1,3}$"))
            {
                return Convert.ToInt32(Regex.Match(valor, @"\d+").Value);
            }
            return valorAntigo;
        }
    }
}