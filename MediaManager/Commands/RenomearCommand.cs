// Developed by: Gabriel Duarte
// 
// Created at: 22/11/2015 22:08

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Services;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class RenomearCommands
    {
        public class CommandRenomear : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is RenomearViewModel
                       && (parameter as RenomearViewModel).lstEpisodios != null
                       && (parameter as RenomearViewModel).lstEpisodios.Count > 0;
            }

            public void Execute(object parameter)
            {
                try
                {
                    var renomearVM = parameter as RenomearViewModel;

                    foreach (Episodio item in renomearVM.lstEpisodios)
                    {
                        try
                        {
                            var episodiosService = App.Container.Resolve<EpisodiosService>();
                            var bCancelarOperacao = false;

                            if (item.bFlSelecionado)
                            {
                                Helper.LogMessage("O arquivo \"" + item.sDsFilepathOriginal + "\" será copiado para \"" +
                                                  item.sDsFilepath + "\"");
                                Helper.LogMessage("Método de processamento: " +
                                                  ((Enums.MetodoDeProcessamento)
                                                   Properties.Settings.Default.pref_MetodoDeProcessamento).ToString());

                                // Adiciona o FilenameRenamed para quando houver pasta no nome (Ex. "Season 04\\Arrow - 4x05 - Haunted.mkv")
                                if (!Directory.Exists(Path.GetDirectoryName(item.sDsFilepath)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(item.sDsFilepath));
                                    Helper.LogMessage("Diretório \"" + Path.GetDirectoryName(item.sDsFilepath) +
                                                      "\" criado.");
                                }

                                if (File.Exists(item.sDsFilepath))
                                {
                                    if (renomearVM.bFlSilencioso ||
                                        (Helper.MostrarMensagem(
                                                                "O arquivo \"" + item.sDsFilepath +
                                                                "\" já existe.\r\nDeseja sobrescrevê-lo pelo arquivo \"" +
                                                                item.sDsFilepathOriginal + "\"?",
                                                                Enums.eTipoMensagem.QuestionamentoSimNao) ==
                                         MessageBoxResult.Yes))
                                    {
                                        Helper.LogMessage("O arquivo \"" + item.sDsFilepath +
                                                          "\" já existe e será substituído.");
                                        File.Delete(item.sDsFilepath);
                                    }
                                    else
                                    {
                                        Helper.LogMessage("O arquivo \"" + item.sDsFilepath + "\" não foi renomeado.");
                                        bCancelarOperacao = true;
                                    }
                                }
                                if (!bCancelarOperacao && Helper.RealizarPosProcessamento(item))
                                {
                                    item.bFlRenomeado = true;
                                    item.nIdEstadoEpisodio = Enums.EstadoEpisodio.Baixado;
                                    episodiosService.UpdateEpisodioRenomeado(item);
                                    Helper.LogMessage("O arquivo \"" + item.sDsFilepath +
                                                      "\" foi renomeado com sucesso.");
                                }
                                else if (!bCancelarOperacao)
                                {
                                    new MediaManagerException(
                                        new Exception(
                                            $"Código: {Marshal.GetLastWin32Error()}{Environment.NewLine}Arquivo: {item.sDsFilepathOriginal}."))
                                        .TratarException("Ocorreu um erro no processo de " +
                                                         ((Enums.MetodoDeProcessamento)
                                                          Properties.Settings.Default.pref_MetodoDeProcessamento)
                                                             .ToString());
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            new MediaManagerException(e).TratarException(
                                                                         $"Ocorreu um erro ao renomear o episódio \"{item.sDsFilepathOriginal}\".");
                        }
                    }

                    if (!renomearVM.bFlSilencioso &&
                        renomearVM.lstEpisodios.Where(x => x.bFlSelecionado).Any(x => !x.bFlRenomeado))
                    {
                        Helper.MostrarMensagem(
                                               "Alguns arquivos não foram renomeados com sucesso, para mais detalhes verifique o log.",
                                               Enums.eTipoMensagem.Erro);
                    }
                    else if (!renomearVM.bFlSilencioso)
                    {
                        Helper.MostrarMensagem("Arquivos renomeados com sucesso.", Enums.eTipoMensagem.Informativa);
                    }

                    if (renomearVM.ActionFechar != null)
                    {
                        renomearVM.ActionFechar();
                    }
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException("Ocorreu um erro ao renomear o episódio");
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
                return parameter is RenomearViewModel;
            }

            public void Execute(object parameter)
            {
                var renomearVM = parameter as RenomearViewModel;
                int episodiosSelecionadosCount = renomearVM.lstEpisodios.Where(x => x.bFlSelecionado).Count();
                if (episodiosSelecionadosCount == renomearVM.lstEpisodios.Count && renomearVM.lstEpisodios.Count > 0)
                {
                    renomearVM.bFlSelecionarTodos = true;
                }
                else if (episodiosSelecionadosCount == 0)
                {
                    renomearVM.bFlSelecionarTodos = false;
                }
                else if (episodiosSelecionadosCount > 0)
                {
                    renomearVM.bFlSelecionarTodos = null;
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
                return parameter is RenomearViewModel;
            }

            public void Execute(object parameter)
            {
                var renomearVM = parameter as RenomearViewModel;
                if (renomearVM.bFlSelecionarTodos == true)
                {
                    renomearVM.lstEpisodios.ToList().ForEach(x => x.bFlSelecionado = true);
                }
                else
                {
                    renomearVM.bFlSelecionarTodos = false;
                    renomearVM.lstEpisodios.ToList().ForEach(x => x.bFlSelecionado = false);
                }
            }
        }
    }
}
