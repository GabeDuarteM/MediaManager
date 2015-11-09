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

        private ICollectionView _FeedsView;

        public ICollectionView FeedsView { get { return _FeedsView; } set { _FeedsView = value; OnPropertyChanged(); } }

        private bool? _bFlSelecionarTodosFeeds;

        public bool? bFlSelecionarTodosFeeds { get { return _bFlSelecionarTodosFeeds; } set { _bFlSelecionarTodosFeeds = value; OnPropertyChanged(); } }

        private ICommand _CommandAdicionarFeed;

        public ICommand CommandAdicionarFeed { get { return _CommandAdicionarFeed; } set { _CommandAdicionarFeed = value; OnPropertyChanged(); } }

        private ICommand _CommandAumentarPrioridadeFeed;

        public ICommand CommandAumentarPrioridadeFeed { get { return _CommandAumentarPrioridadeFeed; } set { _CommandAumentarPrioridadeFeed = value; OnPropertyChanged(); } }

        private ICommand _CommandDiminuirPrioridadeFeed;

        public ICommand CommandDiminuirPrioridadeFeed { get { return _CommandDiminuirPrioridadeFeed; } set { _CommandDiminuirPrioridadeFeed = value; OnPropertyChanged(); } }

        private ICommand _CommandRemoverFeed;

        public ICommand CommandRemoverFeed { get { return _CommandRemoverFeed; } set { _CommandRemoverFeed = value; OnPropertyChanged(); } }

        public FeedsViewModel(List<Feed> feeds = null /* Teste unitário ¬¬ */)
        {
            if (feeds == null)
                Feeds = DBHelper.GetFeeds();
            else
                Feeds = feeds;
            FeedsView = new ListCollectionView(Feeds);
            FeedsView.GroupDescriptions.Add(new PropertyGroupDescription("sDsTipoConteudo"));
            CommandAdicionarFeed = new FeedsCommands.CommandAdicionarFeed();
            CommandAumentarPrioridadeFeed = new FeedsCommands.CommandAumentarPrioridadeFeed();
            CommandDiminuirPrioridadeFeed = new FeedsCommands.CommandDiminuirPrioridadeFeed();
            CommandRemoverFeed = new FeedsCommands.CommandRemoverFeed();
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