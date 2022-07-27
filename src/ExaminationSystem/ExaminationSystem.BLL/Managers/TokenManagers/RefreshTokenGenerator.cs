using System.Security.Cryptography;
using ExaminationSystem.BLL.Managers.TokenManagers.Interfaces;
using ExaminationSystem.BLL.Managers.TokenManagers.Options;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.DAL.Entities;
using Microsoft.Extensions.Options;

namespace ExaminationSystem.BLL.Managers.TokenManagers;

internal class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly JwtOptions _jwtOptions;

    public RefreshTokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public UserRefreshToken GenerateUserRefreshToken(ApplicationUser user, DateTime? utcExpirationDate = null)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        utcExpirationDate ??= DateTime.UtcNow.AddHours(_jwtOptions.RefreshTokenExpirationTimeInHours);

        return new UserRefreshToken
        {
            User = user,
            UserId = user.Id,
            Token = GenerateRefreshToken(),
            UtcExpirationDate = utcExpirationDate.Value
        };
    }
}