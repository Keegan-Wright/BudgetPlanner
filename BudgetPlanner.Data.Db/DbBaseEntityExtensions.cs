using BudgetPlanner.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Data.Db
{
    public static class DbBaseEntityExtensions
    {
        public static async IAsyncEnumerable<TEntityType> GetPagedEntitiesAsync<TEntityType>(this IQueryable<TEntityType> entities, int pageSize, int currentPage = 0, int? maxPages = null)
        {
            var totalCount = await entities.CountAsync();
            var pagesToRun = totalCount / (currentPage + 1 * pageSize);

            while (Math.Max(1,pagesToRun) >= currentPage + 1)
            {
                if (currentPage >= maxPages)
                {
                    yield break;
                }

                await foreach (var entity in entities
                                            .Skip(currentPage * pageSize)
                                            .Take(pageSize)
                                            .AsAsyncEnumerable())
                {
                    yield return entity;
                }
                currentPage++;

            }

        }
    }

}
