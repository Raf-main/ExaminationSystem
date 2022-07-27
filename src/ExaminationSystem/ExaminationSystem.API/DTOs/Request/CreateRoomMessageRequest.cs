namespace ExaminationSystem.API.DTOs.Request;

public class CreateRoomMessageRequest
{
    public string Text { get; set; } = null!;
    public int RoomId { get; set; }
}