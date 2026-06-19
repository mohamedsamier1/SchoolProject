using Microsoft.EntityFrameworkCore;

namespace SchoolProject.Core.Wrappers
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pagesize) where T : class
        {
            if (source == null)
            {
                throw new Exception("Empty");
            }
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pagesize = pagesize == 0 ? 10 : pagesize;
            int count = await source.AsNoTracking().CountAsync();
            if (count == 0) return PaginatedResult<T>.Success(new List<T>(), count, pageNumber, pagesize);
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            var items = await source.Skip((pageNumber - 1) * pagesize).Take(pagesize).ToListAsync();
            return PaginatedResult<T>.Success(items, count, pageNumber, pagesize);
        }
    }
}
