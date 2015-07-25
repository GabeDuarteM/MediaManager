namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajustes_classe_interface_video : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Filmes", "UpdatedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Filmes", "UpdatedAt", c => c.DateTime(nullable: false));
        }
    }
}
