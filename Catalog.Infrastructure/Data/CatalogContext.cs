using Catalog.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
