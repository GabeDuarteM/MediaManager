using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class EpisodiosCommand
    {
        public class SelecionarTodosCommand : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                //if (parameter is EpisodiosViewModel)
                //    return true;
                /*else*/
                return false;
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
                    foreach (var episodio in episodiosVM.Episodios)
                    {
                        episodio.IsSelected = false;
                    }
                }
            }
        }
    }
}