using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Core.Entities.Product
{
    [Table("ProductAttribute")]
    public class ProductAttribute
    {
        public Guid Id { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}