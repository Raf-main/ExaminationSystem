namespace ExaminationSystem.API.DTOs.Response;

public class LoginResponse
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
}