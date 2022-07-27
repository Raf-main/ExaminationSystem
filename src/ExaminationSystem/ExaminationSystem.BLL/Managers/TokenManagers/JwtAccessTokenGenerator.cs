using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ExaminationSystem.BLL.Managers.TokenManagers.Interfaces;
using ExaminationSystem.BLL.Managers.TokenManagers.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.BLL.Managers.TokenManagers;

internal class JwtAccessTokenGenerator : IAccessTokenGenerator
{
    private readonly JwtOptions _jwtOptions;

    public JwtAccessTokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = _jwtOptions.GetSymmetricSecurityKey();
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            _jwtOptions.ValidIssuer,
            _jwtOptions.ValidAudience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationTimeInMinutes),
            signingCredentials: signinCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }
}