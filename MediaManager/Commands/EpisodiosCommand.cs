using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class EpisodiosCommand
    {
        public class CommandSelecionarTodos : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is EpisodiosViewModel;
            }

            public void Execute(object parameter)
            {
                EpisodiosViewModel episodiosVM = parameter as EpisodiosViewModel;
                if (episodiosVM.SelecionarTodos == true)
                {
                    foreach (var episodio in episodiosVM.Episodios)
                    {
                        episodio.IsSelected = true;
                    }
                }
                else
                {
                    episodiosVM.SelecionarTodos = false;
                    foreach (var episodio in episodiosVM.Episodios)
                    {
                        episodio.IsSelected = false;
                    }
                }
            }
        }

        public class CommandFechar : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is EpisodiosViewModel;
            }

            public void Execute(object parameter)
            {
                EpisodiosViewModel episodiosVM = parameter as EpisodiosViewModel;
                episodiosVM.ActionFechar();
            }
        }

        public class CommandSalvar : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is EpisodiosViewModel;
            }

            public void Execute(object parameter)
            {
                EpisodiosViewModel episodiosVM = parameter as EpisodiosViewModel;
                if (episodiosVM.EstadoEpisodioSelecionado != Helpers.Enums.EstadoEpisodio.Selecione)
                {
                    List<Episode> listaEpisodiosModificados = episodiosVM.Episodios.Where(x => x.IsSelected).ToList();
                    if (listaEpisodiosModificados.Count > 0)
                    {
                        foreach (var item in listaEpisodiosModificados)
                        {
                            item.EstadoEpisodio = episodiosVM.EstadoEpisodioSelecionado;
                        }

                        DBHelper.UpdateListaEpisodios(listaEpisodiosModificados);
                    }
                }
            }
        }

        public class CommandIsSelected : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is EpisodiosViewModel;
            }

            public void Execute(object parameter)
            {
                var episodiosVM = parameter as EpisodiosViewModel;
                int episodiosSelecionadosCount = episodiosVM.Episodios.Where(x => x.IsSelected).Count();
                if (episodiosSelecionadosCount == episodiosVM.Episodios.Count)
                {
                    episodiosVM.SelecionarTodos = true;
                }
                else if (episodiosSelecionadosCount == 0)
                {
                    episodiosVM.SelecionarTodos = false;
                }
                else if (episodiosSelecionadosCount > 0)
                {
                    episodiosVM.SelecionarTodos = null;
                }
            }
        }
    }
}