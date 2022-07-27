namespace ExaminationSystem.BLL.Models;

public class ConfirmEmail
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}