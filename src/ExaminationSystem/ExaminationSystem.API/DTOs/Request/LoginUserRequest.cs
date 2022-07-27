namespace ExaminationSystem.API.DTOs.Request;

public class LoginUserRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}