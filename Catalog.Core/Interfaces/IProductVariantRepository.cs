using Catalog.Core.Entities.Product;

namespace Catalog.Core.Interfaces
{
    public interface IProductVariantRepository
    {
        Task<IEnumerable<ProductVariant>> GetProductVariants(Guid productId);
        Task<ProductVariant> GetProductVariant(Guid variantId);
        Task DeleteProductVariant(ProductVariant productVariant);
    }
}