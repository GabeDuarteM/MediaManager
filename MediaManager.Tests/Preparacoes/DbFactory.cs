// Developed by: Gabriel Duarte
// 
// Created at: 19/04/2016 01:00

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

            foreach (int item in lstIntIdsSeries)
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
                    bIsFeedPesquisa = false,
                    nCdFeed = 2,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 1,
                    sDsFeed = "ShowRSS 720p",
                    sDsTagPesquisa = null,
                    sLkFeed = "http://showrss.info/user/5225.rss?magnets=true&namespaces=true&name=null&quality=hd&re=yes"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 1,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 2,
                    sDsFeed = "Kickass Séries 720p",
                    sDsTagPesquisa = null,
                    sLkFeed = "http://kat.cr/usearch/720%20category%3Atv/?rss=1"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 4,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 3,
                    sDsFeed = "ShowRSS",
                    sDsTagPesquisa = null,
                    sLkFeed = "http://showrss.info/user/5225.rss?magnets=true&namespaces=true&name=null&quality=sd&re=yes"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 3,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 4,
                    sDsFeed = "Kickass Séries",
                    sDsTagPesquisa = null,
                    sLkFeed = "https://kat.cr/tv/?rss=1"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 5,
                    nIdTipoConteudo = Enums.TipoConteudo.Filme,
                    nNrPrioridade = 1,
                    sDsFeed = "Kickass Filmes",
                    sDsTagPesquisa = null,
                    sLkFeed = "https://kat.cr/movies/?rss=1"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 6,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 1,
                    sDsFeed = "Nyaa A+ 720p",
                    sDsTagPesquisa = null,
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term=720&filter=3"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 7,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 2,
                    sDsFeed = "Nyaa Trusted 720p",
                    sDsTagPesquisa = null,
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term=720&filter=2"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 8,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 3,
                    sDsFeed = "Nyaa Tudo 720p",
                    sDsTagPesquisa = null,
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term=720"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 9,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 4,
                    sDsFeed = "Nyaa A+",
                    sDsTagPesquisa = null,
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&filter=3"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 10,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 5,
                    sDsFeed = "Nyaa Trusted",
                    sDsTagPesquisa = null,
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&filter=2"
                },
                new Feed
                {
                    bIsFeedPesquisa = false,
                    nCdFeed = 11,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 6,
                    sDsFeed = "Nyaa Tudo",
                    sDsTagPesquisa = null,
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37"
                },
                new Feed
                {
                    bIsFeedPesquisa = true,
                    nCdFeed = 12,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 1,
                    sDsFeed = "Kickass Séries 720p",
                    sDsTagPesquisa = "{TAG}",
                    sLkFeed = "http://kat.cr/usearch/{TAG}%20720%20category%3Atv/?rss=1"
                },
                new Feed
                {
                    bIsFeedPesquisa = true,
                    nCdFeed = 14,
                    nIdTipoConteudo = Enums.TipoConteudo.Série,
                    nNrPrioridade = 2,
                    sDsFeed = "Kickass Séries",
                    sDsTagPesquisa = "{TAG}",
                    sLkFeed = "http://kat.cr/usearch/{TAG}%20category%3Atv/?rss=1"
                },
                new Feed
                {
                    bIsFeedPesquisa = true,
                    nCdFeed = 16,
                    nIdTipoConteudo = Enums.TipoConteudo.Filme,
                    nNrPrioridade = 1,
                    sDsFeed = "Kickass Filmes",
                    sDsTagPesquisa = "{TAG}",
                    sLkFeed = "http://kat.cr/usearch/{TAG}%20720%20category%3Amovies/?rss=1"
                },
                new Feed
                {
                    bIsFeedPesquisa = true,
                    nCdFeed = 17,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 1,
                    sDsFeed = "Nyaa A+ 720p",
                    sDsTagPesquisa = "{TAG}",
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term={TAG}+720&filter=3"
                },
                new Feed
                {
                    bIsFeedPesquisa = true,
                    nCdFeed = 18,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 2,
                    sDsFeed = "Nyaa Trusted 720p",
                    sDsTagPesquisa = "{TAG}",
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term={TAG}+720&filter=2"
                },
                new Feed
                {
                    bIsFeedPesquisa = true,
                    nCdFeed = 19,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 3,
                    sDsFeed = "Nyaa Tudo 720p",
                    sDsTagPesquisa = "{TAG}",
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term={TAG}+720"
                },
                new Feed
                {
                    bIsFeedPesquisa = true,
                    nCdFeed = 20,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 4,
                    sDsFeed = "Nyaa A+",
                    sDsTagPesquisa = "{TAG}",
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&filter=3&term={TAG}"
                },
                new Feed
                {
                    bIsFeedPesquisa = true,
                    nCdFeed = 21,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 5,
                    sDsFeed = "Nyaa Trusted",
                    sDsTagPesquisa = "{TAG}",
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&filter=2&term={TAG}"
                },
                new Feed
                {
                    bIsFeedPesquisa = true,
                    nCdFeed = 22,
                    nIdTipoConteudo = Enums.TipoConteudo.Anime,
                    nNrPrioridade = 6,
                    sDsFeed = "Nyaa Tudo",
                    sDsTagPesquisa = "{TAG}",
                    sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term={TAG}"
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
                    sIdentificadoresQualidade = "720p|HDTV",
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
