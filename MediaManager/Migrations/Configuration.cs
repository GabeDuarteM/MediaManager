// Developed by: Gabriel Duarte
// 
// Created at: 18/12/2015 02:11

using System.Data.Entity.Migrations;
using System.Globalization;
using MediaManager.Helpers;
using MediaManager.Localizacao;
using MediaManager.Model;

namespace MediaManager.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //context.People.AddOrUpdate(
            //  p => p.FullName,
            //  new Person { FullName = "Andrew Peters" },
            //  new Person { FullName = "Brice Lambson" },
            //  new Person { FullName = "Rowan Miller" }
            //);
            //

            #region [ Feed Seed ]

            context.Feed.AddOrUpdate(x => x.nCdFeed,
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 2,
                                         nIdTipoConteudo = Enums.TipoConteudo.Série,
                                         nNrPrioridade = 1,
                                         sDsFeed = "ShowRSS 720p",
                                         sDsTagPesquisa = null,
                                         sLkFeed =
                                             "http://showrss.info/user/5225.rss?magnets=true&namespaces=true&name=null&quality=hd&re=yes"
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
                                         sLkFeed =
                                             "http://showrss.info/user/5225.rss?magnets=true&namespaces=true&name=null&quality=sd&re=yes"
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
                                     });

            #endregion

            #region [ QualidadeDownload Seed ]

            context.QualidadeDownload.AddOrUpdate(x => x.nCdQualidadeDownload,
                                                  new QualidadeDownload
                                                  {
                                                      nCdQualidadeDownload = 1,
                                                      nPrioridade = int.MaxValue,
                                                      sIdentificadoresQualidade = double.MinValue.ToString(CultureInfo.InvariantCulture),
                                                      sQualidade = Mensagens.Desconhecido
                                                  },
                                                  new QualidadeDownload
                                                  {
                                                      nCdQualidadeDownload = 2,
                                                      nPrioridade = 1,
                                                      sIdentificadoresQualidade = "WEB-DL|720pHDTV|720HDTV|HDTV720p|HDTV720|BDRip|BRRip|Blu-Ray|BDR|720p|720",
                                                      sQualidade = "HD"
                                                  },
                                                  new QualidadeDownload
                                                  {
                                                      nCdQualidadeDownload = 3,
                                                      nPrioridade = 2,
                                                      sIdentificadoresQualidade = "1080pWEB-DL|1080WEB-DL|WEB-DL1080p|WEB-DL1080|1080pHDTV|1080HDTV|HDTV1080p|HDTV1080|1080pBDRip|1080BDRip|BDRip1080p|BDRip1080|" +
                                                                                  "1080pBRRip|1080BRRip|BRRip1080p|BRRip1080|1080pBlu-Ray|1080Blu-Ray|Blu-Ray1080p|Blu-Ray1080|1080pBDR|1080BDR|BDR1080p|BDR1080|1080p|1080",
                                                      sQualidade = "FullHD"
                                                  },
                                                  new QualidadeDownload
                                                  {
                                                      nCdQualidadeDownload = 4,
                                                      nPrioridade = 3,
                                                      sIdentificadoresQualidade = "DVDRip|DVD-R|Full-Rip|HDTV|480p|480",
                                                      sQualidade = "HQ"
                                                  },
                                                  new QualidadeDownload
                                                  {
                                                      nCdQualidadeDownload = 5,
                                                      nPrioridade = 4,
                                                      sIdentificadoresQualidade = "280p",
                                                      sQualidade = "Bullshit Quality =D"
                                                  });

            #endregion
        }
    }
}
