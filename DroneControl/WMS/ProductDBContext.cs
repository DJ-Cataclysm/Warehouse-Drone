using System.Data.Entity;

namespace WMS
{
    public class ProductDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}