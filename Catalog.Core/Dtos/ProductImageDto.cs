namespace Catalog.Core.Dtos
{
	public class ProductImageDto
	{
        public Guid? Id { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsMain { get; set; } = false;
		public Guid ProductVariantId { get; set; }
    }
}
