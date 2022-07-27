using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;
using ExaminationSystem.DAL.Helpers;

namespace ExaminationSystem.DAL.Entities;

public class RoomUserDbEntry : Entity
{
    public string UserId { get; set; } = null!;
    public int RoomId { get; set; }
    public RoomPermission Permission { get; set; }

    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(ApplicationUser.Rooms))]
    public ApplicationUser User { get; set; } = null!;

    [ForeignKey(nameof(RoomId))]
    [InverseProperty(nameof(RoomDbEntry.Members))]
    public RoomDbEntry Room { get; set; } = null!;
}