using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Shared.Models.Request.Classifications;
using BudgetPlanner.Shared.Models.Response.Classifications;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services.Classifications
{
    public class ClassificationService : IClassificationService
    {
        private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
        public ClassificationService(BudgetPlannerDbContext budgetPlannerDbContext)
        {
            _budgetPlannerDbContext = budgetPlannerDbContext;
        }

        public async Task<ClassificationsResponse> AddCustomClassificationAsync(AddClassificationsRequest classification)
        {
            var newClassification = new CustomClassification() { Tag = classification.Tag };

            var strategy = _budgetPlannerDbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _budgetPlannerDbContext.Database.BeginTransactionAsync();
                await _budgetPlannerDbContext.CustomClassifications.AddAsync(newClassification);
                await _budgetPlannerDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            });
            

            return new ClassificationsResponse()
            {
                Tag = classification.Tag,
                ClassificationId = newClassification.Id
            };

        }


        public async IAsyncEnumerable<ClassificationsResponse> GetAllCustomClassificationsAsync()
        {
            var query = GetMainClassificationQuery();
            var classifications = GetMainClassificationsSelect(query).AsAsyncEnumerable();

            await foreach (var classification in classifications)
            {
                yield return classification;
            }
        }

        public async Task<GetClassificationResponse> GetClassificationAsync(Guid id)
        {
            var item = await _budgetPlannerDbContext.CustomClassifications.FirstAsync(x => x.Id == id);
            return new GetClassificationResponse()
            {
                ClassificationId = item.Id,
                Tag = item.Tag
            };
        }

        private IQueryable<ClassificationsResponse> GetMainClassificationsSelect(IQueryable<CustomClassification> query)
        {
            return query.Select(x => new ClassificationsResponse()
            {
                Tag = x.Tag,
                ClassificationId = x.Id
            });
        }

        private IQueryable<CustomClassification> GetMainClassificationQuery()
        {
            return _budgetPlannerDbContext.CustomClassifications
                .AsNoTracking().AsQueryable();
        }

        public async Task AddCustomClassificationsToTransactionAsync(AddCustomClassificationsToTransactionRequest requestModel)
        {
            var transaction = await _budgetPlannerDbContext.OpenBankingTransactions.FirstOrDefaultAsync(x => x.Id == requestModel.TransactionId);
            var classifications = await _budgetPlannerDbContext.CustomClassifications.Where(x => requestModel.Classifications.Select(x => x.ClassificationId).Contains(x.Id)).ToListAsync();

            var newClassifications = new List<OpenBankingTransactionClassifications>();
            foreach (var classification in classifications)
            {
                var newClassification = new OpenBankingTransactionClassifications()
                {
                    Transaction = transaction,
                    TransactionId = transaction.Id,
                    Classification = classification.Tag,
                    IsCustomClassification = true
                };
                newClassifications.Add(newClassification);
            }

            await _budgetPlannerDbContext.AddRangeAsync(newClassifications);
            await _budgetPlannerDbContext.SaveChangesAsync();
        }

        public async Task RemoveCustomClassificationAsync(Guid id)
        {
            var strategy = _budgetPlannerDbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _budgetPlannerDbContext.Database.BeginTransactionAsync();

                var classificationToRemove =
                    await _budgetPlannerDbContext.CustomClassifications.FirstAsync(x => x.Id == id);
                var transactionClassifications = await _budgetPlannerDbContext.OpenBankingTransactionClassifications
                    .Where(x => x.IsCustomClassification == true && x.Classification == classificationToRemove.Tag)
                    .ToListAsync();

                await _budgetPlannerDbContext.BulkDeleteAsync(transactionClassifications);
                await _budgetPlannerDbContext.BulkDeleteAsync([classificationToRemove]);

                await transaction.CommitAsync();
            });

        }
    }
}
