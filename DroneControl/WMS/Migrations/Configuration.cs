namespace WMS.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WMS.ProductDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WMS.ProductDBContext context)
        {
            context.Products.AddOrUpdate(
              p => p.ID,
              new Product { Title = "Banaan", Count = 4, Description="Chiquita", LastCheck = DateTime.Now },
              new Product { Title = "Peer", Count = 8, Description = "Conference", LastCheck = DateTime.Now },
              new Product { Title = "Appel", Count = 0, Description = "Golden Delicious", LastCheck = DateTime.Now }
            );
        }
    }
}
