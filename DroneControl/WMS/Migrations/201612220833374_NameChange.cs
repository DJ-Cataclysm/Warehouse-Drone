namespace WMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mutations", "Deviation", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "Deviation", c => c.Double(nullable: false));
            DropColumn("dbo.Mutations", "Afwijking");
            DropColumn("dbo.Products", "Afwijking");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Afwijking", c => c.Double(nullable: false));
            AddColumn("dbo.Mutations", "Afwijking", c => c.Double(nullable: false));
            DropColumn("dbo.Products", "Deviation");
            DropColumn("dbo.Mutations", "Deviation");
        }
    }
}
