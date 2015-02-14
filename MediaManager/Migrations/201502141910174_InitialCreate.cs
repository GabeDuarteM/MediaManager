namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Filmes",
                c => new
                    {
                        IDFilme = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        year = c.Int(nullable: false),
                        tagline = c.String(),
                        overview = c.String(),
                        released = c.String(),
                        runtime = c.Int(nullable: false),
                        rating = c.Double(nullable: false),
                        votes = c.Int(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        language = c.String(),
                        certification = c.String(),
                        ids_IDIds = c.Int(),
                        images_IDImages = c.Int(),
                    })
                .PrimaryKey(t => t.IDFilme)
                .ForeignKey("dbo.Ids", t => t.ids_IDIds)
                .ForeignKey("dbo.Images", t => t.images_IDImages)
                .Index(t => t.ids_IDIds)
                .Index(t => t.images_IDImages);
            
            CreateTable(
                "dbo.Ids",
                c => new
                    {
                        IDIds = c.Int(nullable: false, identity: true),
                        trakt = c.Int(nullable: false),
                        slug = c.String(),
                        tvdb = c.Int(nullable: false),
                        imdb = c.String(),
                        tmdb = c.Int(nullable: false),
                        tvrage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDIds);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        IDImages = c.Int(nullable: false, identity: true),
                        fanart_full = c.String(),
                        fanart_medium = c.String(),
                        fanart_thumb = c.String(),
                        poster_full = c.String(),
                        poster_medium = c.String(),
                        poster_thumb = c.String(),
                        logo_full = c.String(),
                        clearart_full = c.String(),
                        banner_full = c.String(),
                        thumb_full = c.String(),
                        avatar_full = c.String(),
                    })
                .PrimaryKey(t => t.IDImages);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        IDSerie = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        year = c.Int(nullable: false),
                        overview = c.String(),
                        first_aired = c.DateTime(nullable: false),
                        runtime = c.Int(nullable: false),
                        certification = c.String(),
                        network = c.String(),
                        country = c.String(),
                        status = c.String(),
                        rating = c.Double(nullable: false),
                        votes = c.Int(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        language = c.String(),
                        aired_episodes = c.Int(nullable: false),
                        airDay = c.String(),
                        airTime = c.String(),
                        airTimezone = c.String(),
                        ids_IDIds = c.Int(),
                        images_IDImages = c.Int(),
                    })
                .PrimaryKey(t => t.IDSerie)
                .ForeignKey("dbo.Ids", t => t.ids_IDIds)
                .ForeignKey("dbo.Images", t => t.images_IDImages)
                .Index(t => t.ids_IDIds)
                .Index(t => t.images_IDImages);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Series", "images_IDImages", "dbo.Images");
            DropForeignKey("dbo.Series", "ids_IDIds", "dbo.Ids");
            DropForeignKey("dbo.Filmes", "images_IDImages", "dbo.Images");
            DropForeignKey("dbo.Filmes", "ids_IDIds", "dbo.Ids");
            DropIndex("dbo.Series", new[] { "images_IDImages" });
            DropIndex("dbo.Series", new[] { "ids_IDIds" });
            DropIndex("dbo.Filmes", new[] { "images_IDImages" });
            DropIndex("dbo.Filmes", new[] { "ids_IDIds" });
            DropTable("dbo.Series");
            DropTable("dbo.Images");
            DropTable("dbo.Ids");
            DropTable("dbo.Filmes");
        }
    }
}
