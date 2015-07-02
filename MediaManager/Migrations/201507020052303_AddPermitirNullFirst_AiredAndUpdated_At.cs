namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPermitirNullFirst_AiredAndUpdated_At : DbMigration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            AlterColumn("dbo.Series", "updated_at", c => c.DateTime(nullable: true));
            AlterColumn("dbo.Series", "first_aired", c => c.DateTime(nullable: true));
        }
    }
}