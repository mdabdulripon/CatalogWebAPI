using AutoMapper;
using Catalog.Core.Dtos;
using Catalog.Core.Entities.Product;
using Catalog.Core.Interfaces;
using Catalog.Core.RequestHelpers;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(CatalogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<Product>> GetAllProducts(ProductParams productParams)
        {
            var query = _context.Products
                .Include(p => p.Variants)
                    .ThenInclude(pv => pv.ProductAttributes)
                .Include(p => p.Variants)
                    .ThenInclude(pv => pv.ProductImages)
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Categories, productParams.Types, productParams.Brands, productParams.Gender, productParams.Size)
                .Where(o => o.MerchantName == productParams.MerchantName)
                .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);
            return products;
        }

        public async Task<Product> GetProduct(Guid id)
        {
            // return await _context.Products
            //     .Include(p => p.Variants)
            //     .ThenInclude(pv => new { pv.ProductAttribute, pv.Images })
            //     .FirstOrDefaultAsync(x => x.Id == id);
            return await _context.Products
                .Include(p => p.Variants)
                    .ThenInclude(pv => pv.ProductAttributes)
                .Include(p => p.Variants)
                    .ThenInclude(pv => pv.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName)
        {
            var products = await _context.Products
                                    .Where(o => o.Category == categoryName)
                                    .ToListAsync();
            return products;
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);

            /*if (product.Variants != null)
            {
                foreach (var variant in product.Variants)
                {
                    _context.ProductVariants.Add(variant);

                    if (variant.ProductImages != null)
                    {
                        foreach (var image in variant.ProductImages)
                        {
                            _context.ProductImages.Add(image);
                        }
                    }
                }
            }*/

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(ProductDto product)
        {
            // Load the existing product from the database
            Product existingProduct = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                return;
            }

            // Use AutoMapper to map the DTO to the entity
            _mapper.Map(product, existingProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Set<Product>().Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveAllAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<FilterDTO> GetFilters(string merchantName)
        {
            var categories = await _context.Products.Where(p => p.MerchantName == merchantName).Select(p => p.Category.ToLower()).Distinct().ToListAsync();
            var brands = await _context.Products.Where(p => p.MerchantName == merchantName).Select(p => p.Brand.ToLower()).Distinct().ToListAsync();
            var gender = await _context.Products.Where(p => p.MerchantName == merchantName).Select(p => p.Gender.ToLower()).Distinct().ToListAsync();
            var types = await _context.Products.Where(p => p.MerchantName == merchantName).Select(p => p.Type.ToLower()).Distinct().ToListAsync();

            var filters = new FilterDTO
            {
                Categories = categories,
                Brands = brands,
                Gender = gender,
                Types = types
            };
            
            return filters;
        }
    }
}

