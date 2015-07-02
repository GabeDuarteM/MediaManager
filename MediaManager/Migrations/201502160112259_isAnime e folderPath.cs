namespace MediaManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class isAnimeefolderPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Filmes", "folderPath", c => c.String());
            AddColumn("dbo.Series", "folderPath", c => c.String());
            AddColumn("dbo.Series", "isAnime", c => c.Boolean(nullable: false, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Series", "isAnime");
            DropColumn("dbo.Series", "folderPath");
            DropColumn("dbo.Filmes", "folderPath");
        }
    }
}