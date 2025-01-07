using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetPlanner.Server.Data.Models;
using BudgetPlanner.Shared.Models.Request.Auth;
using BudgetPlanner.Shared.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BudgetPlanner.Server.EndPoints;

public static class AuthEndPointExtensions
{
    public static void MapAuthEndPoint(this WebApplication app)
    {
        var authGroup = app.MapGroup("/Auth");


        authGroup.MapPost("/Login", async (UserManager<ApplicationUser> userManager,LoginRequest request) =>
        {
            var user = await userManager.FindByNameAsync(request.Username);
            //var signInResult = await signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);
           // if (signInResult.Succeeded)
            //{
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                };
                
                
                var authIssuer = app.Configuration.GetValue<string>("AUTH_ISSUER");
                var authAudience = app.Configuration.GetValue<string>("AUTH_AUDIENCE");
                var authSigningKey = app.Configuration.GetValue<string>("AUTH_SIGNING_KEY");
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

                return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            //}
            //return null;
        });

        
        authGroup.MapPost("/Register", async (UserManager<ApplicationUser> userManager, RegisterRequest request) =>
        {
            var newUser = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            
            var userResult = await userManager.CreateAsync(newUser, request.Password);

            return new GenericSuccessResponse { Success = userResult.Succeeded };

        });

        authGroup.MapPost("/Logout", async (SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
        });

        
    }
}