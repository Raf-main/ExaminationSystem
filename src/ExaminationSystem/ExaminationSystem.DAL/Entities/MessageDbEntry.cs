using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class MessageDbEntry : BaseEntity
{
    public string Text { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    [InverseProperty(nameof(RoomMessageDbEntry.Message))]
    public virtual ICollection<RoomMessageDbEntry> RoomMessages { get; set; } = new HashSet<RoomMessageDbEntry>();
}