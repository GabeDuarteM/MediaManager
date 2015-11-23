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
        private List<Feed> _ListaFeeds;

        public List<Feed> ListaFeeds { get { return _ListaFeeds; } set { _ListaFeeds = value; OnPropertyChanged(); } }

        private CollectionViewSource _ListaFeedsView;

        public CollectionViewSource ListaFeedsView { get { return _ListaFeedsView; } set { _ListaFeedsView = value; OnPropertyChanged(); } }

        private bool? _bFlSelecionarTodos;

        public bool? bFlSelecionarTodos { get { return _bFlSelecionarTodos; } set { _bFlSelecionarTodos = value; OnPropertyChanged(); } }

        public ICommand CommandAdicionarFeed { get; set; }

        public ICommand CommandAumentarPrioridadeFeed { get; set; }

        public ICommand CommandDiminuirPrioridadeFeed { get; set; }

        public ICommand CommandRemoverFeed { get; set; }

        public ICommand CommandSelecionar { get; set; }

        public ICommand CommandSelecionarTodos { get; set; }

        public FeedsViewModel(List<Feed> listaFeeds = null /* Teste unitário ¬¬ */)
        {
            ListaFeeds = (listaFeeds == null) ? ListaFeeds = DBHelper.GetFeeds() : ListaFeeds = listaFeeds;
            ListaFeedsView = new CollectionViewSource();
            ListaFeedsView.Source = ListaFeeds;
            ListaFeedsView.SortDescriptions.Add(new SortDescription("nNrPrioridade", ListSortDirection.Ascending));
            ListaFeedsView.IsLiveSortingRequested = true;
            ListaFeedsView.GroupDescriptions.Add(new PropertyGroupDescription("sDsTipoConteudo"));
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