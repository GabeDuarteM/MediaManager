namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteNomesMaiusculos : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Filmes", new[] { "ids_IDIds" });
            DropIndex("dbo.Filmes", new[] { "images_IDImages" });
            DropIndex("dbo.Series", new[] { "ids_IDIds" });
            DropIndex("dbo.Series", new[] { "images_IDImages" });
            AddColumn("dbo.Filmes", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Series", "FirstAired", c => c.DateTime());
            AddColumn("dbo.Series", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.Series", "AiredEpisodes", c => c.Int(nullable: false));
            CreateIndex("dbo.Filmes", "Ids_IDIds");
            CreateIndex("dbo.Filmes", "Images_IDImages");
            CreateIndex("dbo.Series", "Ids_IDIds");
            CreateIndex("dbo.Series", "Images_IDImages");
            DropColumn("dbo.Filmes", "updated_at");
            DropColumn("dbo.Series", "first_aired");
            DropColumn("dbo.Series", "updated_at");
            DropColumn("dbo.Series", "aired_episodes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Series", "aired_episodes", c => c.Int(nullable: false));
            AddColumn("dbo.Series", "updated_at", c => c.DateTime());
            AddColumn("dbo.Series", "first_aired", c => c.DateTime());
            AddColumn("dbo.Filmes", "updated_at", c => c.DateTime(nullable: false));
            DropIndex("dbo.Series", new[] { "Images_IDImages" });
            DropIndex("dbo.Series", new[] { "Ids_IDIds" });
            DropIndex("dbo.Filmes", new[] { "Images_IDImages" });
            DropIndex("dbo.Filmes", new[] { "Ids_IDIds" });
            DropColumn("dbo.Series", "AiredEpisodes");
            DropColumn("dbo.Series", "UpdatedAt");
            DropColumn("dbo.Series", "FirstAired");
            DropColumn("dbo.Filmes", "UpdatedAt");
            CreateIndex("dbo.Series", "images_IDImages");
            CreateIndex("dbo.Series", "ids_IDIds");
            CreateIndex("dbo.Filmes", "images_IDImages");
            CreateIndex("dbo.Filmes", "ids_IDIds");
        }
    }
}
