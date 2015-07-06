namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Generos_e_Traducoes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Traducoes",
                c => new
                    {
                        IdTraducao = c.Int(nullable: false, identity: true),
                        Idioma = c.String(),
                        IdSerie = c.Int(),
                        IdFilme = c.Int(),
                    })
                .PrimaryKey(t => t.IdTraducao)
                .ForeignKey("dbo.Filmes", t => t.IdFilme)
                .ForeignKey("dbo.Series", t => t.IdSerie)
                .Index(t => t.IdSerie)
                .Index(t => t.IdFilme);
            
            CreateTable(
                "dbo.Generos",
                c => new
                    {
                        IdGenero = c.Int(nullable: false, identity: true),
                        NomeGenero = c.String(),
                        IdSerie = c.Int(),
                        IdFilme = c.Int(),
                    })
                .PrimaryKey(t => t.IdGenero)
                .ForeignKey("dbo.Filmes", t => t.IdFilme)
                .ForeignKey("dbo.Series", t => t.IdSerie)
                .Index(t => t.IdSerie)
                .Index(t => t.IdFilme);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Traducoes", "IdSerie", "dbo.Series");
            DropForeignKey("dbo.Generos", "IdSerie", "dbo.Series");
            DropForeignKey("dbo.Generos", "IdFilme", "dbo.Filmes");
            DropForeignKey("dbo.Traducoes", "IdFilme", "dbo.Filmes");
            DropIndex("dbo.Generos", new[] { "IdFilme" });
            DropIndex("dbo.Generos", new[] { "IdSerie" });
            DropIndex("dbo.Traducoes", new[] { "IdFilme" });
            DropIndex("dbo.Traducoes", new[] { "IdSerie" });
            DropTable("dbo.Generos");
            DropTable("dbo.Traducoes");
        }
    }
}
