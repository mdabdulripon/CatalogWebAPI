using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Core.Entities.Product
{
    [Table("ProductVariants")]
    public class ProductVariant
    {
        public Guid Id { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}