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

            var DBHelper = new DBHelper(mockContext.Object);

            List<Serie> lstSeries = APIRequests.GetSeries("Arrow");
            DBHelper.AddSerie(lstSeries[0]);

            mockSet.Verify(x => x.Add(It.IsAny<Serie>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public Mock<Context> GerarMassaDeDados()
        {
            List<int> lstIntIdsSeries = new List<int>() { 257655, 273181, 274431, 281485, 263365, 80379, 279121, 258744, 153021, 264492, 295068, 114801, 295224, 79824, 278155, 81797, 293088, 251085, 281249 };
            var lstSeries = new List<Serie>();
            var lstEpisodios = new List<Episodio>();
            List<Feed> lstFeeds = new List<Feed>() { new Feed() { nCdFeed = 1, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 1 },
                                                     new Feed() { nCdFeed = 2, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 2 },
                                                     new Feed() { nCdFeed = 3, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 3 },
                                                     new Feed() { nCdFeed = 4, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 4 },
                                                     new Feed() { nCdFeed = 5, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime, nNrPrioridade = 5 },
                                                     new Feed() { nCdFeed = 6, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 1 },
                                                     new Feed() { nCdFeed = 7, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 2 },
                                                     new Feed() { nCdFeed = 8, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 3 },
                                                     new Feed() { nCdFeed = 9, bFlSelecionado = true, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 4 },
                                                     new Feed() { nCdFeed = 10, bFlSelecionado = false, nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série, nNrPrioridade = 5 } };

            foreach (var item in lstIntIdsSeries)
            {
                var serie = APIRequests.GetSerieInfoAsync(item, "pt").Result;
                lstSeries.Add(serie);
                foreach (var episodio in serie.lstEpisodios)
                {
                    lstEpisodios.Add(episodio);
                }
            }

            var mockSetSerie = new Mock<DbSet<Serie>>();
            mockSetSerie.As<IQueryable<Serie>>().Setup(m => m.Provider).Returns(lstSeries.AsQueryable().Provider);
            mockSetSerie.As<IQueryable<Serie>>().Setup(m => m.Expression).Returns(lstSeries.AsQueryable().Expression);
            mockSetSerie.As<IQueryable<Serie>>().Setup(m => m.ElementType).Returns(lstSeries.AsQueryable().ElementType);
            mockSetSerie.As<IQueryable<Serie>>().Setup(m => m.GetEnumerator()).Returns(lstSeries.AsQueryable().GetEnumerator());

            var mockSetEpisodio = new Mock<DbSet<Episodio>>();
            mockSetEpisodio.As<IQueryable<Episodio>>().Setup(m => m.Provider).Returns(lstEpisodios.AsQueryable().Provider);
            mockSetEpisodio.As<IQueryable<Episodio>>().Setup(m => m.Expression).Returns(lstEpisodios.AsQueryable().Expression);
            mockSetEpisodio.As<IQueryable<Episodio>>().Setup(m => m.ElementType).Returns(lstEpisodios.AsQueryable().ElementType);
            mockSetEpisodio.As<IQueryable<Episodio>>().Setup(m => m.GetEnumerator()).Returns(lstEpisodios.AsQueryable().GetEnumerator());

            var mockSetFeed = new Mock<DbSet<Feed>>();
            mockSetFeed.As<IQueryable<Feed>>().Setup(m => m.Provider).Returns(lstFeeds.AsQueryable().Provider);
            mockSetFeed.As<IQueryable<Feed>>().Setup(m => m.Expression).Returns(lstFeeds.AsQueryable().Expression);
            mockSetFeed.As<IQueryable<Feed>>().Setup(m => m.ElementType).Returns(lstFeeds.AsQueryable().ElementType);
            mockSetFeed.As<IQueryable<Feed>>().Setup(m => m.GetEnumerator()).Returns(lstFeeds.AsQueryable().GetEnumerator());
            var mockSetSerieAlias = new Mock<DbSet<SerieAlias>>();

            var mockContext = new Mock<Context>();
            mockContext.Setup(x => x.Serie).Returns(mockSetSerie.Object);
            mockContext.Setup(x => x.Episodio).Returns(mockSetEpisodio.Object);
            mockContext.Setup(x => x.Feed).Returns(mockSetFeed.Object);
            mockContext.Setup(x => x.SerieAlias).Returns(mockSetSerieAlias.Object);

            var DBHelper = new DBHelper(mockContext.Object);

            foreach (var item in lstSeries)
            {
                DBHelper.AddSerie(item);
                foreach (var episodio in lstEpisodios.Where(x => x.nCdVideoAPI == item.nCdApi))
                {
                    episodio.oSerie = item;
                    DBHelper.AddEpisodio(episodio);
                }
            }

            foreach (var item in lstFeeds)
            {
                DBHelper.AddFeed(item);
            }
            mockContext.Verify(x => x.SaveChanges());

            return mockContext;
        }
    }
}