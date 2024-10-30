namespace Catalog.Core.Dtos
{
    public class ProductDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string MerchantName { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Gender { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public ICollection<ProductVariantDto> Variants { get; set; }
    }
}
