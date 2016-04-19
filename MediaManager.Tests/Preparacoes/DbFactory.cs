// Developed by: Gabriel Duarte
// 
// Created at: 19/04/2016 01:00
// Last update: 19/04/2016 02:47

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Effort;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.Tests.Preparacoes
{
    public static class DbFactory
    {
        private static IList<Episodio> _lstEpisodios;

        private static IList<Serie> _lstSeries;

        private static IList<Feed> _lstFeeds;

        private static IList<SerieAlias> _lstSerieAlias;

        private static IList<QualidadeDownload> _lstQualidadeDownloads;

        private static IContext _ctx;

        private static bool _isPopulado;

        public static IList<Serie> RetornarListaSeries()
        {
            _lstSeries = new List<Serie>();
            _lstEpisodios = new List<Episodio>();

            var lstIntIdsSeries = new List<int>
            {
                257655,
                273181,
                205281,
                121361,
                274431,
                281485,
                263365,
                281662,
                268592,
                80379,
                279121,
                258744,
                153021,
                264492,
                267440,
                289679,
                88031,
                295068,
                289882,
                114801,
                79151,
                85249,
                295224,
                249827,
                79824,
                278155,
                81797,
                293088,
                259640,
                251085,
                281249
            };

            foreach (var item in lstIntIdsSeries)
            {
                Serie serie = APIRequests.GetSerieInfoAsync(item, "en").Result;
                serie.nCdVideo = _lstSeries.Any()
                                     ? _lstSeries.Last().nCdVideo + 1
                                     : 1;
                serie.nIdTipoConteudo = Enums.TipoConteudo.Série;
                serie.SetDefaultFolderPath();

                _lstSeries.Add(serie);
                foreach (Episodio episodio in serie.lstEpisodios)
                {
                    episodio.nCdEpisodio = _lstEpisodios.Any()
                                               ? _lstEpisodios.Last().nCdEpisodio
                                               : 1;
                    episodio.nCdVideo = serie.nCdVideo;
                    episodio.oSerie = serie;
                    _lstEpisodios.Add(episodio);
                }
            }

            return _lstSeries;
        }

        public static IList<Episodio> RetornaListaEpisodios()
        {
            if (_lstSeries == null)
            {
                throw new Exception("Primeiramente deve ser rodado o método RetornarListaSeries().");
            }

            return _lstEpisodios;
        }

        public static IList<Feed> RetornaListaFeeds()
        {
            _lstFeeds = new List<Feed>
            {
                new Feed
                {
                    nCdFeed = 1,
                    bFlSelecionado = true,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 1,
                    sDsFeed = "sDsFeed 1",
                    sLkFeed = "sLkFeed 1"
                },
                new Feed
                {
                    nCdFeed = 2,
                    bFlSelecionado = false,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 2,
                    sDsFeed = "sDsFeed 2",
                    sLkFeed = "sLkFeed 2"
                },
                new Feed
                {
                    nCdFeed = 3,
                    bFlSelecionado = true,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 3,
                    sDsFeed = "sDsFeed 3",
                    sLkFeed = "sLkFeed 3"
                },
                new Feed
                {
                    nCdFeed = 4,
                    bFlSelecionado = false,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 4,
                    sDsFeed = "sDsFeed 4",
                    sLkFeed = "sLkFeed 4"
                },
                new Feed
                {
                    nCdFeed = 5,
                    bFlSelecionado = true,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 5,
                    sDsFeed = "sDsFeed 5",
                    sLkFeed = "sLkFeed 5"
                },
                new Feed
                {
                    nCdFeed = 6,
                    bFlSelecionado = false,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 1,
                    sDsFeed = "sDsFeed 6",
                    sLkFeed = "sLkFeed 6"
                },
                new Feed
                {
                    nCdFeed = 7,
                    bFlSelecionado = true,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 2,
                    sDsFeed = "sDsFeed 7",
                    sLkFeed = "sLkFeed 7"
                },
                new Feed
                {
                    nCdFeed = 8,
                    bFlSelecionado = false,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 3,
                    sDsFeed = "sDsFeed 8",
                    sLkFeed = "sLkFeed 8"
                },
                new Feed
                {
                    nCdFeed = 9,
                    bFlSelecionado = true,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 4,
                    sDsFeed = "sDsFeed 9",
                    sLkFeed = "sLkFeed 9"
                },
                new Feed
                {
                    nCdFeed = 10,
                    bFlSelecionado = false,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 5,
                    sDsFeed = "sDsFeed 10",
                    sLkFeed = "sLkFeed 10"
                }
            };

            return _lstFeeds;
        }

        public static IList<SerieAlias> RetornarListaSerieAlias()
        {
            _lstSerieAlias = new List<SerieAlias>
            {
                new SerieAlias
                {
                    nCdAlias = 1,
                    sDsAlias = "Agent Carter",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 281485).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 281485)
                },
                new SerieAlias
                {
                    nCdAlias = 2,
                    sDsAlias = "Agents of S.H.I.E.L.D.",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 263365).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 263365)
                },
                new SerieAlias
                {
                    nCdAlias = 3,
                    sDsAlias = "Daredevil",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 281662).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 281662)
                },
                new SerieAlias
                {
                    nCdAlias = 4,
                    sDsAlias = "Mastermind (2012)",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 258744).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 258744)
                },
                new SerieAlias
                {
                    nCdAlias = 5,
                    sDsAlias = "Shingeki no Kyojin",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 267440).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 267440)
                },
                new SerieAlias
                {
                    nCdAlias = 6,
                    sDsAlias = "Shingeki no Kyojin: No Regrets",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 267440).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 267440)
                },
                new SerieAlias
                {
                    nCdAlias = 7,
                    sDsAlias = "Shingeki no Kyojin: Birth of Levi",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 267440).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 267440)
                },
                new SerieAlias
                {
                    nCdAlias = 8,
                    sDsAlias = "Attack on Titan: No Regrets",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 267440).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 267440)
                },
                new SerieAlias
                {
                    nCdAlias = 9,
                    sDsAlias = "Attack on Titan: Birth of Levi",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 267440).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 267440)
                },
                new SerieAlias
                {
                    nCdAlias = 10,
                    sDsAlias = "Dragon Ball Z Kai",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 88031).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 88031)
                },
                new SerieAlias
                {
                    nCdAlias = 11,
                    sDsAlias = "Dragonball Kai",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 88031).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 88031)
                },
                new SerieAlias
                {
                    nCdAlias = 12,
                    sDsAlias = "Dragonball Z Kai",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 88031).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 88031)
                },
                new SerieAlias
                {
                    nCdAlias = 13,
                    sDsAlias = "Dragon Ball Chou",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 295068).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 295068)
                },
                new SerieAlias
                {
                    nCdAlias = 14,
                    sDsAlias = "Fairy Tail S2",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 114801).nCdVideo,
                    nNrTemporada = 5,
                    oSerie = _lstSeries.First(x => x.nCdApi == 114801)
                },
                new SerieAlias
                {
                    nCdAlias = 15,
                    sDsAlias = "Fate Stay Night",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 79151).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 79151)
                },
                new SerieAlias
                {
                    nCdAlias = 16,
                    sDsAlias = "Full Metal Alchemist Brotherhood",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 85249).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 85249)
                },
                new SerieAlias
                {
                    nCdAlias = 17,
                    sDsAlias = "Full Metal Alchemist: Brotherhood",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 85249).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 85249)
                },
                new SerieAlias
                {
                    nCdAlias = 18,
                    sDsAlias = "Hagane no Renkinjutsushi (2009)",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 85249).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 85249)
                },
                new SerieAlias
                {
                    nCdAlias = 19,
                    sDsAlias = "God Eater Burst",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 295224).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 295224)
                },
                new SerieAlias
                {
                    nCdAlias = 20,
                    sDsAlias = "Naruto Shippūden",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 79824).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 79824)
                },
                new SerieAlias
                {
                    nCdAlias = 21,
                    sDsAlias = "Naruto Shippuuden",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 79824).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 79824)
                },
                new SerieAlias
                {
                    nCdAlias = 22,
                    sDsAlias = "Wanpanman",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 293088).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 293088)
                },
                new SerieAlias
                {
                    nCdAlias = 23,
                    sDsAlias = "Sword Art Online II",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 259640).nCdVideo,
                    nNrTemporada = 2,
                    oSerie = _lstSeries.First(x => x.nCdApi == 259640)
                },
                new SerieAlias
                {
                    nCdAlias = 24,
                    sDsAlias = "The Last Airbender The Legend of Korra",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 251085).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 251085)
                },
                new SerieAlias
                {
                    nCdAlias = 25,
                    sDsAlias = "Avatar: The Legend of Korra",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 251085).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 251085)
                },
                new SerieAlias
                {
                    nCdAlias = 26,
                    sDsAlias = "Legend of Korra",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 251085).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 251085)
                },
                new SerieAlias
                {
                    nCdAlias = 27,
                    sDsAlias = "La Leggenda Di Korra",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 251085).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 251085)
                },
                new SerieAlias
                {
                    nCdAlias = 28,
                    sDsAlias = "Tokyo Ghoul Root A",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 281249).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 281249)
                },
                new SerieAlias
                {
                    nCdAlias = 29,
                    sDsAlias = "Tokyo Ghoul",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 281249).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 281249)
                },
                new SerieAlias
                {
                    nCdAlias = 30,
                    sDsAlias = "DanMachi",
                    nNrEpisodio = 1,
                    nCdVideo = _lstSeries.First(x => x.nCdApi == 289882).nCdVideo,
                    nNrTemporada = 1,
                    oSerie = _lstSeries.First(x => x.nCdApi == 289882)
                }
            };

            return _lstSerieAlias;
        }

        public static IList<QualidadeDownload> RetornarListaQualidadeDownload()
        {
            _lstQualidadeDownloads = new List<QualidadeDownload>
            {
                new QualidadeDownload
                {
                    nCdQualidadeDownload = 1,
                    nPrioridade = 1,
                    sIdentificadoresQualidade = "720p",
                    sQualidade = "HD"
                },
                new QualidadeDownload
                {
                    nCdQualidadeDownload = 2,
                    nPrioridade = 2,
                    sIdentificadoresQualidade = "1080p",
                    sQualidade = "FullHD"
                },
                new QualidadeDownload
                {
                    nCdQualidadeDownload = 3,
                    nPrioridade = 3,
                    sIdentificadoresQualidade = "480p",
                    sQualidade = "HQ"
                },
                new QualidadeDownload
                {
                    nCdQualidadeDownload = 4,
                    nPrioridade = 4,
                    sIdentificadoresQualidade = "280p",
                    sQualidade = "Bullshit Quality =D"
                }
            };

            return _lstQualidadeDownloads;
        }

        public static IContext RetornarContextEffort()
        {
            if (_ctx != null)
            {
                return _ctx;
            }

            DbConnection connection = DbConnectionFactory.CreateTransient();

            _ctx = new Context(connection);

            return _ctx;
        }

        public static IContext RetornarContextEffortPopulado()
        {
            if (_isPopulado)
            {
                return _ctx;
            }

            RetornarContextEffort();

            _ctx.QualidadeDownload.AddRange(RetornarListaQualidadeDownload());
            _ctx.Serie.AddRange(RetornarListaSeries());
            _ctx.SerieAlias.AddRange(RetornarListaSerieAlias());
            _ctx.Feed.AddRange(RetornaListaFeeds());
            _ctx.SaveChanges();
            _isPopulado = true;

            return _ctx;
        }
    }
}
