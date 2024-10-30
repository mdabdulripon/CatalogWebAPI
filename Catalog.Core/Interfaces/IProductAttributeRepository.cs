using Catalog.Core.Dtos;
using Catalog.Core.Entities.Product;

namespace Catalog.Core.Interfaces;

public interface IProductAttributeRepository
{
    // Task<IList<ProductAttributeDto>> CreateProductAttribute(string merchantName, Guid variantId);
    // Task<ProductAttributeDto> GetProductAttribute(Guid variantId, Guid attributeId);
    Task<ProductAttribute> GetProductAttributeById(Guid id);
    Task DeleteProductAttribute(ProductAttribute productAttribute);
}