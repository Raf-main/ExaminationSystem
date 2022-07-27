using System.Security.Claims;

namespace ExaminationSystem.BLL.Managers.TokenManagers.Interfaces;

public interface IAccessTokenGenerator
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
}