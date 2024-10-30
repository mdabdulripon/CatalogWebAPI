using AutoMapper;
using Catalog.Core.Entities.Product;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Catalog.Infrastructure.Repositories;

public class ProductAttributeRepository : IProductAttributeRepository
{
    
    private readonly IConfiguration _configuration;
    private readonly CatalogContext _context;
    private readonly IMapper _mapper;

    public ProductAttributeRepository(IConfiguration configuration, CatalogContext context, IMapper mapper)
    {
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductAttribute> GetProductAttributeById(Guid id)
    {
        return await _context.ProductAttribute.FirstOrDefaultAsync(p => p.Id == id);
        // return await _context.ProductAttribute
        //     .FirstOrDefaultAsync(pv => pv.Id == id);
    }

    public async Task DeleteProductAttribute(ProductAttribute productAttribute)
    {
        _context.Set<ProductAttribute>().Remove(productAttribute);
        await _context.SaveChangesAsync();
    }
}