namespace Duikboot.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSpotsToCapicity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Capacities", "Spots", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Capacities", "Spots");
        }
    }
}
