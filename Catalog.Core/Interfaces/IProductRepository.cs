using Catalog.Core.Dtos;
using Catalog.Core.Entities.Product;
using Catalog.Core.RequestHelpers;

namespace Catalog.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetAllProducts(ProductParams productParams);
        Task<Product> GetProduct(Guid id);
        Task<IEnumerable<Product>> GetProductsByCategory(string categoryName);
        Task CreateProduct(Product product);
        Task UpdateProduct(ProductDto product);
        Task DeleteProduct(Product product);
        Task<bool> SaveAllAsync(Product product);
        Task<FilterDTO> GetFilters(string merchantName);
    }
}
