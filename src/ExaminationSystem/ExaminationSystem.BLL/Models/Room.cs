namespace ExaminationSystem.BLL.Models;

public class Room
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string OwnerId { get; set; } = null!;
}