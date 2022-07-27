namespace ExaminationSystem.BLL.Models;

public class LoginResult
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserTokens Tokens { get; set; } = null!;
}