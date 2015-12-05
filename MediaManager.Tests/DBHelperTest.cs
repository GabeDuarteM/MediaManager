using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MediaManager.Helpers;
using MediaManager.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MediaManager.Tests
{
    [TestClass]
    public class DBHelperTest
    {
        [TestMethod]
        public void AdicionarSeriesTeste()
        {
            var mockSet = new Mock<DbSet<Serie>>();
            var mockContext = new Mock<Context>();

            mockContext.Setup(x => x.Serie).Returns(mockSet.Object);

            DBHelper.Context = mockContext.Object;

            var DbHelper = new DBHelper();

            List<Serie> lstSeries = APIRequests.GetSeries("Arrow");
            DbHelper.AddSerie(lstSeries[0]);

            mockSet.Verify(x => x.Add(It.IsAny<Serie>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}