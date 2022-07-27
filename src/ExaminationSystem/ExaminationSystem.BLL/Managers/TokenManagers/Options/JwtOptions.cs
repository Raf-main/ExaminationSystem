using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.BLL.Managers.TokenManagers.Options;

public class JwtOptions
{
    public string ValidIssuer { get; set; } = null!;
    public string ValidAudience { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public int TokenExpirationTimeInMinutes { get; set; }
    public int RefreshTokenExpirationTimeInHours { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
    }
}