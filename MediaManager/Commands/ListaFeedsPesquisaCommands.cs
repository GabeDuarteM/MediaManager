// Developed by: Gabriel Duarte
// 
// Created at: 12/12/2015 07:40
// Last update: 19/04/2016 02:46

using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Services;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class ListaFeedsPesquisaCommands
    {
        public class CommandAdicionarFeed : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel;
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;
                var frmAdicionarFeedPesquisa = new frmAdicionarFeedPesquisa();
                frmAdicionarFeedPesquisa.ShowDialog(oListaFeedsPesquisaVM.Owner);

                if (frmAdicionarFeedPesquisa.DialogResult == true)
                {
                    oListaFeedsPesquisaVM.AtualizarListaFeeds();
                }
            }
        }

        public class CommandAumentarPrioridadeFeed : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel &&
                       (parameter as ListaFeedsPesquisaViewModel).lstFeeds.Any(x => x.bFlSelecionado);
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;

                var feedsService = App.Container.Resolve<FeedsService>();

                var lstFeedsSelecionados =
                    oListaFeedsPesquisaVM.lstFeeds.Where(x => x.bFlSelecionado).OrderBy(x => x.nNrPrioridade).ToList();

                foreach (Feed item in lstFeedsSelecionados)
                {
                    Feed oFeedAcima =
                        oListaFeedsPesquisaVM.lstFeeds.Where(
                                                             x =>
                                                             x.nIdTipoConteudo == item.nIdTipoConteudo &&
                                                             !x.bFlSelecionado &&
                                                             x.nNrPrioridade == item.nNrPrioridade - 1).FirstOrDefault();

                    if (oFeedAcima != null)
                    {
                        item.nNrPrioridade--;
                        oFeedAcima.nNrPrioridade++;
                        if (!feedsService.Update(item, oFeedAcima))
                        {
                            Helper.MostrarMensagem(
                                                   "Ocorreu um erro ao alterar a prioridade do feed de pesquisa " +
                                                   item.sDsFeed,
                                                   Enums.eTipoMensagem.Erro);
                        }
                    }
                }
            }
        }

        public class CommandDiminuirPrioridadeFeed : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel &&
                       (parameter as ListaFeedsPesquisaViewModel).lstFeeds.Any(x => x.bFlSelecionado);
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;
                var feedsService = App.Container.Resolve<FeedsService>();

                var lstFeedsSelecionados =
                    oListaFeedsPesquisaVM.lstFeeds.Where(x => x.bFlSelecionado)
                                         .OrderByDescending(x => x.nNrPrioridade)
                                         .ToList();

                foreach (Feed item in lstFeedsSelecionados)
                {
                    Feed oFeedAbaixo =
                        oListaFeedsPesquisaVM.lstFeeds.Where(
                                                             x =>
                                                             x.nIdTipoConteudo == item.nIdTipoConteudo &&
                                                             !x.bFlSelecionado &&
                                                             x.nNrPrioridade == item.nNrPrioridade + 1).FirstOrDefault();

                    if (oFeedAbaixo != null)
                    {
                        item.nNrPrioridade++;
                        oFeedAbaixo.nNrPrioridade--;
                        if (!feedsService.Update(item, oFeedAbaixo))
                        {
                            Helper.MostrarMensagem(
                                                   "Ocorreu um erro ao alterar a prioridade do feed de pesquisa " +
                                                   item.sDsFeed,
                                                   Enums.eTipoMensagem.Erro);
                        }
                    }
                }
            }
        }

        public class CommandRemoverFeed : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel &&
                       (parameter as ListaFeedsPesquisaViewModel).lstFeeds.Any(x => x.bFlSelecionado);
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;

                if (
                    Helper.MostrarMensagem("Você realmente deseja remover os feeds selecionados?",
                                           Enums.eTipoMensagem.QuestionamentoSimNao, "Remover feeds") ==
                    MessageBoxResult.Yes)
                {
                    var feedsService = App.Container.Resolve<FeedsService>();

                    feedsService.Remover(oListaFeedsPesquisaVM.lstFeeds.Where(x => x.bFlSelecionado).ToArray());
                    oListaFeedsPesquisaVM.AtualizarListaFeeds();
                }
            }
        }

        public class CommandSelecionar : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel;
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;
                var feedsSelecionadosCount = oListaFeedsPesquisaVM.lstFeeds.Where(x => x.bFlSelecionado).Count();
                if (feedsSelecionadosCount == oListaFeedsPesquisaVM.lstFeeds.Count &&
                    oListaFeedsPesquisaVM.lstFeeds.Count > 0)
                {
                    oListaFeedsPesquisaVM.bFlSelecionarTodos = true;
                }
                else if (feedsSelecionadosCount == 0)
                {
                    oListaFeedsPesquisaVM.bFlSelecionarTodos = false;
                }
                else if (feedsSelecionadosCount > 0)
                {
                    oListaFeedsPesquisaVM.bFlSelecionarTodos = null;
                }
            }
        }

        public class CommandSelecionarTodos : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel;
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;
                if (oListaFeedsPesquisaVM.bFlSelecionarTodos == true)
                {
                    foreach (Feed feed in oListaFeedsPesquisaVM.lstFeeds)
                    {
                        feed.bFlSelecionado = true;
                    }
                }
                else
                {
                    oListaFeedsPesquisaVM.bFlSelecionarTodos = false;
                    foreach (Feed feed in oListaFeedsPesquisaVM.lstFeeds)
                    {
                        feed.bFlSelecionado = false;
                    }
                }
            }
        }
    }
}
