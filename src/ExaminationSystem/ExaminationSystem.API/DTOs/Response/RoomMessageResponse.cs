namespace ExaminationSystem.API.DTOs.Response;

public class RoomMessageResponse
{
    public string Text { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string UserId { get; set; } = null!;
    public int RoomId { get; set; }
    public int MessageId { get; set; }
}