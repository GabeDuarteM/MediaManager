// Developed by: Gabriel Duarte
// 
// Created at: 11/12/2015 20:23
// Last update: 19/04/2016 02:57

using System;
using System.Windows.Input;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Services;
using MediaManager.ViewModel;

namespace MediaManager.Commands
{
    public class FeedCommands
    {
        public class CommandSalvar : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return parameter is FeedViewModel;
            }

            public void Execute(object parameter)
            {
                var FeedVM = parameter as FeedViewModel;

                if (FeedVM.oFeed.IsValid & FeedVM.IsValid)
                {
                    var feedsService = App.Container.Resolve<FeedsService>();
                    if (FeedVM.bAnime)
                    {
                        FeedVM.oFeed.nIdTipoConteudo = Enums.TipoConteudo.Anime;
                        feedsService.Adicionar(FeedVM.oFeed);
                    }

                    if (FeedVM.bSerie)
                    {
                        FeedVM.oFeed.nIdTipoConteudo = Enums.TipoConteudo.Série;
                        feedsService.Adicionar(FeedVM.oFeed);
                    }

                    if (FeedVM.bFilme)
                    {
                        FeedVM.oFeed.nIdTipoConteudo = Enums.TipoConteudo.Filme;
                        feedsService.Adicionar(FeedVM.oFeed);
                    }
                    FeedVM.ActionFechar(true);
                }
            }
        }
    }
}
