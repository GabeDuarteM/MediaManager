using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaManager.Forms;
using MediaManager.Model;
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
                if (parameter is AdicionarConteudoViewModel &&
                    ((parameter as AdicionarConteudoViewModel).nIdTipoConteudo == Helpers.Enums.TipoConteudo.Anime
                    || (parameter as AdicionarConteudoViewModel).nIdTipoConteudo == Helpers.Enums.TipoConteudo.Série))
                {
                    return true;
                }
                else return false;
            }

            public void Execute(object parameter)
            {
                AdicionarConteudoViewModel AdicionarConteudoVM = parameter as AdicionarConteudoViewModel;

                frmEpisodios frmEpisodios = new frmEpisodios(AdicionarConteudoVM.oVideoSelecionado);
                frmEpisodios.Show();
            }
        }
    }
}