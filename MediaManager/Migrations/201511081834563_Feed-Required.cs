namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Feeds", "sNmFeed", c => c.String(nullable: false));
            AlterColumn("dbo.Feeds", "sNmUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Feeds", "sNmUrl", c => c.String());
            AlterColumn("dbo.Feeds", "sNmFeed", c => c.String());
        }
    }
}
