
using Catalog.Core.Dtos;
using Catalog.Core.Entities.Product;
using Microsoft.AspNetCore.Http;

namespace Catalog.Core.Interfaces
{
    public interface IProductImagesRepository
    {
        Task<IList<ProductImageDto>> UploadProductImages(string merchantName, Guid productVariantId, IList<IFormFile> formFiles);
        Task<ProductImageDto> GetProductImage(Guid variantId, string imageUrl);
        Task<ProductImage> GetProductImageById(Guid id);
        Task SetMainProductImage(ProductImage productImage);
        Task DeleteProductImages(ProductImage productImage);
    }
}
