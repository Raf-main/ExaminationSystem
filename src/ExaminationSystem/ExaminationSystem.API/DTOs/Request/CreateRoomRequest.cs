namespace ExaminationSystem.API.DTOs.Request;

public class CreateRoomRequest
{
    public string Title { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Description { get; set; } = null!;
}