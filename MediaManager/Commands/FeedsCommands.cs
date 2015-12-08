using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MediaManager.Helpers;
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
                return;
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
                DBHelper DBHelper = new DBHelper();

                if (feedsVM.lstFeeds.Where(x => x.bFlSelecionado).Count() == 1)
                {
                    var feed = feedsVM.lstFeeds.Where(x => x.bFlSelecionado && x.nNrPrioridade > 1).FirstOrDefault();
                    if (feed != null)
                    {
                        var feedAcima = feedsVM.lstFeeds.Where(x => x.nNrPrioridade == feed.nNrPrioridade - 1 && x.nIdTipoConteudo == feed.nIdTipoConteudo).FirstOrDefault();
                        feed.nNrPrioridade--;
                        feedAcima.nNrPrioridade++;
                        if (DBHelper.UpdateFeed(feed, feedAcima) == false)
                        {
                            Helper.MostrarMensagem("Ocorreu um erro ao alterar a prioridade do feed " + feed.sDsFeed, Enums.eTipoMensagem.Erro);
                        }
                    }
                }
                else
                {
                    Helper.MostrarMensagem("Para realizar a operação, selecione somente um registro.", Enums.eTipoMensagem.Alerta);
                }
                //var feedsSelecionadosSeries = feedsVM.Feeds.Where(x => x.bFlSelecionado && x.nIdTipoConteudo == Helpers.Enums.ContentType.show);
                //var feedsSelecionadosAnimes = feedsVM.Feeds.Where(x => x.bFlSelecionado && x.nIdTipoConteudo == Helpers.Enums.ContentType.anime);
                //var feedsAlterados = new List<Feed>();

                //foreach (var item in feedsSelecionadosSeries)
                //{
                //    if (item.nNrPrioridade == 1)
                //        continue;
                //    var feedAcima = feedsVM.Feeds.Where(x => x.nIdTipoConteudo == Helpers.Enums.ContentType.show && x.nNrPrioridade == item.nNrPrioridade - 1).First();

                //    if (feedAcima != null && !feedsAlterados.Contains(feedAcima))
                //    {
                //        feedsVM.Feeds.Where(x => x.nCdFeed == item.nCdFeed).First().nNrPrioridade++;
                //        feedsVM.Feeds.Where(x => x.nCdFeed == feedAcima.nCdFeed).First().nNrPrioridade--;
                //    }
                //}
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
                var feedsVM = parameter as FeedsViewModel;
                DBHelper DBHelper = new DBHelper();

                if (feedsVM.lstFeeds.Where(x => x.bFlSelecionado).Count() == 1)
                {
                    var feed = feedsVM.lstFeeds.Where(x => x.bFlSelecionado && x.nNrPrioridade < feedsVM.lstFeeds.Where(y => y.nIdTipoConteudo == x.nIdTipoConteudo).Count()).FirstOrDefault();
                    if (feed != null)
                    {
                        var feedAbaixo = feedsVM.lstFeeds.Where(x => x.nNrPrioridade == feed.nNrPrioridade + 1 && x.nIdTipoConteudo == feed.nIdTipoConteudo).FirstOrDefault();
                        feed.nNrPrioridade++;
                        feedAbaixo.nNrPrioridade--;
                        if (DBHelper.UpdateFeed(feed, feedAbaixo) == false)
                        {
                            Helper.MostrarMensagem("Ocorreu um erro ao alterar a prioridade do feed " + feed.sDsFeed, Enums.eTipoMensagem.Erro);
                        }
                    }
                }
                else
                {
                    Helper.MostrarMensagem("Para realizar a operação, selecione somente um registro.", Enums.eTipoMensagem.Alerta);
                }
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

        public class CommandSelecionar : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is FeedsViewModel;
            }

            public void Execute(object parameter)
            {
                var feedsVM = parameter as FeedsViewModel;
                int feedsSelecionadosCount = feedsVM.lstFeeds.Where(x => x.bFlSelecionado).Count();
                if (feedsSelecionadosCount == feedsVM.lstFeeds.Count && feedsVM.lstFeeds.Count > 0)
                {
                    feedsVM.bFlSelecionarTodos = true;
                }
                else if (feedsSelecionadosCount == 0)
                {
                    feedsVM.bFlSelecionarTodos = false;
                }
                else if (feedsSelecionadosCount > 0)
                {
                    feedsVM.bFlSelecionarTodos = null;
                }
            }
        }

        public class CommandSelecionarTodos : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is FeedsViewModel;
            }

            public void Execute(object parameter)
            {
                var feedsVM = parameter as FeedsViewModel;
                if (feedsVM.bFlSelecionarTodos == true)
                {
                    foreach (var feed in feedsVM.lstFeeds)
                    {
                        feed.bFlSelecionado = true;
                    }
                }
                else
                {
                    feedsVM.bFlSelecionarTodos = false;
                    foreach (var feed in feedsVM.lstFeeds)
                    {
                        feed.bFlSelecionado = false;
                    }
                }
            }
        }
    }
}