
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.RequestHelpers
{
    public class PagedList<T> : List<T>
    {

        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var count = await query.CountAsync();

            /**
            ** * If pageNumber = 1, and pageSize = 10 it will skip nothing. EX: (1 - 1) * 10 = 0
            ** * If pageNumber = 2, and pageSize = 10 it will skip first 10 items. EX: (2 - 1) * 10 = 10
            ** * If pageNumber = 3, and pageSize = 10 it will skip first and second 10 items (20). EX: (3 - 1) * 10 = 20
            ****/
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

    }
}