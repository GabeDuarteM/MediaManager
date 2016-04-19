using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public interface IListaFeedsViewModel
    {
        List<Feed> lstFeeds { get; set; }

        CollectionViewSource lstFeedsView { get; set; }

        bool? bFlSelecionarTodos { get; set; }

        Window Owner { get; set; }

        ICommand CommandAdicionarFeed { get; set; }

        ICommand CommandAumentarPrioridadeFeed { get; set; }

        ICommand CommandDiminuirPrioridadeFeed { get; set; }

        ICommand CommandRemoverFeed { get; set; }

        ICommand CommandSelecionar { get; set; }

        ICommand CommandSelecionarTodos { get; set; }

        void AtualizarListaFeeds(List<Feed> lstFeeds = null);
    }
}