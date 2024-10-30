namespace Catalog.Core.Entities.Product
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MerchantName { get; set; }        // zahins, American-Eagle, H&M 
        public string Category { get; set; }            // Shirt, Pant, Bikes, Cars, Laptop, Mouse Pad
        public string Type { get; set; }                // Apparel & Accessories, Toys, Electronics,  watches, handbag, jewelry, smartwatch 
        public string Brand { get; set; }               // Polo, Adidas, Nike
        public string Gender { get; set; }              // Man, Women, Kids
        public string Summary { get; set; }
        public string Description { get; set; }
        public ICollection<ProductVariant> Variants { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}

