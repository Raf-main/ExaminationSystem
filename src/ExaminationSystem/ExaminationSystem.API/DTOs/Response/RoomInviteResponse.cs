namespace ExaminationSystem.API.DTOs.Response;

public class RoomInviteResponse
{
    public DateTime CreatedOn { get; set; }
    public bool IsAccepted { get; set; }
    public string FromId { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
    public string ToId { get; set; } = null!;
    public string ToEmail { get; set; } = null!;
    public int RoomId { get; set; }
}