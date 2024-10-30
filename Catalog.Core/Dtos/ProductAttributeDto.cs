namespace Catalog.Core.Dtos
{
    public class ProductAttributeDto
    {
        public Guid? Id { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}