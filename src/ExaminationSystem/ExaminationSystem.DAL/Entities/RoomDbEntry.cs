using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class RoomDbEntry : BaseEntity
{
    [Required] public string Title { get; set; } = null!;

    [Required] public string ImageUrl { get; set; } = null!;

    [Required] public string Description { get; set; } = null!;

    [Required] public string OwnerId { get; set; } = null!;

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(ApplicationUser.OwnedRooms))]
    public virtual ApplicationUser Owner { get; set; } = null!;

    [InverseProperty(nameof(ExamDbEntry.Room))]
    public virtual ICollection<ExamDbEntry> Exams { get; set; } = new HashSet<ExamDbEntry>();

    [InverseProperty(nameof(RoomUserDbEntry.Room))]
    public virtual ICollection<RoomUserDbEntry> Members { get; set; } = new HashSet<RoomUserDbEntry>();

    [InverseProperty(nameof(RoomMessageDbEntry.Room))]
    public virtual ICollection<RoomMessageDbEntry> UserMessages { get; set; } = new HashSet<RoomMessageDbEntry>();

    [InverseProperty(nameof(RoomInviteDbEntry.Room))]
    public virtual ICollection<RoomInviteDbEntry> Invites { get; set; } = new HashSet<RoomInviteDbEntry>();
}