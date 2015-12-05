using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaManager.Helpers;
using MediaManager.Model;
using Moq;

namespace MediaManager.Tests
{
    public class TesteHelper
    {
        public static Mock<Context> GerarMassaDeDados()
        {
            List<int> lstIntIdsSeries = new List<int>() { 257655, 273181, 205281, 121361, 274431, 281485, 263365, 281662, 268592, 80379, 279121, 258744, 153021,
                                                          264492, 267440, 289679, 88031, 295068, 289882, 114801, 79151, 85249, 295224, 249827, 79824, 278155, 81797,
                                                          293088, 259640, 251085, 281249 };
            var lstSeries = new List<Serie>();
            var lstEpisodios = new List<Episodio>();
            var lstFeeds = new List<Feed>()
            {
                new Feed() { nCdFeed = 1, bFlSelecionado = true, nIdTipoConteudo = Enums.TipoConteudo.Anime, nNrPrioridade = 1 },
                new Feed() { nCdFeed = 2, bFlSelecionado = false, nIdTipoConteudo = Enums.TipoConteudo.Anime, nNrPrioridade = 2 },
                new Feed() { nCdFeed = 3, bFlSelecionado = true, nIdTipoConteudo = Enums.TipoConteudo.Anime, nNrPrioridade = 3 },
                new Feed() { nCdFeed = 4, bFlSelecionado = false, nIdTipoConteudo = Enums.TipoConteudo.Anime, nNrPrioridade = 4 },
                new Feed() { nCdFeed = 5, bFlSelecionado = true, nIdTipoConteudo = Enums.TipoConteudo.Anime, nNrPrioridade = 5 },
                new Feed() { nCdFeed = 6, bFlSelecionado = false, nIdTipoConteudo = Enums.TipoConteudo.Série, nNrPrioridade = 1 },
                new Feed() { nCdFeed = 7, bFlSelecionado = true, nIdTipoConteudo = Enums.TipoConteudo.Série, nNrPrioridade = 2 },
                new Feed() { nCdFeed = 8, bFlSelecionado = false, nIdTipoConteudo = Enums.TipoConteudo.Série, nNrPrioridade = 3 },
                new Feed() { nCdFeed = 9, bFlSelecionado = true, nIdTipoConteudo = Enums.TipoConteudo.Série, nNrPrioridade = 4 },
                new Feed() { nCdFeed = 10, bFlSelecionado = false, nIdTipoConteudo = Enums.TipoConteudo.Série, nNrPrioridade = 5 }
            };
            foreach (var item in lstIntIdsSeries)
            {
                var serie = APIRequests.GetSerieInfoAsync(item, "en").Result;
                serie.nCdVideo = (lstSeries.Count > 0) ? lstSeries.Last().nCdVideo + 1 : 1;
                serie.nIdTipoConteudo = Enums.TipoConteudo.Série;
                serie.SetDefaultFolderPath();

                lstSeries.Add(serie);
                foreach (var episodio in serie.lstEpisodios)
                {
                    episodio.nCdEpisodio = (lstEpisodios.Count > 0) ? lstEpisodios.Last().nCdEpisodio : 1;
                    episodio.nCdVideo = serie.nCdVideo;
                    episodio.oSerie = serie;
                    lstEpisodios.Add(episodio);
                }
            }

            var lstSerieAlias = new List<SerieAlias>()
            {
                new SerieAlias() { nCdAlias = 1, sDsAlias = "Agent Carter", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 281485).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 281485) },
                new SerieAlias() { nCdAlias = 2, sDsAlias = "Agents of S.H.I.E.L.D.", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 263365).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 263365) },
                new SerieAlias() { nCdAlias = 3, sDsAlias = "Daredevil", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 281662).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 281662) },
                new SerieAlias() { nCdAlias = 4, sDsAlias = "Mastermind (2012)", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 258744).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 258744) },
                new SerieAlias() { nCdAlias = 5, sDsAlias = "Shingeki no Kyojin", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 267440).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 267440) },
                new SerieAlias() { nCdAlias = 6, sDsAlias = "Shingeki no Kyojin: No Regrets", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 267440).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 267440) },
                new SerieAlias() { nCdAlias = 7, sDsAlias = "Shingeki no Kyojin: Birth of Levi", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 267440).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 267440) },
                new SerieAlias() { nCdAlias = 8, sDsAlias = "Attack on Titan: No Regrets", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 267440).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 267440) },
                new SerieAlias() { nCdAlias = 9, sDsAlias = "Attack on Titan: Birth of Levi", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 267440).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 267440) },
                new SerieAlias() { nCdAlias = 10, sDsAlias = "Dragon Ball Z Kai", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 88031).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 88031) },
                new SerieAlias() { nCdAlias = 11, sDsAlias = "Dragonball Kai", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 88031).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 88031) },
                new SerieAlias() { nCdAlias = 12, sDsAlias = "Dragonball Z Kai", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 88031).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 88031) },
                new SerieAlias() { nCdAlias = 13, sDsAlias = "Dragon Ball Chou", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 295068).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 295068) },
                new SerieAlias() { nCdAlias = 14, sDsAlias = "Fairy Tail S2", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 114801).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 114801) },
                new SerieAlias() { nCdAlias = 15, sDsAlias = "Fate Stay Night", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 79151).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 79151) },
                new SerieAlias() { nCdAlias = 16, sDsAlias = "Full Metal Alchemist Brotherhood", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 85249).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 85249) },
                new SerieAlias() { nCdAlias = 17, sDsAlias = "Full Metal Alchemist: Brotherhood", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 85249).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 85249) },
                new SerieAlias() { nCdAlias = 18, sDsAlias = "Hagane no Renkinjutsushi (2009)", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 85249).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 85249) },
                new SerieAlias() { nCdAlias = 19, sDsAlias = "God Eater Burst", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 295224).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 295224) },
                new SerieAlias() { nCdAlias = 20, sDsAlias = "Naruto Shippūden", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 79824).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 79824) },
                new SerieAlias() { nCdAlias = 21, sDsAlias = "Naruto Shippuuden", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 79824).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 79824) },
                new SerieAlias() { nCdAlias = 22, sDsAlias = "Wanpanman", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 293088).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 293088) },
                new SerieAlias() { nCdAlias = 23, sDsAlias = "Sword Art Online II", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 259640).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 259640) },
                new SerieAlias() { nCdAlias = 24, sDsAlias = "The Last Airbender The Legend of Korra", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 251085).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 251085) },
                new SerieAlias() { nCdAlias = 25, sDsAlias = "Avatar: The Legend of Korra", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 251085).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 251085) },
                new SerieAlias() { nCdAlias = 26, sDsAlias = "Legend of Korra", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 251085).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 251085) },
                new SerieAlias() { nCdAlias = 27, sDsAlias = "La Leggenda Di Korra", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 251085).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 251085) },
                new SerieAlias() { nCdAlias = 28, sDsAlias = "Tokyo Ghoul Root A", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 281249).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 281249) },
                new SerieAlias() { nCdAlias = 29, sDsAlias = "Tokyo Ghoul", nNrEpisodio = 1, nCdVideo = lstSeries.First(x => x.nCdApi == 281249).nCdVideo, nNrTemporada = 1, oSerie = lstSeries.First(x => x.nCdApi == 281249) }
            };

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
            mockSetSerieAlias.As<IQueryable<SerieAlias>>().Setup(m => m.Provider).Returns(lstSerieAlias.AsQueryable().Provider);
            mockSetSerieAlias.As<IQueryable<SerieAlias>>().Setup(m => m.Expression).Returns(lstSerieAlias.AsQueryable().Expression);
            mockSetSerieAlias.As<IQueryable<SerieAlias>>().Setup(m => m.ElementType).Returns(lstSerieAlias.AsQueryable().ElementType);
            mockSetSerieAlias.As<IQueryable<SerieAlias>>().Setup(m => m.GetEnumerator()).Returns(lstSerieAlias.AsQueryable().GetEnumerator());

            var mockContext = new Mock<Context>();
            mockContext.Setup(x => x.Serie).Returns(mockSetSerie.Object);
            mockContext.Setup(x => x.Episodio).Returns(mockSetEpisodio.Object);
            mockContext.Setup(x => x.Feed).Returns(mockSetFeed.Object);
            mockContext.Setup(x => x.SerieAlias).Returns(mockSetSerieAlias.Object);

            DBHelper.Context = mockContext.Object;

            var DbHelper = new DBHelper();

            foreach (var item in lstSeries)
            {
                DbHelper.AddSerie(item);
                foreach (var episodio in lstEpisodios.Where(x => x.nCdVideoAPI == item.nCdApi))
                {
                    episodio.oSerie = item;
                    DbHelper.AddEpisodio(episodio);
                }
            }

            foreach (var item in lstFeeds)
            {
                DbHelper.AddFeed(item);
            }
            mockContext.Verify(x => x.SaveChanges());

            return mockContext;
        }
    }
}