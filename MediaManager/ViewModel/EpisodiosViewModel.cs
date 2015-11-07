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
        private ICollectionView _episodiosView;

        public ICollectionView EpisodiosView { get { return _episodiosView; } set { _episodiosView = value; OnPropertyChanged(); } }

        private ObservableCollection<Episode> _episodios;

        public ObservableCollection<Episode> Episodios { get { return _episodios; } set { _episodios = value; OnPropertyChanged(); } }

        private bool? _selecionarTodos;

        public bool? SelecionarTodos { get { return _selecionarTodos; } set { _selecionarTodos = value; OnPropertyChanged(); } }

        private Array _EstadoEpisodio;

        public Array EstadoEpisodio { get { return _EstadoEpisodio; } set { _EstadoEpisodio = value; OnPropertyChanged(); } }

        private Enums.EstadoEpisodio _EstadoEpisodioSelecionado;

        public Enums.EstadoEpisodio EstadoEpisodioSelecionado { get { return _EstadoEpisodioSelecionado; } set { _EstadoEpisodioSelecionado = value; OnPropertyChanged(); } }

        public Action ActionFechar { get; set; }

        public ICommand CommandSelecionarTodos { get; set; }

        public ICommand CommandIsSelected { get; set; }

        public ICommand CommandSalvar { get; set; }

        public ICommand CommandFechar { get; set; }

        public EpisodiosViewModel(List<Episode> episodios)
        {
            Episodios = new ObservableCollection<Episode>(episodios);
            EpisodiosView = new ListCollectionView(Episodios);
            EpisodiosView.GroupDescriptions.Add(new PropertyGroupDescription("SeasonNumber"));
            SelecionarTodos = false;
            var estadosParaExibir = new List<int> { 0, 1, 2, 4, 5 };
            EstadoEpisodio = Enum.GetValues(typeof(Enums.EstadoEpisodio)).Cast<Enums.EstadoEpisodio>().Where(x => estadosParaExibir.Contains((int)x)).ToArray();
            EstadoEpisodioSelecionado = Enums.EstadoEpisodio.Selecione;
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