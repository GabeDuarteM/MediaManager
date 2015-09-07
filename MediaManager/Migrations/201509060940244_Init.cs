namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Episodes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EpisodeName = c.String(),
                        EpisodeNumber = c.Int(nullable: false),
                        SeasonNumber = c.Int(nullable: false),
                        AbsoluteNumber = c.Int(),
                        AirsAfterSeason = c.Int(),
                        AirsBeforeEpisode = c.Int(),
                        AirsBeforeSeason = c.Int(),
                        Artwork = c.String(),
                        FilePath = c.String(),
                        FirstAired = c.DateTime(),
                        FolderPath = c.String(),
                        IDSeasonTvdb = c.Int(nullable: false),
                        IDSerie = c.Int(nullable: false),
                        IDSeriesTvdb = c.Int(nullable: false),
                        IDTvdb = c.Int(nullable: false),
                        IsRenamed = c.Boolean(nullable: false),
                        Language = c.String(),
                        LastUpdated = c.String(),
                        OriginalFilePath = c.String(),
                        Overview = c.String(),
                        Rating = c.Double(),
                        RatingCount = c.Int(),
                        ThumbAddedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Series", t => t.IDSerie, cascadeDelete: true)
                .Index(t => t.IDSerie);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        IDApi = c.Int(nullable: false),
                        Actors = c.String(),
                        Airs_DayOfWeek = c.String(),
                        Airs_Time = c.String(),
                        AliasNames = c.String(),
                        ContentRating = c.String(),
                        FirstAired = c.DateTime(),
                        Genre = c.String(),
                        ImgFanart = c.String(),
                        ImgPoster = c.String(),
                        IsAnime = c.Boolean(nullable: false),
                        Language = c.String(),
                        LastUpdated = c.String(),
                        Network = c.String(),
                        Overview = c.String(),
                        Rating = c.Double(),
                        RatingCount = c.Int(),
                        Runtime = c.Int(),
                        Status = c.String(),
                        FolderPath = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Filmes",
                c => new
                    {
                        IDBanco = c.Int(nullable: false, identity: true),
                        Certification = c.String(),
                        FolderMetadata = c.String(),
                        FolderPath = c.String(),
                        Generos = c.String(),
                        IDApi = c.Int(nullable: false),
                        ImgFanart = c.String(),
                        ImgPoster = c.String(),
                        Language = c.String(),
                        LastUpdated = c.DateTime(),
                        Overview = c.String(),
                        Rating = c.Double(nullable: false),
                        Released = c.String(),
                        Runtime = c.Int(nullable: false),
                        Tagline = c.String(),
                        Title = c.String(),
                        Traducoes = c.String(),
                        Votes = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDBanco);
            
            CreateTable(
                "dbo.Ids",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        imdb = c.String(),
                        slug = c.String(),
                        tmdb = c.Int(nullable: false),
                        trakt = c.Int(nullable: false),
                        tvdb = c.Int(nullable: false),
                        tvrage = c.Int(nullable: false),
                        Filme_IDBanco = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Filmes", t => t.Filme_IDBanco)
                .Index(t => t.Filme_IDBanco);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        banner_full = c.String(),
                        clearart_full = c.String(),
                        fanart_full = c.String(),
                        fanart_medium = c.String(),
                        fanart_thumb = c.String(),
                        logo_full = c.String(),
                        poster_full = c.String(),
                        poster_medium = c.String(),
                        poster_thumb = c.String(),
                        thumb_full = c.String(),
                        Filme_IDBanco = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Filmes", t => t.Filme_IDBanco)
                .Index(t => t.Filme_IDBanco);
            
            CreateTable(
                "dbo.SerieAlias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AliasName = c.String(),
                        Episodio = c.Int(nullable: false),
                        IDSerie = c.Int(nullable: false),
                        Temporada = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Series", t => t.IDSerie, cascadeDelete: true)
                .Index(t => t.IDSerie);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SerieAlias", "IDSerie", "dbo.Series");
            DropForeignKey("dbo.Images", "Filme_IDBanco", "dbo.Filmes");
            DropForeignKey("dbo.Ids", "Filme_IDBanco", "dbo.Filmes");
            DropForeignKey("dbo.Episodes", "IDSerie", "dbo.Series");
            DropIndex("dbo.SerieAlias", new[] { "IDSerie" });
            DropIndex("dbo.Images", new[] { "Filme_IDBanco" });
            DropIndex("dbo.Ids", new[] { "Filme_IDBanco" });
            DropIndex("dbo.Episodes", new[] { "IDSerie" });
            DropTable("dbo.SerieAlias");
            DropTable("dbo.Images");
            DropTable("dbo.Ids");
            DropTable("dbo.Filmes");
            DropTable("dbo.Series");
            DropTable("dbo.Episodes");
        }
    }
}
