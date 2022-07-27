namespace ExaminationSystem.BLL.Models;

public class RoomMessage
{
    public string UserId { get; set; } = null!;
    public int RoomId { get; set; }
    public Message Message { get; set; } = null!;
}