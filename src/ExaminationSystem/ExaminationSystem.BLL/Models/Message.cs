namespace ExaminationSystem.BLL.Models;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
}