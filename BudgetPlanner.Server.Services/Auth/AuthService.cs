using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetPlanner.Server.Data.Db;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BudgetPlanner.Server.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly BudgetPlannerDbContext _budgetPlannerDbContext;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> userManager, BudgetPlannerDbContext budgetPlannerDbContext, IConfiguration configuration)
    {
        _userManager = userManager;
        _budgetPlannerDbContext = budgetPlannerDbContext;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _budgetPlannerDbContext.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.UserName.ToUpper() == request.Username.ToUpper());
        
        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        if (passwordIsCorrect)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user);
            
            return new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Id,
                Success = true
            };

        }
        else
        {
            return new LoginResponse()
            {
                Success = false,
                Errors = ["Invalid username or password."]
            };
        }
                
    }
    
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var newUser = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        if (request.Password != request.ConfirmPassword)
            return new RegisterResponse { Success = false };
            
            
        var userResult = await _userManager.CreateAsync(newUser, request.Password);

        return new RegisterResponse() { Success = userResult.Succeeded };
    }

    public async Task LogoutAsync(ClaimsPrincipal contextUser)
    {
        var user = await _userManager.FindByNameAsync(contextUser.Identity.Name);
        await _budgetPlannerDbContext.RefreshTokens.Where(x => x.UserId == user.Id && ((!x.Consumed && x.Revoked) || (!x.Revoked && !x.Consumed)))
            .ExecuteUpdateAsync(x => x.SetProperty(c => c.Revoked, true));
        
    }

    public async Task<TokenResponse> ProcessRefreshTokenAsync(TokenRequest request, ClaimsPrincipal contextUser)
    {
        var refreshToken = await _budgetPlannerDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Id == request.RefreshToken);
        var user = await _budgetPlannerDbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == refreshToken.UserId);

        //if (refreshToken.UserId == user.Id && user.UserName == contextUser.Identity.Name)
        //{
            refreshToken.Consumed = true;
            await _budgetPlannerDbContext.SaveChangesAsync();
            
            var accessToken = GenerateAccessToken(user);
            var newRefreshToken = await GenerateRefreshTokenAsync(user);

            return new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Id,
            };
        //}
        return null;
    }
    
    private async Task<RefreshToken> GenerateRefreshTokenAsync(ApplicationUser user)
    {
        var refreshToken = new RefreshToken()
        {
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddHours(24),
            Issued = DateTime.UtcNow,
            UserId = user.Id,
            Revoked = false,
            Consumed = false
        };
     
        user.RefreshTokens.Add(refreshToken);
        
        await _budgetPlannerDbContext.SaveChangesAsync();
        return refreshToken;
    }

    private string GenerateAccessToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
        };
        
        var authIssuer = _configuration["AUTH_ISSUER"];
        var authAudience = _configuration["AUTH_AUDIENCE"];
        var authSigningKey = _configuration["AUTH_SIGNING_KEY"];
        var encodedKey = Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(authSigningKey));
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow.AddMinutes(-5),
            NotBefore = DateTime.UtcNow.AddMinutes(-5),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = authIssuer,
            Audience = authAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256),
        };

        var accessToken =  tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        return accessToken;
    }

}