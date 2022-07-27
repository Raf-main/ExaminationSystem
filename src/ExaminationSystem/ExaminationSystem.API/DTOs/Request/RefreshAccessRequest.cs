namespace ExaminationSystem.API.DTOs.Request;

public class RefreshAccessRequest
{
    public string ExpiredToken { get; set; } = null!;
}