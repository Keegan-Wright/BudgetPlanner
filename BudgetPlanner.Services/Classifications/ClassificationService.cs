using BudgetPlanner.Data.Db;
using BudgetPlanner.Data.Models;
using BudgetPlanner.Models.Request.Classifications;
using BudgetPlanner.Models.Response.Classifications;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Services.Classifications
{
    public class ClassificationService : IClassificationService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        public ClassificationService(BudgetPlannerDbContext budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
        }

        public Task<ClassificationsResponse> AddCustomClassificationAsync(AddClassificationsRequest classification)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<ClassificationsResponse> GetAllClassificationsAsync()
        {
            IQueryable<OpenBankingTransactionClassifications> query = GetMainClassificationQuery();

            var classifications = GetMainClassificationsSelect(query).AsAsyncEnumerable();

            await foreach (var classification in classifications)
            {
                yield return classification;
            }
        }


        public async IAsyncEnumerable<ClassificationsResponse> GetAllCustomClassificationsAsync()
        {
            IQueryable<OpenBankingTransactionClassifications> query = GetMainClassificationQuery();
            query = query.Where(x => x.IsCustomClassification == true);
            var classifications = GetMainClassificationsSelect(query).AsAsyncEnumerable();

            await foreach (var classification in classifications)
            {
                yield return classification;
            }
        }

        public async IAsyncEnumerable<ClassificationsResponse> GetAllExternalClassificationsAsync()
        {
            IQueryable<OpenBankingTransactionClassifications> query = GetMainClassificationQuery();
            query = query.Where(x => x.IsCustomClassification != true);
            var classifications = GetMainClassificationsSelect(query).AsAsyncEnumerable();

            await foreach (var classification in classifications)
            {
                yield return classification;
            }
        }

        public async Task<GetClassificationResponse> GetClassificationAsync(Guid id)
        {
            var item = await _budgetPlannerDbContext.OpenBankingTransactionClassifications.FirstAsync(x => x.Id == id);
            return new GetClassificationResponse()
            {
                ClassificationId = item.Id,
                Classification = item.Classification
            };
        }

        private IQueryable<ClassificationsResponse> GetMainClassificationsSelect(IQueryable<OpenBankingTransactionClassifications> query)
        {
            return query.Select(x => new ClassificationsResponse()
            {
                Classification = x.Classification,
                IsCustomClassification = x.IsCustomClassification
            });
        }

        private IQueryable<OpenBankingTransactionClassifications> GetMainClassificationQuery()
        {
            return _budgetPlannerDbContext.OpenBankingTransactionClassifications
                .AsNoTracking()
                .DistinctBy(x => x.Classification).AsQueryable();
        }

    }
}
