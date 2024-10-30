using System.Text.Json;
using Catalog.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data
{
    public static class CatalogDbInitializer
    {

        public static async Task SeedData(CatalogContext context)
        {
            // find if there is any product available just return
            if (await context.Products.AnyAsync()) return;

            // Read Data from file 
            var data = await File.ReadAllTextAsync("productsSeedData.json");

            // PropertyNameCaseInsensitive
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // deserialize the data 
            var products = JsonSerializer.Deserialize<List<Product>>(data);

            foreach (var product in products)
            {
                product.Created = DateTime.SpecifyKind(product.Created, DateTimeKind.Utc);
                product.LastUpdate = DateTime.SpecifyKind(product.LastUpdate, DateTimeKind.Utc);
                await context.Products.AddAsync(product);
            }
            await context.SaveChangesAsync();
        }
    }
}
