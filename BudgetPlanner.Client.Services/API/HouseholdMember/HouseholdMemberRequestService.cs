using BudgetPlanner.Shared.Models.Request.HouseholdMember;

namespace BudgetPlanner.Client.Services.HouseholdMember;

public class HouseholdMemberRequestService : BaseRequestService, IHouseholdMemberRequestService
{
    public HouseholdMemberRequestService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        BaseRoute = "householdMember";
    }

    public override string BaseRoute { get; init; }
    public async IAsyncEnumerable<Data.Models.HouseholdMember> GetHouseholdMembersAsync()
    {
        await foreach (var householdMember in GetAsyncEnumerable<Data.Models.HouseholdMember>("GetAll"))
        {
            yield return householdMember;
        }
    }

    public async Task<Data.Models.HouseholdMember> AddHouseholdMemberAsync(AddHouseholdMemberRequest houseHoldMemberToAdd)
    {
        return await PostAsync<AddHouseholdMemberRequest, Data.Models.HouseholdMember>("AddHouseholdMember", houseHoldMemberToAdd);
    }

    public async Task<bool> DeleteHouseholdMemberAsync(Guid id)
    {
        return await DeleteAsync($"DeleteHouseholdMember/{id}");
    }
}