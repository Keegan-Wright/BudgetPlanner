using System.Security.Claims;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Shared.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Server.Services;

public class BaseService : InstrumentedService
{
    private readonly ClaimsPrincipal _user;
    private readonly BudgetPlannerDbContext _budgetPlannerDbContext;

    public BaseService(ClaimsPrincipal user, BudgetPlannerDbContext budgetPlannerDbContext)
    {
        _user = user;
        _budgetPlannerDbContext = budgetPlannerDbContext;
    }
    
    protected string Username => _user.Identity.Name;
    protected Guid UserId => Guid.Parse(_user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
}