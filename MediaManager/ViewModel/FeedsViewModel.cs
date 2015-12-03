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
        private List<Feed> _lstFeeds;

        public List<Feed> lstFeeds { get { return _lstFeeds; } set { _lstFeeds = value; OnPropertyChanged(); } }

        private CollectionViewSource _lstFeedsView;

        public CollectionViewSource lstFeedsView { get { return _lstFeedsView; } set { _lstFeedsView = value; OnPropertyChanged(); } }

        private bool? _bFlSelecionarTodos;

        public bool? bFlSelecionarTodos { get { return _bFlSelecionarTodos; } set { _bFlSelecionarTodos = value; OnPropertyChanged(); } }

        public ICommand CommandAdicionarFeed { get; set; }

        public ICommand CommandAumentarPrioridadeFeed { get; set; }

        public ICommand CommandDiminuirPrioridadeFeed { get; set; }

        public ICommand CommandRemoverFeed { get; set; }

        public ICommand CommandSelecionar { get; set; }

        public ICommand CommandSelecionarTodos { get; set; }

        public FeedsViewModel(List<Feed> lstFeeds = null /* Teste unitário ¬¬ */)
        {
            this.lstFeeds = (lstFeeds == null) ? this.lstFeeds = DBHelper.GetFeeds() : this.lstFeeds = lstFeeds;
            lstFeedsView = new CollectionViewSource();
            lstFeedsView.Source = this.lstFeeds;
            lstFeedsView.SortDescriptions.Add(new SortDescription("nNrPrioridade", ListSortDirection.Ascending));
            lstFeedsView.IsLiveSortingRequested = true;
            lstFeedsView.GroupDescriptions.Add(new PropertyGroupDescription("sDsTipoConteudo"));
            CommandAdicionarFeed = new FeedsCommands.CommandAdicionarFeed();
            CommandAumentarPrioridadeFeed = new FeedsCommands.CommandAumentarPrioridadeFeed();
            CommandDiminuirPrioridadeFeed = new FeedsCommands.CommandDiminuirPrioridadeFeed();
            CommandRemoverFeed = new FeedsCommands.CommandRemoverFeed();
            CommandSelecionar = new FeedsCommands.CommandSelecionar();
            CommandSelecionarTodos = new FeedsCommands.CommandSelecionarTodos();
            CommandSelecionar.Execute(this);
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