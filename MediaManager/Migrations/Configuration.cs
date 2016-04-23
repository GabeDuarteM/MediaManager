// Developed by: Gabriel Duarte
// 
// Created at: 18/12/2015 02:11

using System.Data.Entity.Migrations;
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
            context.Feed.AddOrUpdate(x => x.nCdFeed,
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 2,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série,
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
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série,
                                         nNrPrioridade = 2,
                                         sDsFeed = "Kickass Séries 720p",
                                         sDsTagPesquisa = null,
                                         sLkFeed = "http://kat.cr/usearch/720%20category%3Atv/?rss=1"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 4,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série,
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
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série,
                                         nNrPrioridade = 4,
                                         sDsFeed = "Kickass Séries",
                                         sDsTagPesquisa = null,
                                         sLkFeed = "https://kat.cr/tv/?rss=1"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 5,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Filme,
                                         nNrPrioridade = 1,
                                         sDsFeed = "Kickass Filmes",
                                         sDsTagPesquisa = null,
                                         sLkFeed = "https://kat.cr/movies/?rss=1"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 6,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 1,
                                         sDsFeed = "Nyaa A+ 720p",
                                         sDsTagPesquisa = null,
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term=720&filter=3"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 7,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 2,
                                         sDsFeed = "Nyaa Trusted 720p",
                                         sDsTagPesquisa = null,
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term=720&filter=2"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 8,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 3,
                                         sDsFeed = "Nyaa Tudo 720p",
                                         sDsTagPesquisa = null,
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term=720"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 9,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 4,
                                         sDsFeed = "Nyaa A+",
                                         sDsTagPesquisa = null,
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&filter=3"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 10,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 5,
                                         sDsFeed = "Nyaa Trusted",
                                         sDsTagPesquisa = null,
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&filter=2"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = false,
                                         nCdFeed = 11,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 6,
                                         sDsFeed = "Nyaa Tudo",
                                         sDsTagPesquisa = null,
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = true,
                                         nCdFeed = 12,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série,
                                         nNrPrioridade = 1,
                                         sDsFeed = "Kickass Séries 720p",
                                         sDsTagPesquisa = "{TAG}",
                                         sLkFeed = "http://kat.cr/usearch/{TAG}%20720%20category%3Atv/?rss=1"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = true,
                                         nCdFeed = 14,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Série,
                                         nNrPrioridade = 2,
                                         sDsFeed = "Kickass Séries",
                                         sDsTagPesquisa = "{TAG}",
                                         sLkFeed = "http://kat.cr/usearch/{TAG}%20category%3Atv/?rss=1"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = true,
                                         nCdFeed = 16,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Filme,
                                         nNrPrioridade = 1,
                                         sDsFeed = "Kickass Filmes",
                                         sDsTagPesquisa = "{TAG}",
                                         sLkFeed = "http://kat.cr/usearch/{TAG}%20720%20category%3Amovies/?rss=1"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = true,
                                         nCdFeed = 17,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 1,
                                         sDsFeed = "Nyaa A+ 720p",
                                         sDsTagPesquisa = "{TAG}",
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term={TAG}+720&filter=3"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = true,
                                         nCdFeed = 18,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 2,
                                         sDsFeed = "Nyaa Trusted 720p",
                                         sDsTagPesquisa = "{TAG}",
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term={TAG}+720&filter=2"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = true,
                                         nCdFeed = 19,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 3,
                                         sDsFeed = "Nyaa Tudo 720p",
                                         sDsTagPesquisa = "{TAG}",
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term={TAG}+720"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = true,
                                         nCdFeed = 20,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 4,
                                         sDsFeed = "Nyaa A+",
                                         sDsTagPesquisa = "{TAG}",
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&filter=3&term={TAG}"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = true,
                                         nCdFeed = 21,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 5,
                                         sDsFeed = "Nyaa Trusted",
                                         sDsTagPesquisa = "{TAG}",
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&filter=2&term={TAG}"
                                     },
                                     new Feed
                                     {
                                         bIsFeedPesquisa = true,
                                         nCdFeed = 22,
                                         nIdTipoConteudo = Helpers.Enums.TipoConteudo.Anime,
                                         nNrPrioridade = 6,
                                         sDsFeed = "Nyaa Tudo",
                                         sDsTagPesquisa = "{TAG}",
                                         sLkFeed = "http://www.nyaa.se/?page=rss&cats=1_37&term={TAG}"
                                     });
        }
    }
}
