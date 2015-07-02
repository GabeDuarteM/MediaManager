namespace MediaManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class metadataFolder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Filmes", "metadataFolder", c => c.String());
            AddColumn("dbo.Series", "metadataFolder", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Series", "metadataFolder");
            DropColumn("dbo.Filmes", "metadataFolder");
        }
    }
}