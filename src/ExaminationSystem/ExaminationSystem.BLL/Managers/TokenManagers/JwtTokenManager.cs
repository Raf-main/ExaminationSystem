using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ExaminationSystem.BLL.Managers.TokenManagers.Interfaces;
using ExaminationSystem.BLL.Managers.TokenManagers.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.BLL.Managers.TokenManagers;

internal class JwtTokenManager : ITokenManager
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenManager(IOptions<JwtOptions> jwtOptions, IAccessTokenGenerator? accessTokenGenerator,
        IRefreshTokenGenerator? refreshTokenGenerator)
    {
        AccessTokenGenerator = accessTokenGenerator ?? new JwtAccessTokenGenerator(jwtOptions);
        RefreshTokenGenerator = refreshTokenGenerator ?? new RefreshTokenGenerator(jwtOptions);
        _jwtOptions = jwtOptions.Value;
    }

    public IAccessTokenGenerator AccessTokenGenerator { get; }
    public IRefreshTokenGenerator RefreshTokenGenerator { get; }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = _jwtOptions.ValidAudience,
            ValidIssuer = _jwtOptions.ValidIssuer,
            RequireExpirationTime = true,
            ValidateLifetime = false,
            IssuerSigningKey = _jwtOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token or refresh token");
        }

        return principal;
    }
}