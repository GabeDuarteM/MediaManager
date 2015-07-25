namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Ajuste_Nos_Ids_Serie_Filme : DbMigration
    {
        public override void Down()
        {
            DropPrimaryKey("dbo.Series");
            DropPrimaryKey("dbo.Filmes");
            DropColumn("dbo.Series", "ID");
            DropColumn("dbo.Filmes", "ID");
            AddColumn("dbo.Series", "IDSerie", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Filmes", "IDFilme", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Series", "IDSerie");
            AddPrimaryKey("dbo.Filmes", "IDFilme");
        }

        public override void Up()
        {
            DropPrimaryKey("dbo.Filmes");
            DropPrimaryKey("dbo.Series");
            DropColumn("dbo.Filmes", "IDFilme");
            DropColumn("dbo.Series", "IDSerie");
            AddColumn("dbo.Filmes", "ID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Series", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Filmes", "ID");
            AddPrimaryKey("dbo.Series", "ID");
        }
    }
}