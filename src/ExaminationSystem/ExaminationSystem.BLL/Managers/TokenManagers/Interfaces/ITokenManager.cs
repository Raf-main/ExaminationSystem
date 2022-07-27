using System.Security.Claims;

namespace ExaminationSystem.BLL.Managers.TokenManagers.Interfaces;

public interface ITokenManager
{
    IAccessTokenGenerator AccessTokenGenerator { get; }
    IRefreshTokenGenerator RefreshTokenGenerator { get; }
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}