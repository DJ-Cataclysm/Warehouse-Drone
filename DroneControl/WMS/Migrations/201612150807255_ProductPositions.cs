namespace WMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductPositions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "X", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Y", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Z", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Z");
            DropColumn("dbo.Products", "Y");
            DropColumn("dbo.Products", "X");
        }
    }
}
