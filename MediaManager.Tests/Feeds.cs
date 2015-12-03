using System;
using System.Collections.Generic;
using MediaManager.Model;
using MediaManager.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaManager.Tests
{
    [TestClass]
    public class Feeds
    {
        [TestMethod]
        public void FeedsPrioridades()
        {
            Feed feed = new Feed() { nCdFeed = 1, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 1 };
            Feed feed2 = new Feed() { nCdFeed = 2, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 2 };
            Feed feed3 = new Feed() { nCdFeed = 3, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 3 };
            Feed feed4 = new Feed() { nCdFeed = 4, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 4 };
            Feed feed5 = new Feed() { nCdFeed = 5, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 5 };
            Feed feed6 = new Feed() { nCdFeed = 6, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 1 };
            Feed feed7 = new Feed() { nCdFeed = 7, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 2 };
            Feed feed8 = new Feed() { nCdFeed = 8, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 3 };
            Feed feed9 = new Feed() { nCdFeed = 9, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 4 };
            Feed feed10 = new Feed() { nCdFeed = 10, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 5 };

            FeedsViewModel feedVM = new FeedsViewModel(new List<Feed>());
            feedVM.lstFeeds = new List<Feed>();
            feedVM.lstFeeds.Add(feed);
            feedVM.lstFeeds.Add(feed2);
            feedVM.lstFeeds.Add(feed3);
            feedVM.lstFeeds.Add(feed4);
            feedVM.lstFeeds.Add(feed5);
            feedVM.lstFeeds.Add(feed6);
            feedVM.lstFeeds.Add(feed7);
            feedVM.lstFeeds.Add(feed8);
            feedVM.lstFeeds.Add(feed9);
            feedVM.lstFeeds.Add(feed10);

            feedVM.CommandAumentarPrioridadeFeed = new Commands.FeedsCommands.CommandAumentarPrioridadeFeed();
            feedVM.CommandAumentarPrioridadeFeed.Execute(feedVM);
        }
    }
}