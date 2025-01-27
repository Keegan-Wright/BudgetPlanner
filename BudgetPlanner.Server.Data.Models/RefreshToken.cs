namespace BudgetPlanner.Server.Data.Models;

public class RefreshToken : BaseEntity
{
    public required DateTime Issued { get; set; }
    public required DateTime Expires { get; set; }
    public required Guid UserId { get; set; }
    public required bool Revoked { get; set; }
    public required bool Consumed { get; set; }
}