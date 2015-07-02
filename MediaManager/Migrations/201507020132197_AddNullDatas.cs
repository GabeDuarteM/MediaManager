namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNullDatas : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Series", "first_aired", c => c.DateTime());
            AlterColumn("dbo.Series", "updated_at", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Series", "updated_at", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Series", "first_aired", c => c.DateTime(nullable: false));
        }
    }
}
