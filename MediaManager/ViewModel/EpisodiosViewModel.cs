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
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class EpisodiosViewModel : INotifyPropertyChanged
    {
        private ICollectionView _episodiosView;

        public ICollectionView EpisodiosView { get { return _episodiosView; } set { _episodiosView = value; OnPropertyChanged(); } }

        private ObservableCollection<Episode> _episodios;

        public ObservableCollection<Episode> Episodios { get { return _episodios; } set { _episodios = value; OnPropertyChanged(); } }

        private bool _selecionarTodos;

        public bool SelecionarTodos { get { return _selecionarTodos; } set { _selecionarTodos = value; OnPropertyChanged(); } }

        public ICommand SelecionarTodosCommand { get; set; }

        public EpisodiosViewModel(List<Episode> episodios)
        {
            Episodios = new ObservableCollection<Episode>(episodios);
            EpisodiosView = new ListCollectionView(Episodios);
            EpisodiosView.GroupDescriptions.Add(new PropertyGroupDescription("SeasonNumber"));
            SelecionarTodosCommand = new EpisodiosCommand.SelecionarTodosCommand();
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