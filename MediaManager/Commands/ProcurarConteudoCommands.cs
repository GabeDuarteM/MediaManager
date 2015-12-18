using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class ProcurarConteudoCommands
    {
        public class CommandAdicionar : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ProcurarConteudoViewModel
                    && (parameter as ProcurarConteudoViewModel).lstConteudos.Count > 0
                    && (parameter as ProcurarConteudoViewModel).lstConteudos.Where(x => x.bFlSelecionado && !x.bFlNaoEncontrado).Count() > 0;
            }

            public void Execute(object parameter)
            {
                ProcurarConteudoViewModel ProcurarConteudoViewModel = parameter as ProcurarConteudoViewModel;
                frmBarraProgresso frmBarraProgresso = new frmBarraProgresso();
                frmBarraProgresso.BarraProgressoViewModel.dNrProgressoMaximo = ProcurarConteudoViewModel.lstConteudos.Count;
                frmBarraProgresso.BarraProgressoViewModel.sDsTarefa = "Salvando...";
                frmBarraProgresso.BarraProgressoViewModel.Worker.DoWork += (s, ev) =>
                {
                    SeriesService seriesService = App.Container.Resolve<SeriesService>();
                    //if (ProcurarConteudoViewModel.lstConteudos.Where(x => x.bFlSelecionado && !x.bFlNaoEncontrado).Count() == 0)
                    //{
                    //    Helper.MostrarMensagem("Para realizar a operação, selecione ao menos um registro.", Enums.eTipoMensagem.Alerta);
                    //}
                    foreach (var item in ProcurarConteudoViewModel.lstConteudos)
                    {
                        if (item.bFlSelecionado && !item.bFlNaoEncontrado)
                        {
                            switch (item.nIdTipoConteudo)
                            {
                                case Enums.TipoConteudo.Série:
                                case Enums.TipoConteudo.Anime:
                                    {
                                        if (item.nIdEstado != Enums.Estado.Completo)
                                        {
                                            frmBarraProgresso.BarraProgressoViewModel.sDsTexto = "Salvando " + item.sDsTitulo + "...";
                                            Serie serie = APIRequests.GetSerieInfoAsync(item.nCdApi, Properties.Settings.Default.pref_IdiomaPesquisa).Result;
                                            serie.nIdTipoConteudo = item.nIdTipoConteudo;
                                            serie.sDsPasta = item.sDsPasta;
                                            serie.sAliases = item.sAliases;
                                            serie.lstSerieAlias = item.lstSerieAlias;
                                            serie.sDsTitulo = item.sDsTitulo;
                                            seriesService.Adicionar(serie);
                                            frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                        }
                                        else
                                        {
                                            frmBarraProgresso.BarraProgressoViewModel.sDsTexto = "Salvando " + item.sDsTitulo + "...";
                                            seriesService.Adicionar((Serie)item);
                                            frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                        }
                                        break;
                                    }
                                case Enums.TipoConteudo.Filme:
                                    // TODO Fazer funfar
                                    //Filme filme = await Helper.API_GetFilmeInfoAsync(item.TraktSlug);
                                    //filme.FolderPath = item.Pasta;
                                    //await DatabaseHelper.AddFilmeAsync(filme);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }

                    Helper.MostrarMensagem("Séries inseridas com sucesso.", Enums.eTipoMensagem.Informativa);
                };

                frmBarraProgresso.BarraProgressoViewModel.Worker.RunWorkerCompleted += (s, ev) =>
                {
                    ProcurarConteudoViewModel.ActionFechar();
                };
                frmBarraProgresso.BarraProgressoViewModel.Worker.RunWorkerAsync();
                frmBarraProgresso.ShowDialog(ProcurarConteudoViewModel.Owner);
            }
        }

        public class CommandSelecionar : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ProcurarConteudoViewModel;
            }

            public void Execute(object parameter)
            {
                var procurarConteudoVM = parameter as ProcurarConteudoViewModel;
                int conteudosSelecionadosCount = procurarConteudoVM.lstConteudos.Where(x => x.bFlSelecionado).Count();
                if (conteudosSelecionadosCount == procurarConteudoVM.lstConteudos.Count && procurarConteudoVM.lstConteudos.Count > 0)
                {
                    procurarConteudoVM.bFlSelecionarTodos = true;
                }
                else if (conteudosSelecionadosCount == 0)
                {
                    procurarConteudoVM.bFlSelecionarTodos = false;
                }
                else if (conteudosSelecionadosCount > 0)
                {
                    procurarConteudoVM.bFlSelecionarTodos = null;
                }
            }
        }

        public class CommandSelecionarTodos : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is ProcurarConteudoViewModel;
            }

            public void Execute(object parameter)
            {
                var procurarConteudoVM = parameter as ProcurarConteudoViewModel;
                if (procurarConteudoVM.bFlSelecionarTodos == true)
                {
                    procurarConteudoVM.lstConteudos.ToList().ForEach(x => x.bFlSelecionado = true);
                }
                else
                {
                    procurarConteudoVM.bFlSelecionarTodos = false;
                    procurarConteudoVM.lstConteudos.ToList().ForEach(x => x.bFlSelecionado = false);
                }
            }
        }
    }
}