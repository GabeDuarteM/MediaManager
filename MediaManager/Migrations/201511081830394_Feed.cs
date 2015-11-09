namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Feed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ids", "Filme_IDBanco", "dbo.Filmes");
            DropForeignKey("dbo.Images", "Filme_IDBanco", "dbo.Filmes");
            DropIndex("dbo.Ids", new[] { "Filme_IDBanco" });
            DropIndex("dbo.Images", new[] { "Filme_IDBanco" });
            CreateTable(
                "dbo.Feeds",
                c => new
                    {
                        nCdFeed = c.Int(nullable: false, identity: true),
                        sNmFeed = c.String(),
                        sNmUrl = c.String(),
                        nIdTipoConteudo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.nCdFeed);
            
            DropTable("dbo.Filmes");
            DropTable("dbo.Ids");
            DropTable("dbo.Images");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.ID);
            
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
            
            DropTable("dbo.Feeds");
            CreateIndex("dbo.Images", "Filme_IDBanco");
            CreateIndex("dbo.Ids", "Filme_IDBanco");
            AddForeignKey("dbo.Images", "Filme_IDBanco", "dbo.Filmes", "IDBanco");
            AddForeignKey("dbo.Ids", "Filme_IDBanco", "dbo.Filmes", "IDBanco");
        }
    }
}
