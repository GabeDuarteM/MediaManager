namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajuste_Feeds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feeds", "sDsFeed", c => c.String(nullable: false));
            DropColumn("dbo.Feeds", "sNmFeed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Feeds", "sNmFeed", c => c.String(nullable: false));
            DropColumn("dbo.Feeds", "sDsFeed");
        }
    }
}
