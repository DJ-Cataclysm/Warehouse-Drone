namespace WMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Afwijking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mutations", "Afwijking", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "Afwijking", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Afwijking");
            DropColumn("dbo.Mutations", "Afwijking");
        }
    }
}
