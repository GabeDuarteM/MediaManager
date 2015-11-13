namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MediaManager.Model.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MediaManager.Model.Context context)
        {
            context.Feed.AddOrUpdate(x => x.nCdFeed,
                new Model.Feed { nCdFeed = 1, nIdTipoConteudo = Helpers.Enums.ContentType.Anime, nNrPrioridade = 3, sNmFeed = "Nyaa.se", sNmUrl = @"http://www.nyaa.se/?page=rss&cats=1_37" },
                new Model.Feed { nCdFeed = 2, nIdTipoConteudo = Helpers.Enums.ContentType.Anime, nNrPrioridade = 2, sNmFeed = "Nyaa.se Trusted", sNmUrl = @"http://www.nyaa.se/?page=rss&cats=1_37&filter=2" },
                new Model.Feed { nCdFeed = 3, nIdTipoConteudo = Helpers.Enums.ContentType.Anime, nNrPrioridade = 1, sNmFeed = "Nyaa.se A+", sNmUrl = @"http://www.nyaa.se/?page=rss&cats=1_37&filter=3" },
                new Model.Feed { nCdFeed = 4, nIdTipoConteudo = Helpers.Enums.ContentType.Série, nNrPrioridade = 1, sNmFeed = "Kickass", sNmUrl = @"https://kat.cr/tv/?rss=1" },
                new Model.Feed { nCdFeed = 5, nIdTipoConteudo = Helpers.Enums.ContentType.Série, nNrPrioridade = 2, sNmFeed = "ExtraTorrent", sNmUrl = @"http://extratorrent.cc/rss.xml?type=popular&cid=8" },
                new Model.Feed { nCdFeed = 6, nIdTipoConteudo = Helpers.Enums.ContentType.Série, nNrPrioridade = 3, sNmFeed = "TorrentZ", sNmUrl = @"https://torrentz.eu/feed?q=tv" }
                );
        }
    }
}