using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Services;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class AdicionarConteudoCommands
    {
        public class CommandAbrirEpisodios : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is AdicionarConteudoViewModel &&
                                    ((parameter as AdicionarConteudoViewModel).nIdTipoConteudo == Helpers.Enums.TipoConteudo.Anime
                                    || (parameter as AdicionarConteudoViewModel).nIdTipoConteudo == Helpers.Enums.TipoConteudo.Série) &&
                    ((parameter as AdicionarConteudoViewModel).oVideoSelecionado.nIdEstado == Helpers.Enums.Estado.Completo ||
                    (parameter as AdicionarConteudoViewModel).oVideoSelecionado.nIdEstado == Helpers.Enums.Estado.CompletoSemForeignKeys);
            }

            public void Execute(object parameter)
            {
                AdicionarConteudoViewModel AdicionarConteudoVM = parameter as AdicionarConteudoViewModel;

                frmEpisodios frmEpisodios = new frmEpisodios(AdicionarConteudoVM.oVideoSelecionado);
                frmEpisodios.Show();
            }
        }

        public class CommandConfigurarConteudo : ICommand
        {
            private object db;

            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is AdicionarConteudoViewModel && (parameter as AdicionarConteudoViewModel).oVideoSelecionado != null &&
                    ((parameter as AdicionarConteudoViewModel).oVideoSelecionado.nIdEstado == Helpers.Enums.Estado.Completo ||
                    (parameter as AdicionarConteudoViewModel).oVideoSelecionado.nIdEstado == Helpers.Enums.Estado.CompletoSemForeignKeys);
            }

            public void Execute(object parameter)
            {
                AdicionarConteudoViewModel AdicionarConteudoVM = parameter as AdicionarConteudoViewModel;

                frmConfigConteudo frmConfigConteudo = new frmConfigConteudo(AdicionarConteudoVM.oVideoSelecionado);
                frmConfigConteudo.ShowDialog();
                if (frmConfigConteudo.DialogResult == true && AdicionarConteudoVM.oVideoSelecionado.nCdVideo > 0)
                {
                    //AdicionarConteudoVM.lstResultPesquisa.First(x => x.nCdApi == AdicionarConteudoVM.oVideoSelecionado.nCdApi).lstSerieAlias = frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias;
                    SerieAliasService serieAliasService = App.Container.Resolve<SerieAliasService>();

                    var lstSerieAliasOriginal = serieAliasService.GetLista(AdicionarConteudoVM.oVideoSelecionado);

                    foreach (var item in AdicionarConteudoVM.oVideoSelecionado.lstSerieAlias.Where(x => x.nCdAlias == 0))
                    {
                        serieAliasService.Adicionar(item);
                    }

                    serieAliasService.Remover(lstSerieAliasOriginal.Where(x => !AdicionarConteudoVM.oVideoSelecionado.lstSerieAlias.Contains(x)).ToArray());
                }
                //foreach (var item in AdicionarConteudoVM.lstResultPesquisa) // Fora do if do DialogResult pois os aliases são salvos direto na tela e independem do resultado do DialogResult.
                //{
                //    if (item.nCdApi == AdicionarConteudoVM.oVideoSelecionado.nCdApi)
                //    {
                //        item.lstSerieAlias = frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias;
                //        //AdicionarConteudoViewModel.SelectedVideo.AliasNames = frmConfigConteudo.ConfigurarConteudoVM.Video.AliasNames;
                //        break;
                //    }
                //}
                if (frmConfigConteudo.ConfigurarConteudoVM.bFlAcaoRemover)
                {
                    AdicionarConteudoVM.ActionClose(true);
                }
            }
        }

        public class CommandSalvarConteudo : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is AdicionarConteudoViewModel && (parameter as AdicionarConteudoViewModel).oVideoSelecionado != null && ((parameter as AdicionarConteudoViewModel).oVideoSelecionado.nIdEstado == Helpers.Enums.Estado.Completo || (parameter as AdicionarConteudoViewModel).oVideoSelecionado.nIdEstado == Helpers.Enums.Estado.CompletoSemForeignKeys);
            }

            public async void Execute(object parameter)
            {
                AdicionarConteudoViewModel adicionarConteudoVM = parameter as AdicionarConteudoViewModel;

                if (adicionarConteudoVM.oVideoSelecionado == null || adicionarConteudoVM.oVideoSelecionado.sDsPasta == null)
                {
                    Helper.MostrarMensagem("Favor preencher todos os campos antes de salvar.", Enums.eTipoMensagem.Alerta);
                    return;
                }
                else if (adicionarConteudoVM.bProcurarConteudo)
                {
                    adicionarConteudoVM.ActionClose(true);
                }
                else
                {
                    SeriesService serieService = App.Container.Resolve<SeriesService>();
                    switch (adicionarConteudoVM.nIdTipoConteudo)
                    {
                        case Enums.TipoConteudo.Filme:
                            break;

                        case Enums.TipoConteudo.Série:
                        case Enums.TipoConteudo.Anime:
                            {
                                Serie serie = null;

                                serie = (Serie)adicionarConteudoVM.oVideoSelecionado;

                                serie.lstSerieAlias = Helper.PopularCampoSerieAlias(serie);

                                if (serie.nCdVideo > 0)
                                {
                                    try
                                    {
                                        serie.SetEstadoEpisodio();
                                        await serieService.UpdateAsync(serie);
                                    }
                                    catch (Exception ex)
                                    {
                                        new MediaManagerException(ex).TratarException($"Ocorreu um erro ao atualizar a série \"{serie.sDsTitulo}\".");
                                        adicionarConteudoVM.ActionClose(false);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        serie.SetEstadoEpisodio();
                                        await serieService.AdicionarAsync(serie);
                                    }
                                    catch (Exception ex)
                                    {
                                        new MediaManagerException(ex).TratarException($"Ocorreu um erro ao incluir a série \"{serie.sDsTitulo}\".");
                                        adicionarConteudoVM.ActionClose(false);
                                    }
                                }
                                break;
                            }

                        case Enums.TipoConteudo.AnimeFilmeSérie:
                            {
                                Serie anime = null;

                                if (adicionarConteudoVM.oVideoSelecionado is Serie)
                                    anime = (Serie)adicionarConteudoVM.oVideoSelecionado;

                                if (adicionarConteudoVM.oVideoSelecionado.nCdVideo > 0)
                                {
                                    try
                                    {
                                        anime.SetEstadoEpisodio();
                                        await serieService.UpdateAsync(anime);
                                    }
                                    catch (Exception ex)
                                    {
                                        new MediaManagerException(ex).TratarException("Ocorreu um erro ao atualizar o conteúdo.");
                                        adicionarConteudoVM.ActionClose(false);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        anime.SetEstadoEpisodio();
                                        await serieService.AdicionarAsync(anime);
                                    }
                                    catch (Exception ex)
                                    {
                                        new MediaManagerException(ex).TratarException("Ocorreu um erro ao incluir o conteúdo.");
                                        adicionarConteudoVM.ActionClose(false);
                                    }
                                }
                                break;
                            }
                        default:
                            break;
                    }
                    adicionarConteudoVM.ActionClose(true);
                }
            }
        }
    }
}