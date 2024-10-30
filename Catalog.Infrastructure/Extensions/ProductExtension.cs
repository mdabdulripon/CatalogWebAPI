using Catalog.Core.Entities.Product;

namespace Catalog.Infrastructure.Extensions
{
    public static class ProductExtension
    {
        public static IQueryable<Product> Sort(this IQueryable<Product> query, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Name);

            query = orderBy switch
            {
                "date" => query.OrderBy(p => p.Variants.Min(v=> v.Created)),
                "dateDesc" => query.OrderByDescending(p => p.Variants.Min(v => v.Created)),
                _ => query.OrderBy(p => p.Name)
            };
            return query;
        }

        public static IQueryable<Product> Search(this IQueryable<Product> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<Product> Filter(this IQueryable<Product> query, string categories, string types, string brands, string gender, string size)
        {
            var categoryList = new List<string>();
            var typeList = new List<string>();
            var brandList = new List<string>();
            var genderList = new List<string>();
            var sizeList = new List<string>();

            if (!string.IsNullOrEmpty(categories))
            {
                categoryList.AddRange(categories.ToLower().Split(","));
            }

            if (!string.IsNullOrEmpty(types))
            {
                typeList.AddRange(types.ToLower().Split(","));
            }

            if (!string.IsNullOrEmpty(brands))
            {
                brandList.AddRange(brands.ToLower().Split(","));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                genderList.AddRange(gender.ToLower().Split(","));
            }

            if (!string.IsNullOrEmpty(size))
            {
                sizeList.AddRange(size.ToLower().Split(","));
            }

            query = query
                .Where(p => categoryList.Count == 0 || categoryList.Contains(p.Category.ToLower()))
                .Where(p => typeList.Count == 0 || typeList.Contains(p.Type.ToLower()))
                .Where(p => brandList.Count == 0 || brandList.Contains(p.Brand.ToLower()))
                .Where(p => genderList.Count == 0 || genderList.Contains(p.Gender.ToLower()));
                //.Where(p => sizeList.Count == 0 || p.Variants.Any(v => sizeList.Contains(v.Size.ToLower())));

            return query;
        }
    }
}
