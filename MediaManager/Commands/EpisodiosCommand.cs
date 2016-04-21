// Developed by: Gabriel Duarte
// 
// Created at: 01/11/2015 14:08

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Services;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class EpisodiosCommand
    {
        public class CommandSelecionarTodos : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is EpisodiosViewModel;
            }

            public void Execute(object parameter)
            {
                var episodiosVM = parameter as EpisodiosViewModel;
                if (episodiosVM.bFlSelecionarTodos == true)
                {
                    foreach (Episodio episodio in episodiosVM.lstEpisodios)
                    {
                        episodio.bFlSelecionado = true;
                    }
                }
                else
                {
                    episodiosVM.bFlSelecionarTodos = false;
                    foreach (Episodio episodio in episodiosVM.lstEpisodios)
                    {
                        episodio.bFlSelecionado = false;
                    }
                }
            }
        }

        public class CommandFechar : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is EpisodiosViewModel;
            }

            public void Execute(object parameter)
            {
                var episodiosVM = parameter as EpisodiosViewModel;
                episodiosVM.ActionFechar();
            }
        }

        public class CommandSalvar : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is EpisodiosViewModel;
            }

            public void Execute(object parameter)
            {
                var episodiosVM = parameter as EpisodiosViewModel;

                var episodiosService = App.Container.Resolve<EpisodiosService>();

                if (episodiosVM.nIdEstadoEpisodioSelecionado != Enums.EstadoEpisodio.Selecione)
                {
                    List<Episodio> lstEpisodiosModificados =
                        episodiosVM.lstEpisodios.Where(x => x.bFlSelecionado).ToList();
                    if (lstEpisodiosModificados.Count > 0)
                    {
                        foreach (Episodio item in lstEpisodiosModificados)
                        {
                            item.nIdEstadoEpisodio = episodiosVM.nIdEstadoEpisodioSelecionado;
                        }

                        episodiosService.UpdateEstadoEpisodio(lstEpisodiosModificados.ToArray());
                    }
                }
            }
        }

        public class CommandIsSelected : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is EpisodiosViewModel;
            }

            public void Execute(object parameter)
            {
                var episodiosVM = parameter as EpisodiosViewModel;
                int episodiosSelecionadosCount = episodiosVM.lstEpisodios.Where(x => x.bFlSelecionado).Count();
                if (episodiosSelecionadosCount == episodiosVM.lstEpisodios.Count)
                {
                    episodiosVM.bFlSelecionarTodos = true;
                }
                else if (episodiosSelecionadosCount == 0)
                {
                    episodiosVM.bFlSelecionarTodos = false;
                }
                else if (episodiosSelecionadosCount > 0)
                {
                    episodiosVM.bFlSelecionarTodos = null;
                }
            }
        }
    }
}
