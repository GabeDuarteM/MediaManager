namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alterar_Serie_Traducoes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Filmes", "Traducoes", c => c.String());
            AddColumn("dbo.Filmes", "Generos", c => c.String());
            AddColumn("dbo.Series", "Generos", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Series", "Generos");
            DropColumn("dbo.Filmes", "Generos");
            DropColumn("dbo.Filmes", "Traducoes");
        }
    }
}
