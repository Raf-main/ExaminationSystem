namespace ExaminationSystem.BLL.Models;

public class Invite
{
    public bool IsAccepted { get; set; }
    public DateTime CreatedOn { get; set; }
    public User From { get; set; } = null!;
    public User To { get; set; } = null!;
    public Room Room { get; set; } = null!;
}