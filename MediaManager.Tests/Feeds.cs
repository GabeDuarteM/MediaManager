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
            Feed feed = new Feed() { nCdFeed = 1, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.ContentType.Anime, nNrPrioridade = 1 };
            Feed feed2 = new Feed() { nCdFeed = 2, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.ContentType.Anime, nNrPrioridade = 2 };
            Feed feed3 = new Feed() { nCdFeed = 3, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.ContentType.Anime, nNrPrioridade = 3 };
            Feed feed4 = new Feed() { nCdFeed = 4, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.ContentType.Anime, nNrPrioridade = 4 };
            Feed feed5 = new Feed() { nCdFeed = 5, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.ContentType.Anime, nNrPrioridade = 5 };
            Feed feed6 = new Feed() { nCdFeed = 6, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.ContentType.Série, nNrPrioridade = 1 };
            Feed feed7 = new Feed() { nCdFeed = 7, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.ContentType.Série, nNrPrioridade = 2 };
            Feed feed8 = new Feed() { nCdFeed = 8, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.ContentType.Série, nNrPrioridade = 3 };
            Feed feed9 = new Feed() { nCdFeed = 9, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.ContentType.Série, nNrPrioridade = 4 };
            Feed feed10 = new Feed() { nCdFeed = 10, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.ContentType.Série, nNrPrioridade = 5 };

            FeedsViewModel feedVM = new FeedsViewModel(new List<Feed>());
            feedVM.Feeds = new List<Feed>();
            feedVM.Feeds.Add(feed);
            feedVM.Feeds.Add(feed2);
            feedVM.Feeds.Add(feed3);
            feedVM.Feeds.Add(feed4);
            feedVM.Feeds.Add(feed5);
            feedVM.Feeds.Add(feed6);
            feedVM.Feeds.Add(feed7);
            feedVM.Feeds.Add(feed8);
            feedVM.Feeds.Add(feed9);
            feedVM.Feeds.Add(feed10);

            feedVM.CommandAumentarPrioridadeFeed = new Commands.FeedsCommands.CommandAumentarPrioridadeFeed();
            feedVM.CommandAumentarPrioridadeFeed.Execute(feedVM);
        }
    }
}