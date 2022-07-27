using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class RoomInviteDbEntry : BaseEntity
{
    public string FromId { get; set; } = null!;
    public string ToId { get; set; } = null!;
    public int RoomId { get; set; }
    public bool IsAccepted { get; set; }
    public DateTime CreatedOn { get; set; }

    [ForeignKey(nameof(FromId))]
    [InverseProperty(nameof(ApplicationUser.SentRoomInvites))]
    public ApplicationUser From { get; set; } = null!;

    [ForeignKey(nameof(ToId))]
    [InverseProperty(nameof(ApplicationUser.ReceivedRoomInvites))]
    public ApplicationUser To { get; set; } = null!;

    [ForeignKey(nameof(RoomId))]
    [InverseProperty(nameof(RoomDbEntry.Invites))]
    public RoomDbEntry Room { get; set; } = null!;
}