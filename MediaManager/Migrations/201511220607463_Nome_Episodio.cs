namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nome_Episodio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodios", "sDsEpisodio", c => c.String());
            DropColumn("dbo.Episodios", "sDsNome");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Episodios", "sDsNome", c => c.String());
            DropColumn("dbo.Episodios", "sDsEpisodio");
        }
    }
}
