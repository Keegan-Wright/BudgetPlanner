using BudgetPlanner.Shared.Models.Request.HouseholdMember;

namespace BudgetPlanner.Client.Services.HouseholdMember;

public class HouseholdMemberRequestService : BaseRequestService, IHouseholdMemberRequestService
{
    public HouseholdMemberRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public override string BaseRoute { get; init; }
    public IAsyncEnumerable<Data.Models.HouseholdMember> GetHouseholdMembersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Data.Models.HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest categoryToAdd)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteHouseholdMemberAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}