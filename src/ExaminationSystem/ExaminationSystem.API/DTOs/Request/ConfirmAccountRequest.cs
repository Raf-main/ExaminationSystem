namespace ExaminationSystem.API.DTOs.Request;

public class ConfirmAccountRequest
{
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
}