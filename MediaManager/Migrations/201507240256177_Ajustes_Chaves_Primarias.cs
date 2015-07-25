namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Ajustes_Chaves_Primarias : DbMigration
    {
        public override void Down()
        {
            AddColumn("dbo.Series", "Images_IDImages", c => c.Int());
            AddColumn("dbo.Series", "Ids_IDIds", c => c.Int());
            AddColumn("dbo.Images", "IDImages", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Ids", "IDIds", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Filmes", "Images_IDImages", c => c.Int());
            AddColumn("dbo.Filmes", "Ids_IDIds", c => c.Int());
            DropForeignKey("dbo.Images", "Serie_ID", "dbo.Series");
            DropForeignKey("dbo.Images", "Filme_ID", "dbo.Filmes");
            DropForeignKey("dbo.Ids", "Serie_ID", "dbo.Series");
            DropForeignKey("dbo.Ids", "Filme_ID", "dbo.Filmes");
            DropIndex("dbo.Images", new[] { "Serie_ID" });
            DropIndex("dbo.Images", new[] { "Filme_ID" });
            DropIndex("dbo.Ids", new[] { "Serie_ID" });
            DropIndex("dbo.Ids", new[] { "Filme_ID" });
            DropPrimaryKey("dbo.Images");
            DropPrimaryKey("dbo.Ids");
            DropColumn("dbo.Images", "Serie_ID");
            DropColumn("dbo.Images", "Filme_ID");
            DropColumn("dbo.Images", "ID");
            DropColumn("dbo.Ids", "Serie_ID");
            DropColumn("dbo.Ids", "Filme_ID");
            DropColumn("dbo.Ids", "ID");
            AddPrimaryKey("dbo.Images", "IDImages");
            AddPrimaryKey("dbo.Ids", "IDIds");
            CreateIndex("dbo.Series", "Images_IDImages");
            CreateIndex("dbo.Series", "Ids_IDIds");
            CreateIndex("dbo.Filmes", "Images_IDImages");
            CreateIndex("dbo.Filmes", "Ids_IDIds");
            AddForeignKey("dbo.Series", "Images_IDImages", "dbo.Images", "IDImages");
            AddForeignKey("dbo.Series", "Ids_IDIds", "dbo.Ids", "IDIds");
            AddForeignKey("dbo.Filmes", "Images_IDImages", "dbo.Images", "IDImages");
            AddForeignKey("dbo.Filmes", "Ids_IDIds", "dbo.Ids", "IDIds");
        }

        public override void Up()
        {
            DropForeignKey("dbo.Filmes", "Ids_IDIds", "dbo.Ids");
            DropForeignKey("dbo.Filmes", "Images_IDImages", "dbo.Images");
            DropForeignKey("dbo.Series", "Ids_IDIds", "dbo.Ids");
            DropForeignKey("dbo.Series", "Images_IDImages", "dbo.Images");
            DropIndex("dbo.Filmes", new[] { "Ids_IDIds" });
            DropIndex("dbo.Filmes", new[] { "Images_IDImages" });
            DropIndex("dbo.Series", new[] { "Ids_IDIds" });
            DropIndex("dbo.Series", new[] { "Images_IDImages" });
            DropPrimaryKey("dbo.Ids");
            DropPrimaryKey("dbo.Images");
            DropColumn("dbo.Ids", "IDIds");
            DropColumn("dbo.Images", "IDImages");
            AddColumn("dbo.Ids", "ID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Ids", "Filme_ID", c => c.Int());
            AddColumn("dbo.Ids", "Serie_ID", c => c.Int());
            AddColumn("dbo.Images", "ID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Images", "Filme_ID", c => c.Int());
            AddColumn("dbo.Images", "Serie_ID", c => c.Int());
            AddPrimaryKey("dbo.Ids", "ID");
            AddPrimaryKey("dbo.Images", "ID");
            CreateIndex("dbo.Ids", "Filme_ID");
            CreateIndex("dbo.Ids", "Serie_ID");
            CreateIndex("dbo.Images", "Filme_ID");
            CreateIndex("dbo.Images", "Serie_ID");
            AddForeignKey("dbo.Ids", "Filme_ID", "dbo.Filmes", "ID");
            AddForeignKey("dbo.Ids", "Serie_ID", "dbo.Series", "ID");
            AddForeignKey("dbo.Images", "Filme_ID", "dbo.Filmes", "ID");
            AddForeignKey("dbo.Images", "Serie_ID", "dbo.Series", "ID");
            DropColumn("dbo.Filmes", "Ids_IDIds");
            DropColumn("dbo.Filmes", "Images_IDImages");
            DropColumn("dbo.Series", "Ids_IDIds");
            DropColumn("dbo.Series", "Images_IDImages");
        }
    }
}