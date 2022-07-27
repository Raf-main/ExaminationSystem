namespace ExaminationSystem.API.DTOs.Response;

public class RoomResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Description { get; set; } = null!;
}