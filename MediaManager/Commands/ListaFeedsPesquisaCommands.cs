using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class ListaFeedsPesquisaCommands
    {
        public class CommandAdicionarFeed : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel;
            }

            public void Execute(object parameter)
            {
                ListaFeedsPesquisaViewModel oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;
                frmAdicionarFeedPesquisa frmAdicionarFeedPesquisa = new frmAdicionarFeedPesquisa();
                frmAdicionarFeedPesquisa.ShowDialog(oListaFeedsPesquisaVM.Owner);

                if (frmAdicionarFeedPesquisa.DialogResult == true)
                {
                    oListaFeedsPesquisaVM.AtualizarListaFeeds();
                }
            }
        }

        public class CommandAumentarPrioridadeFeed : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel && (parameter as ListaFeedsPesquisaViewModel).lstFeeds.Any(x => x.bFlSelecionado);
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;
                DBHelper db = new DBHelper();

                var lstFeedsSelecionados = oListaFeedsPesquisaVM.lstFeeds.Where(x => x.bFlSelecionado).OrderBy(x => x.nNrPrioridade).ToList();

                foreach (var item in lstFeedsSelecionados)
                {
                    Feed oFeedAcima = oListaFeedsPesquisaVM.lstFeeds.Where(x => x.nIdTipoConteudo == item.nIdTipoConteudo && !x.bFlSelecionado && x.nNrPrioridade == item.nNrPrioridade - 1).FirstOrDefault();

                    if (oFeedAcima != null)
                    {
                        item.nNrPrioridade--;
                        oFeedAcima.nNrPrioridade++;
                        if (!db.UpdateFeed(item, oFeedAcima))
                        {
                            Helper.MostrarMensagem("Ocorreu um erro ao alterar a prioridade do feed de pesquisa " + item.sDsFeed, Enums.eTipoMensagem.Erro);
                        }
                    }
                }
            }
        }

        public class CommandDiminuirPrioridadeFeed : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel && (parameter as ListaFeedsPesquisaViewModel).lstFeeds.Any(x => x.bFlSelecionado);
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;
                DBHelper db = new DBHelper();

                var lstFeedsSelecionados = oListaFeedsPesquisaVM.lstFeeds.Where(x => x.bFlSelecionado).OrderByDescending(x => x.nNrPrioridade).ToList();

                foreach (var item in lstFeedsSelecionados)
                {
                    Feed oFeedAbaixo = oListaFeedsPesquisaVM.lstFeeds.Where(x => x.nIdTipoConteudo == item.nIdTipoConteudo && !x.bFlSelecionado && x.nNrPrioridade == item.nNrPrioridade + 1).FirstOrDefault();

                    if (oFeedAbaixo != null)
                    {
                        item.nNrPrioridade++;
                        oFeedAbaixo.nNrPrioridade--;
                        if (!db.UpdateFeed(item, oFeedAbaixo))
                        {
                            Helper.MostrarMensagem("Ocorreu um erro ao alterar a prioridade do feed de pesquisa " + item.sDsFeed, Enums.eTipoMensagem.Erro);
                        }
                    }
                }
            }
        }

        public class CommandRemoverFeed : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel && (parameter as ListaFeedsPesquisaViewModel).lstFeeds.Any(x => x.bFlSelecionado);
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;

                if (Helper.MostrarMensagem("Você realmente deseja remover os feeds selecionados?", Enums.eTipoMensagem.QuestionamentoSimNao, "Remover feeds") == MessageBoxResult.Yes)
                {
                    DBHelper db = new DBHelper();

                    db.RemoveFeed(oListaFeedsPesquisaVM.lstFeeds.Where(x => x.bFlSelecionado));
                    oListaFeedsPesquisaVM.AtualizarListaFeeds();
                }
            }
        }

        public class CommandSelecionar : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel;
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;
                int feedsSelecionadosCount = oListaFeedsPesquisaVM.lstFeeds.Where(x => x.bFlSelecionado).Count();
                if (feedsSelecionadosCount == oListaFeedsPesquisaVM.lstFeeds.Count && oListaFeedsPesquisaVM.lstFeeds.Count > 0)
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
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ListaFeedsPesquisaViewModel;
            }

            public void Execute(object parameter)
            {
                var oListaFeedsPesquisaVM = parameter as ListaFeedsPesquisaViewModel;
                if (oListaFeedsPesquisaVM.bFlSelecionarTodos == true)
                {
                    foreach (var feed in oListaFeedsPesquisaVM.lstFeeds)
                    {
                        feed.bFlSelecionado = true;
                    }
                }
                else
                {
                    oListaFeedsPesquisaVM.bFlSelecionarTodos = false;
                    foreach (var feed in oListaFeedsPesquisaVM.lstFeeds)
                    {
                        feed.bFlSelecionado = false;
                    }
                }
            }
        }
    }
}