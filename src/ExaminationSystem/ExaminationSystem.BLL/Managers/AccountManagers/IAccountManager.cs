using System.Security.Claims;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.BLL.Models.Create;
using ExaminationSystem.DAL.Entities;

namespace ExaminationSystem.BLL.Managers.AccountManagers;

public interface IAccountManager
{
    Task<LoginResult> LoginAsync(UserLoginData userData);
    Task RegisterAsync(UserCreateData userData, string emailConfirmationAction);
    Task<UserTokens> RefreshTokenAsync(UserTokens tokens);
    Task ConfirmEmailAsync(ConfirmEmail confirmEmailData);
    ClaimsPrincipal? GetCurrentUser();
    string? GetCurrentUserClaim(string claimKey);
    Task<ApplicationUser?> FindUserByEmailAsync(string email);
    Task<ApplicationUser?> FindUserByIdAsync(string id);
}