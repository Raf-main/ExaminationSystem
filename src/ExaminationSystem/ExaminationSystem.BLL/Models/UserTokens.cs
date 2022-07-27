namespace ExaminationSystem.BLL.Models;

public class UserTokens
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpirationDate { get; set; }
}