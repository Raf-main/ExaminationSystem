using ExaminationSystem.DAL.Entities;

namespace ExaminationSystem.BLL.Models;

public class UserRefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime UtcExpirationDate { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;
}