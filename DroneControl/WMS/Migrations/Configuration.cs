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
              new Product { }, //1
              new Product { Title = "Banaan", Count = 1, Description="Chiquita!", LastCheck = DateTime.Now, X = 0, Y = 2, Z = 0 }, //2
              new Product { Title = "Bivakmutsen", Count = 0, Description = "", LastCheck = DateTime.Now, X = 0 , Y = 1, Z = 0}, //3
              new Product { Title = "Waardigheid van Jordi", Count = 1, Description = "Deze is leeg.", LastCheck = DateTime.Now, X = 1, Y = 2, Z = 0 }, //4
              new Product { Title = "Gestolen kernwapens", Count = 0, Description = "", LastCheck = DateTime.Now, X = 1, Y = 1, Z = 0 }, //5
              new Product { }, //6
              new Product { }, //7
              new Product { Title = "Ventieldopjes", Count = 1, Description = "", LastCheck = DateTime.Now, X = 2, Y = 1, Z = 0}, //8
              new Product { }, //9
              new Product { Title = "Amerikaanse staatsgeheimen", Count = 1, Description = "", LastCheck = DateTime.Now, X = 3, Y = 1, Z = 0 } //10

            );
        }
    }
}
