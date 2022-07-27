namespace ExaminationSystem.API.DTOs.Response;

public class TokenResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpirationDate { get; set; }
}