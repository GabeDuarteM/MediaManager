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
            ListaFeedsViewModel feedVM = new ListaFeedsViewModel(new List<Feed>());
            feedVM.lstFeeds = new List<Feed>();

            feedVM.CommandAumentarPrioridadeFeed = new Commands.ListaFeedsCommands.CommandAumentarPrioridadeFeed();
            feedVM.CommandAumentarPrioridadeFeed.Execute(feedVM);
        }
    }
}