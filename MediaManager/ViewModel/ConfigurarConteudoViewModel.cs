// Developed by: Gabriel Duarte
// 
// Created at: 06/09/2015 06:14
// Last update: 19/04/2016 02:57

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class ConfigurarConteudoViewModel : ViewModelBase
    {
        private bool _bIsParado;

        private IList<SerieAlias> _lstTempSerieAliases;

        private int _nNrEpisodio;

        private int _nNrTemporada;

        private SerieAlias _oAliasSelecionado;

        private Video _oVideo;

        private string _sDsAlias;

        private string _sFormatoRenomeioPersonalizado;

        public ConfigurarConteudoViewModel(Video oVideo)
        {
            this.oVideo = oVideo;
            _nNrTemporada = 1;
            _nNrEpisodio = 1;
            this.oVideo.lstSerieAlias = Helper.PopularCampoSerieAlias(this.oVideo);

            if (oVideo is Serie)
            {
                bIsParado = (oVideo as Serie).bIsParado;
            }

            sFormatoRenomeioPersonalizado = oVideo.sFormatoRenomeioPersonalizado;

            PopularListaTempSerieAlias();

            DoubleClickCommand = new ConfigurarConteudoCommands.DoubleClickNoGridAliasCommand();
            AddAlias = new ConfigurarConteudoCommands.AddAlias();
            RemoveAlias = new ConfigurarConteudoCommands.RemoveAlias();
            CommandSalvar = new ConfigurarConteudoCommands.CommandSalvar();
            CommandRemoverSerie = new ConfigurarConteudoCommands.CommandRemoverSerie();
        }

        public string sDsAlias
        {
            get { return _sDsAlias; }
            set
            {
                _sDsAlias = value;
                OnPropertyChanged();
            }
        }

        public string sNrTemporada
        {
            get
            {
                return nNrTemporada >= 0
                           ? "S" + _nNrTemporada.ToString("00")
                           : "S";
            }
            set
            {
                _nNrTemporada = ValidarPossivelTexto(value, _nNrTemporada);
                OnPropertyChanged("nNrTemporada");
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

        public int nNrTemporada
        {
            get { return _nNrTemporada; }
            set
            {
                _nNrTemporada = value;
                OnPropertyChanged();
                OnPropertyChanged("sNrTemporada");
            }
        }

        public string sNrEpisodio
        {
            get
            {
                return nNrEpisodio >= 0
                           ? "E" + _nNrEpisodio.ToString("00")
                           : "E";
            }
            set
            {
                _nNrEpisodio = ValidarPossivelTexto(value, _nNrEpisodio);
                OnPropertyChanged("nNrEpisodio");
            }
        }

        public int nNrEpisodio
        {
            get { return _nNrEpisodio; }
            set
            {
                _nNrEpisodio = value;
                OnPropertyChanged();
                OnPropertyChanged("sNrEpisodio");
            }
        }

        public bool bFlAcaoRemover { get; set; } // Para poder fechar as outras janelas ao remover.

        public bool bIsParado
        {
            get { return _bIsParado; }
            set
            {
                _bIsParado = value;
                OnPropertyChanged();
            }
        } // TODO Adicionar comando também na label, não só no check

        public SerieAlias oAliasSelecionado
        {
            get { return _oAliasSelecionado; }
            set
            {
                _oAliasSelecionado = value;
                OnPropertyChanged();
            }
        }

        public Video oVideo
        {
            get { return _oVideo; }
            set
            {
                _oVideo = value;
                OnPropertyChanged();
            }
        }

        public IList<SerieAlias> lstTempSerieAliases
        {
            get { return _lstTempSerieAliases; }
            set
            {
                _lstTempSerieAliases = value;
                OnPropertyChanged();
            }
        }

        public Action ActionFechar { get; set; }

        public ICommand DoubleClickCommand { get; set; }

        public ICommand AddAlias { get; set; }

        public ICommand RemoveAlias { get; set; }

        public ICommand CommandSalvar { get; set; }

        public ICommand CommandRemoverSerie { get; set; }

        private void PopularListaTempSerieAlias()
        {
            lstTempSerieAliases = new ObservableCollection<SerieAlias>();
            foreach (SerieAlias item in oVideo.lstSerieAlias)
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
