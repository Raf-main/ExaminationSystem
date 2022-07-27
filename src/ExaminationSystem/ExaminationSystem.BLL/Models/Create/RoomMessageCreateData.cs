namespace ExaminationSystem.BLL.Models.Create;

public class RoomMessageCreateData
{
    public string Text { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string UserId { get; set; } = null!;
    public int RoomId { get; set; }
}