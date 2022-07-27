namespace ExaminationSystem.BLL.Models.Create;

public class RoomCreateData
{
    public string OwnerId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
}