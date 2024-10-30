using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Core.Entities.Product
{
    [Table("ProductImages")]
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; } = false;

        public Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } 

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
