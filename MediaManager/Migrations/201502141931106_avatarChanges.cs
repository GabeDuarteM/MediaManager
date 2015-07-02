namespace MediaManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class avatarChanges : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Images", "avatar_full");
        }

        public override void Down()
        {
            AddColumn("dbo.Images", "avatar_full", c => c.String());
        }
    }
}