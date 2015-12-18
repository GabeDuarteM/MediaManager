using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Autofac;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Services;

namespace MediaManager.ViewModel
{
    public class ListaFeedsPesquisaViewModel : ViewModelBase, IListaFeedsViewModel
    {
        private List<Feed> _lstFeeds;

        public List<Feed> lstFeeds { get { return _lstFeeds; } set { _lstFeeds = value; OnPropertyChanged(); } }

        private CollectionViewSource _lstFeedsView;

        public CollectionViewSource lstFeedsView { get { return _lstFeedsView; } set { _lstFeedsView = value; OnPropertyChanged(); } }

        private bool? _bFlSelecionarTodos;

        public bool? bFlSelecionarTodos { get { return _bFlSelecionarTodos; } set { _bFlSelecionarTodos = value; OnPropertyChanged(); } }

        public Window Owner { get; set; }

        public ICommand CommandAdicionarFeed { get; set; }

        public ICommand CommandAumentarPrioridadeFeed { get; set; }

        public ICommand CommandDiminuirPrioridadeFeed { get; set; }

        public ICommand CommandRemoverFeed { get; set; }

        public ICommand CommandSelecionar { get; set; }

        public ICommand CommandSelecionarTodos { get; set; }

        public ListaFeedsPesquisaViewModel(List<Feed> lstFeeds = null /* Teste unitário ¬¬ */)
        {
            CommandAdicionarFeed = new ListaFeedsPesquisaCommands.CommandAdicionarFeed();
            CommandAumentarPrioridadeFeed = new ListaFeedsPesquisaCommands.CommandAumentarPrioridadeFeed();
            CommandDiminuirPrioridadeFeed = new ListaFeedsPesquisaCommands.CommandDiminuirPrioridadeFeed();
            CommandRemoverFeed = new ListaFeedsPesquisaCommands.CommandRemoverFeed();
            CommandSelecionar = new ListaFeedsPesquisaCommands.CommandSelecionar();
            CommandSelecionarTodos = new ListaFeedsPesquisaCommands.CommandSelecionarTodos();
            AtualizarListaFeeds(lstFeeds);
        }

        public void AtualizarListaFeeds(List<Feed> lstFeeds = null)
        {
            FeedsService feedsService = App.Container.Resolve<FeedsService>();

            this.lstFeeds = (lstFeeds == null) ? feedsService.GetLista().Where(x => x.bIsFeedPesquisa).ToList() : lstFeeds;
            lstFeedsView = new CollectionViewSource();
            lstFeedsView.Source = this.lstFeeds;
            lstFeedsView.SortDescriptions.Add(new SortDescription("nNrPrioridade", ListSortDirection.Ascending));
            lstFeedsView.IsLiveSortingRequested = true;
            lstFeedsView.GroupDescriptions.Add(new PropertyGroupDescription("sDsTipoConteudo"));
            CommandSelecionar.Execute(this);
        }
    }
}