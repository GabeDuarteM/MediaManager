// Developed by: Gabriel Duarte
// 
// Created at: 08/11/2015 23:01

using System;
using System.Collections.Generic;
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
    public class ListaFeedsCommands
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
                return parameter is ListaFeedsViewModel;
            }

            public void Execute(object parameter)
            {
                var oListaFeedsVM = parameter as ListaFeedsViewModel;
                var frmAdicionarFeed = new frmAdicionarFeed();
                frmAdicionarFeed.ShowDialog(oListaFeedsVM.Owner);

                if (frmAdicionarFeed.DialogResult == true)
                {
                    oListaFeedsVM.AtualizarListaFeeds();
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
                return parameter is ListaFeedsViewModel &&
                       (parameter as ListaFeedsViewModel).lstFeeds.Any(x => x.bFlSelecionado);
            }

            public void Execute(object parameter)
            {
                var feedsVM = parameter as ListaFeedsViewModel;

                var feedsService = App.Container.Resolve<FeedsService>();

                List<Feed> lstFeedsSelecionados =
                    feedsVM.lstFeeds.Where(x => x.bFlSelecionado).OrderBy(x => x.nNrPrioridade).ToList();

                foreach (Feed item in lstFeedsSelecionados)
                {
                    Feed oFeedAcima =
                        feedsVM.lstFeeds.Where(
                                               x =>
                                               x.nIdTipoConteudo == item.nIdTipoConteudo && !x.bFlSelecionado &&
                                               x.nNrPrioridade == item.nNrPrioridade - 1).FirstOrDefault();

                    if (oFeedAcima != null)
                    {
                        item.nNrPrioridade--;
                        oFeedAcima.nNrPrioridade++;
                        if (!feedsService.Update(item, oFeedAcima))
                        {
                            Helper.MostrarMensagem("Ocorreu um erro ao alterar a prioridade do feed " + item.sDsFeed,
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
                return parameter is ListaFeedsViewModel &&
                       (parameter as ListaFeedsViewModel).lstFeeds.Any(x => x.bFlSelecionado);
            }

            public void Execute(object parameter)
            {
                var feedsVM = parameter as ListaFeedsViewModel;
                var feedsService = App.Container.Resolve<FeedsService>();
                List<Feed> lstFeedsSelecionados =
                    feedsVM.lstFeeds.Where(x => x.bFlSelecionado).OrderByDescending(x => x.nNrPrioridade).ToList();

                foreach (Feed item in lstFeedsSelecionados)
                {
                    Feed oFeedAbaixo =
                        feedsVM.lstFeeds.Where(
                                               x =>
                                               x.nIdTipoConteudo == item.nIdTipoConteudo && !x.bFlSelecionado &&
                                               x.nNrPrioridade == item.nNrPrioridade + 1).FirstOrDefault();

                    if (oFeedAbaixo != null)
                    {
                        item.nNrPrioridade++;
                        oFeedAbaixo.nNrPrioridade--;
                        if (!feedsService.Update(item, oFeedAbaixo))
                        {
                            Helper.MostrarMensagem("Ocorreu um erro ao alterar a prioridade do feed " + item.sDsFeed,
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
                return parameter is ListaFeedsViewModel &&
                       (parameter as ListaFeedsViewModel).lstFeeds.Any(x => x.bFlSelecionado);
            }

            public void Execute(object parameter)
            {
                var oListaFeedsVM = parameter as ListaFeedsViewModel;

                if (
                    Helper.MostrarMensagem("Você realmente deseja remover os feeds selecionados?",
                                           Enums.eTipoMensagem.QuestionamentoSimNao, "Remover feeds") ==
                    MessageBoxResult.Yes)
                {
                    var feedsService = App.Container.Resolve<FeedsService>();

                    var lstTipoConteudoSelecionado = new List<Enums.TipoConteudo>();
                    lstTipoConteudoSelecionado =
                        oListaFeedsVM.lstFeeds.Where(x => x.bFlSelecionado)
                                     .Select(x => x.nIdTipoConteudo)
                                     .Distinct()
                                     .ToList();

                    feedsService.Remover(oListaFeedsVM.lstFeeds.Where(x => x.bFlSelecionado).ToArray());

                    List<Feed> lstFeeds =
                        feedsService.GetLista()
                                    .Where(
                                           x =>
                                           lstTipoConteudoSelecionado.Contains(x.nIdTipoConteudo) && !x.bIsFeedPesquisa)
                                    .ToList();

                    // Arrumar a prioridade
                    for (var i = 0; i < lstFeeds.Count - 1; i++)
                    {
                        if (i == 0 && lstFeeds[i].nNrPrioridade != 1)
                        {
                            lstFeeds[i].nNrPrioridade = 1;
                        }

                        if (lstFeeds[i].nNrPrioridade + 1 != lstFeeds[i + 1].nNrPrioridade)
                        {
                            lstFeeds[i + 1].nNrPrioridade = lstFeeds[i].nNrPrioridade + 1;
                            feedsService.Update(lstFeeds[i + 1]);
                        }
                    }

                    oListaFeedsVM.AtualizarListaFeeds();
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
                return parameter is ListaFeedsViewModel;
            }

            public void Execute(object parameter)
            {
                var feedsVM = parameter as ListaFeedsViewModel;
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
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsViewModel;
            }

            public void Execute(object parameter)
            {
                var feedsVM = parameter as ListaFeedsViewModel;
                if (feedsVM.bFlSelecionarTodos == true)
                {
                    foreach (Feed feed in feedsVM.lstFeeds)
                    {
                        feed.bFlSelecionado = true;
                    }
                }
                else
                {
                    feedsVM.bFlSelecionarTodos = false;
                    foreach (Feed feed in feedsVM.lstFeeds)
                    {
                        feed.bFlSelecionado = false;
                    }
                }
            }
        }
    }
}
