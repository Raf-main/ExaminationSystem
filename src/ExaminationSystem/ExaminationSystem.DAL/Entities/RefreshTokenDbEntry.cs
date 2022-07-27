using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class RefreshTokenDbEntry : BaseEntity
{
    public string Token { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime UtcExpirationTime { get; set; }

    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(ApplicationUser.RefreshTokens))]
    public virtual ApplicationUser User { get; set; } = null!;
}