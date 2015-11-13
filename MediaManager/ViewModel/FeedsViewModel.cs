using System;
using System.Collections.Generic;
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
    public class FeedsViewModel : INotifyPropertyChanged
    {
        private List<Feed> _Feeds;

        public List<Feed> Feeds { get { return _Feeds; } set { _Feeds = value; OnPropertyChanged(); } }

        private CollectionViewSource _FeedsView;

        public CollectionViewSource FeedsView { get { return _FeedsView; } set { _FeedsView = value; OnPropertyChanged(); } }

        private bool? _bFlSelecionarTodosFeeds;

        public bool? bFlSelecionarTodosFeeds { get { return _bFlSelecionarTodosFeeds; } set { _bFlSelecionarTodosFeeds = value; OnPropertyChanged(); } }

        public ICommand CommandAdicionarFeed { get; set; }

        public ICommand CommandAumentarPrioridadeFeed { get; set; }

        public ICommand CommandDiminuirPrioridadeFeed { get; set; }

        public ICommand CommandRemoverFeed { get; set; }

        public ICommand CommandSelecionar { get; set; }

        public FeedsViewModel(List<Feed> feeds = null /* Teste unitário ¬¬ */)
        {
            if (feeds == null)
                Feeds = DBHelper.GetFeeds();
            else
                Feeds = feeds;
            FeedsView = new CollectionViewSource();
            FeedsView.Source = Feeds;
            FeedsView.SortDescriptions.Add(new SortDescription("nNrPrioridade", ListSortDirection.Ascending));
            FeedsView.IsLiveSortingRequested = true;
            FeedsView.GroupDescriptions.Add(new PropertyGroupDescription("sDsTipoConteudo"));
            CommandAdicionarFeed = new FeedsCommands.CommandAdicionarFeed();
            CommandAumentarPrioridadeFeed = new FeedsCommands.CommandAumentarPrioridadeFeed();
            CommandDiminuirPrioridadeFeed = new FeedsCommands.CommandDiminuirPrioridadeFeed();
            CommandRemoverFeed = new FeedsCommands.CommandRemoverFeed();
            CommandSelecionar = new FeedsCommands.CommandSelecionar();
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