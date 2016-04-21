// Developed by: Gabriel Duarte
// 
// Created at: 01/11/2015 01:25

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Autofac;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Localizacao;
using MediaManager.Model;
using MediaManager.Services;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class AdicionarConteudoCommands
    {
        public class CommandAbrirEpisodios : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                var adicionarConteudoVm = parameter as AdicionarConteudoViewModel;
                return parameter is AdicionarConteudoViewModel &&
                       (adicionarConteudoVm.nIdTipoConteudo == Enums.TipoConteudo.Anime ||
                        adicionarConteudoVm.nIdTipoConteudo == Enums.TipoConteudo.Série)
                       && adicionarConteudoVm.oVideoSelecionado != null &&
                       (adicionarConteudoVm.oVideoSelecionado.nIdEstado == Enums.Estado.Completo ||
                        adicionarConteudoVm.oVideoSelecionado.nIdEstado == Enums.Estado.CompletoSemForeignKeys);
            }

            public void Execute(object parameter)
            {
                var adicionarConteudoVm = parameter as AdicionarConteudoViewModel;

                var frmEpisodios = new frmEpisodios(adicionarConteudoVm.oVideoSelecionado);
                frmEpisodios.Show();
            }
        }

        public class CommandConfigurarConteudo : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return (parameter as AdicionarConteudoViewModel)?.oVideoSelecionado != null
                       && (((AdicionarConteudoViewModel) parameter).oVideoSelecionado.nIdEstado == Enums.Estado.Completo
                           ||
                           ((AdicionarConteudoViewModel) parameter).oVideoSelecionado.nIdEstado ==
                           Enums.Estado.CompletoSemForeignKeys);
            }

            public void Execute(object parameter)
            {
                var adicionarConteudoVm = parameter as AdicionarConteudoViewModel;

                var frmConfigConteudo = new frmConfigConteudo(adicionarConteudoVm.oVideoSelecionado);
                frmConfigConteudo.ShowDialog();
                if (frmConfigConteudo.DialogResult == true && adicionarConteudoVm.oVideoSelecionado.nCdVideo > 0)
                {
                    //AdicionarConteudoVM.lstResultPesquisa.First(x => x.nCdApi == AdicionarConteudoVM.oVideoSelecionado.nCdApi).lstSerieAlias = frmConfigConteudo.ConfigurarConteudoVM.oVideo.lstSerieAlias;
                    var serieAliasService = App.Container.Resolve<SerieAliasService>();

                    List<SerieAlias> lstSerieAliasOriginal =
                        serieAliasService.GetLista(adicionarConteudoVm.oVideoSelecionado);

                    foreach (
                        SerieAlias item in
                            adicionarConteudoVm.oVideoSelecionado.lstSerieAlias.Where(x => x.nCdAlias == 0))
                    {
                        serieAliasService.Adicionar(item);
                    }

                    serieAliasService.Remover(lstSerieAliasOriginal.Where(x => !adicionarConteudoVm.oVideoSelecionado.lstSerieAlias.Contains(x)).ToArray());
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
                    adicionarConteudoVm.ActionClose(true);
                }
            }
        }

        public class CommandSalvarConteudo : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return (parameter as AdicionarConteudoViewModel)?.oVideoSelecionado != null
                       && (((AdicionarConteudoViewModel) parameter).oVideoSelecionado.nIdEstado == Enums.Estado.Completo
                           ||
                           ((AdicionarConteudoViewModel) parameter).oVideoSelecionado.nIdEstado ==
                           Enums.Estado.CompletoSemForeignKeys);
            }

            public async void Execute(object parameter)
            {
                var adicionarConteudoVm = parameter as AdicionarConteudoViewModel;

                if (adicionarConteudoVm.oVideoSelecionado?.sDsPasta == null)
                {
                    Helper.MostrarMensagem(Mensagens.Favor_preencher_todos_os_campos_antes_de_salvar,
                                           Enums.eTipoMensagem.Alerta);
                    return;
                }

                if (adicionarConteudoVm.bProcurarConteudo)
                {
                    adicionarConteudoVm.ActionClose(true);
                }
                else
                {
                    var serieService = App.Container.Resolve<SeriesService>();
                    switch (adicionarConteudoVm.nIdTipoConteudo)
                    {
                        case Enums.TipoConteudo.Filme:
                            break;

                        case Enums.TipoConteudo.Série:
                        case Enums.TipoConteudo.Anime:
                        {
                            var serie = (Serie) adicionarConteudoVm.oVideoSelecionado;

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
                                    new MediaManagerException(ex).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_atualizar_1_0_, serie.sDsTitulo, serie.nIdTipoConteudo.GetDescricao().ToLower()));
                                    adicionarConteudoVm.ActionClose(false);
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
                                    new MediaManagerException(ex).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_adicionar_1_0_, serie.sDsTitulo, serie.nIdTipoConteudo.GetDescricao().ToLower()));
                                    adicionarConteudoVm.ActionClose(false);
                                }
                            }
                            break;
                        }

                        case Enums.TipoConteudo.AnimeFilmeSérie:
                        {
                            Serie anime = null;

                            if (adicionarConteudoVm.oVideoSelecionado is Serie)
                            {
                                anime = (Serie) adicionarConteudoVm.oVideoSelecionado;
                            }

                            if (adicionarConteudoVm.oVideoSelecionado.nCdVideo > 0)
                            {
                                try
                                {
                                    anime.SetEstadoEpisodio();
                                    await serieService.UpdateAsync(anime);
                                }
                                catch (Exception ex)
                                {
                                    new MediaManagerException(ex).TratarException(Mensagens.Ocorreu_um_erro_ao_atualizar_o_conteúdo);
                                    adicionarConteudoVm.ActionClose(false);
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
                                    new MediaManagerException(ex).TratarException(Mensagens.Ocorreu_um_erro_ao_adicionar_o_conteúdo);
                                    adicionarConteudoVm.ActionClose(false);
                                }
                            }
                            break;
                        }
                        case Enums.TipoConteudo.Selecione:
                            break;
                        case Enums.TipoConteudo.Episódio:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    adicionarConteudoVm.ActionClose(true);
                }
            }
        }

        public class CommandEscolherPasta : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return (parameter as AdicionarConteudoViewModel)?.oVideoSelecionado != null && ((parameter as AdicionarConteudoViewModel).oVideoSelecionado.nIdEstado == Enums.Estado.Completo
                                                                                                || ((AdicionarConteudoViewModel) parameter).oVideoSelecionado.nIdEstado == Enums.Estado.CompletoSemForeignKeys);
            }

            public void Execute(object parameter)
            {
                var adicionarConteudoVm = parameter as AdicionarConteudoViewModel;

                var folderDialog = new Ookii.Dialogs.VistaFolderBrowserDialog
                {
                    SelectedPath = adicionarConteudoVm?.oVideoSelecionado.sDsPasta
                };

                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    adicionarConteudoVm.oVideoSelecionado.sDsPasta = folderDialog.SelectedPath;
                }
            }
        }
    }
}
