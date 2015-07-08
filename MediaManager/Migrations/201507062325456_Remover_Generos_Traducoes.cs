namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remover_Generos_Traducoes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Traducoes", "IdFilme", "dbo.Filmes");
            DropForeignKey("dbo.Generos", "IdFilme", "dbo.Filmes");
            DropForeignKey("dbo.Generos", "IdSerie", "dbo.Series");
            DropForeignKey("dbo.Traducoes", "IdSerie", "dbo.Series");
            DropIndex("dbo.Traducoes", new[] { "IdSerie" });
            DropIndex("dbo.Traducoes", new[] { "IdFilme" });
            DropIndex("dbo.Generos", new[] { "IdSerie" });
            DropIndex("dbo.Generos", new[] { "IdFilme" });
            AddColumn("dbo.Series", "Traducoes", c => c.String());
            DropTable("dbo.Traducoes");
            DropTable("dbo.Generos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Generos",
                c => new
                    {
                        IdGenero = c.Int(nullable: false, identity: true),
                        NomeGenero = c.String(),
                        IdSerie = c.Int(),
                        IdFilme = c.Int(),
                    })
                .PrimaryKey(t => t.IdGenero);
            
            CreateTable(
                "dbo.Traducoes",
                c => new
                    {
                        IdTraducao = c.Int(nullable: false, identity: true),
                        Idioma = c.String(),
                        IdSerie = c.Int(),
                        IdFilme = c.Int(),
                    })
                .PrimaryKey(t => t.IdTraducao);
            
            DropColumn("dbo.Series", "Traducoes");
            CreateIndex("dbo.Generos", "IdFilme");
            CreateIndex("dbo.Generos", "IdSerie");
            CreateIndex("dbo.Traducoes", "IdFilme");
            CreateIndex("dbo.Traducoes", "IdSerie");
            AddForeignKey("dbo.Traducoes", "IdSerie", "dbo.Series", "IDSerie");
            AddForeignKey("dbo.Generos", "IdSerie", "dbo.Series", "IDSerie");
            AddForeignKey("dbo.Generos", "IdFilme", "dbo.Filmes", "IDFilme");
            AddForeignKey("dbo.Traducoes", "IdFilme", "dbo.Filmes", "IDFilme");
        }
    }
}
