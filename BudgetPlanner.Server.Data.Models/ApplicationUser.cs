using Microsoft.AspNetCore.Identity;

namespace BudgetPlanner.Server.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public ICollection<OpenBankingProvider> Providers { get; set; }
    public ICollection<CustomClassification> CustomClassifications { get; set; }
    public ICollection<HouseholdMember> HouseholdMembers { get; set; }
    public ICollection<Debt> Debts { get; set; }
    public ICollection<BudgetCategory> BudgetCategories { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}