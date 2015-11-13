namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionarPrioridadeNoFeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feeds", "nNrPrioridade", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feeds", "nNrPrioridade");
        }
    }
}
