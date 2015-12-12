using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaManager.Helpers;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class FeedCommands
    {
        public class CommandSalvar : ICommand
        {
            public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }

            public bool CanExecute(object parameter)
            {
                return parameter is FeedViewModel;
            }

            public void Execute(object parameter)
            {
                var FeedVM = parameter as FeedViewModel;

                if (FeedVM.oFeed.IsValid & FeedVM.IsValid)
                {
                    DBHelper db = new DBHelper();

                    if (FeedVM.bAnime)
                    {
                        FeedVM.oFeed.nIdTipoConteudo = Enums.TipoConteudo.Anime;
                        db.AddFeed(FeedVM.oFeed);
                    }

                    if (FeedVM.bSerie)
                    {
                        FeedVM.oFeed.nIdTipoConteudo = Enums.TipoConteudo.Série;
                        db.AddFeed(FeedVM.oFeed);
                    }

                    if (FeedVM.bFilme)
                    {
                        FeedVM.oFeed.nIdTipoConteudo = Enums.TipoConteudo.Filme;
                        db.AddFeed(FeedVM.oFeed);
                    }
                    FeedVM.ActionFechar(true);
                }
            }
        }
    }
}