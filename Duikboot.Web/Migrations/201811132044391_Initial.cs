namespace Duikboot.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Capacities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(unicode: false),
                        SurName = c.String(unicode: false),
                        Email = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        Zaterdag = c.Boolean(),
                        Zondag = c.Boolean(),
                        Maandag = c.Boolean(),
                        Dinsdag = c.Boolean(),
                        Amount = c.Decimal(precision: 18, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Capacities");
        }
    }
}
