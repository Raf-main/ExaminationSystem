using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.DAL.Entities;

public class ApplicationUser : IdentityUser
{
    [InverseProperty(nameof(RoomUserDbEntry.User))]
    public virtual ICollection<RoomUserDbEntry> Rooms { get; set; } = new HashSet<RoomUserDbEntry>();

    [InverseProperty(nameof(RefreshTokenDbEntry.User))]
    public virtual ICollection<RefreshTokenDbEntry> RefreshTokens { get; set; } = new HashSet<RefreshTokenDbEntry>();

    [InverseProperty(nameof(RoomDbEntry.Owner))]
    public virtual ICollection<RoomDbEntry> OwnedRooms { get; set; } = new HashSet<RoomDbEntry>();

    [InverseProperty(nameof(ExamResultDbEntry.User))]
    public virtual ICollection<ExamResultDbEntry> ExamResults { get; set; } = new HashSet<ExamResultDbEntry>();

    [InverseProperty(nameof(RoomMessageDbEntry.User))]
    public virtual ICollection<RoomMessageDbEntry> RoomMessages { get; set; } = new HashSet<RoomMessageDbEntry>();

    [InverseProperty(nameof(RoomInviteDbEntry.To))]
    public virtual ICollection<RoomInviteDbEntry> ReceivedRoomInvites { get; set; } = new HashSet<RoomInviteDbEntry>();

    [InverseProperty(nameof(RoomInviteDbEntry.From))]
    public virtual ICollection<RoomInviteDbEntry> SentRoomInvites { get; set; } = new HashSet<RoomInviteDbEntry>();
}
