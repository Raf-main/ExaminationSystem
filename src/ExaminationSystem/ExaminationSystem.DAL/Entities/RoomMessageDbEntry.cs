using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class RoomMessageDbEntry : Entity
{
    public string UserId { get; set; } = null!;
    public int RoomId { get; set; }
    public int MessageId { get; set; }

    [ForeignKey(nameof(RoomId))]
    [InverseProperty(nameof(RoomDbEntry.UserMessages))]
    public RoomDbEntry Room { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(ApplicationUser.RoomMessages))]
    public ApplicationUser User { get; set; } = null!;

    [ForeignKey(nameof(MessageId))]
    [InverseProperty(nameof(MessageDbEntry.RoomMessages))]
    public MessageDbEntry Message { get; set; } = null!;
}