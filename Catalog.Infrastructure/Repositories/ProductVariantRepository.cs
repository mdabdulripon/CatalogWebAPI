using AutoMapper;
using Catalog.Core.Entities.Product;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly CatalogContext _context;
        private readonly IMapper _mapper;

        public ProductVariantRepository(CatalogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get products variants by product id
        public async Task<IEnumerable<ProductVariant>> GetProductVariants(Guid productId)
        {
            var productVariants = await _context.ProductVariants
                .Include(pv => pv.ProductImages)
                .Where(pv => pv.ProductId == productId)
                .ToListAsync();
            return productVariants;
        }
        
        // Get single variants by variantId
        public async Task<ProductVariant> GetProductVariant(Guid variantId)
        {
         return await _context.ProductVariants
            .Include(pv => pv.ProductAttributes)
            .Include(pv => pv.ProductImages)
            .FirstOrDefaultAsync(pv => pv.Id == variantId);
        }

        public async Task DeleteProductVariant(ProductVariant productVariant)
        {
            _context.Set<ProductVariant>().Remove(productVariant);
            await _context.SaveChangesAsync();
        }
    }
}