using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class FeedsCommands
    {
        public class CommandAdicionarFeed : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is FeedsViewModel;
            }

            public void Execute(object parameter)
            {
                throw new NotImplementedException("Ainda não cara, ainda não...");
            }
        }

        public class CommandAumentarPrioridadeFeed : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is FeedsViewModel;
            }

            public void Execute(object parameter)
            {
                var feedsVM = parameter as FeedsViewModel;

                var feedsSelecionadosSeries = feedsVM.Feeds.Where(x => x.bFlSelecionado && x.nIdTipoConteudo == Helpers.Enums.ContentType.show);
                var feedsSelecionadosAnimes = feedsVM.Feeds.Where(x => x.bFlSelecionado && x.nIdTipoConteudo == Helpers.Enums.ContentType.anime);
                var feedsAlterados = new List<Feed>();

                foreach (var item in feedsSelecionadosSeries)
                {
                    if (item.nNrPrioridade == 1)
                        continue;
                    var feedAcima = feedsVM.Feeds.Where(x => x.nIdTipoConteudo == Helpers.Enums.ContentType.show && x.nNrPrioridade == item.nNrPrioridade - 1).First();

                    if (feedAcima != null && !feedsAlterados.Contains(feedAcima))
                    {
                        feedsVM.Feeds.Where(x => x.nCdFeed == item.nCdFeed).First().nNrPrioridade++;
                        feedsVM.Feeds.Where(x => x.nCdFeed == feedAcima.nCdFeed).First().nNrPrioridade--;
                    }
                }
            }
        }

        public class CommandDiminuirPrioridadeFeed : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is FeedsViewModel;
            }

            public void Execute(object parameter)
            {
            }
        }

        public class CommandRemoverFeed : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is FeedsViewModel;
            }

            public void Execute(object parameter)
            {
            }
        }
    }
}