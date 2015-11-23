using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class EpisodiosViewModel : INotifyPropertyChanged
    {
        private ICollectionView _ListaEpisodiosView;

        public ICollectionView ListaEpisodiosView { get { return _ListaEpisodiosView; } set { _ListaEpisodiosView = value; OnPropertyChanged(); } }

        private ObservableCollection<Episodio> _ListaEpisodios;

        public ObservableCollection<Episodio> ListaEpisodios { get { return _ListaEpisodios; } set { _ListaEpisodios = value; OnPropertyChanged(); } }

        private bool? _bFlSelecionarTodos;

        public bool? bFlSelecionarTodos { get { return _bFlSelecionarTodos; } set { _bFlSelecionarTodos = value; OnPropertyChanged(); } }

        private Array _ArrayEstadoEpisodio;

        public Array ArrayEstadoEpisodio { get { return _ArrayEstadoEpisodio; } set { _ArrayEstadoEpisodio = value; OnPropertyChanged(); } }

        private Enums.EstadoEpisodio _nIdEstadoEpisodioSelecionado;

        public Enums.EstadoEpisodio nIdEstadoEpisodioSelecionado { get { return _nIdEstadoEpisodioSelecionado; } set { _nIdEstadoEpisodioSelecionado = value; OnPropertyChanged(); } }

        public Action ActionFechar { get; set; }

        public ICommand CommandSelecionarTodos { get; set; }

        public ICommand CommandIsSelected { get; set; }

        public ICommand CommandSalvar { get; set; }

        public ICommand CommandFechar { get; set; }

        public EpisodiosViewModel(List<Episodio> episodios)
        {
            ListaEpisodios = new ObservableCollection<Episodio>(episodios);
            ListaEpisodiosView = new ListCollectionView(ListaEpisodios);
            ListaEpisodiosView.GroupDescriptions.Add(new PropertyGroupDescription("nNrTemporada"));
            bFlSelecionarTodos = false;
            var listaEstadosParaExibir = new List<int> { 0, 1, 2, 4, 5 };
            ArrayEstadoEpisodio = Enum.GetValues(typeof(Enums.EstadoEpisodio)).Cast<Enums.EstadoEpisodio>().Where(x => listaEstadosParaExibir.Contains((int)x)).ToArray();
            nIdEstadoEpisodioSelecionado = Enums.EstadoEpisodio.Selecione;
            CommandSelecionarTodos = new EpisodiosCommand.CommandSelecionarTodos();
            CommandIsSelected = new EpisodiosCommand.CommandIsSelected();
            CommandSalvar = new EpisodiosCommand.CommandSalvar();
            CommandFechar = new EpisodiosCommand.CommandFechar();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
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