using AutoMapper;
using Catalog.Core.Dtos;
using Catalog.Core.Entities.Product;

namespace Catalog.API.Mapping
{
	public class CatalogProfile : Profile
    {
		public CatalogProfile()
		{
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductVariantDto, ProductVariant>().ReverseMap();
            CreateMap<ProductAttributeDto, ProductAttribute>().ReverseMap();
            CreateMap<ProductImageDto, ProductImage>().ReverseMap();
        }
    }
}

