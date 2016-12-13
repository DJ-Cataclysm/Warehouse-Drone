namespace WMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MutationsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mutations",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        MutationDate = c.DateTime(nullable: false),
                        NewCount = c.Int(nullable: false),
                        OldCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.MutationDate });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Mutations");
        }
    }
}
