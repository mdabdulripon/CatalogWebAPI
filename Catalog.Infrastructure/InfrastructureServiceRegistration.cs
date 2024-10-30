using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddDbContext<CatalogContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("dbConnectionString")));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
            services.AddScoped<IProductImagesRepository, ProductImagesRepository>();
            return services;
        }
    }
}