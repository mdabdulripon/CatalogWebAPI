using Catalog.Core.Entities.Product;

namespace Catalog.Core.Dtos
{
    public class ProductVariantDto
    {
        public Guid? Id { get; set; }
        public ICollection<ProductAttributeDto> ProductAttributes { get; set; }
        public ICollection<ProductImageDto> ProductImages { get; set; }
    }
}

