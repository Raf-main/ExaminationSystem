using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;
using ExaminationSystem.DAL.Helpers;

namespace ExaminationSystem.DAL.Entities;

public class ExamDbEntry : BaseEntity
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public ExamStatus Status { get; set; }
    public TimeSpan Duration { get; set; }

    public DateTime? StartTime { get; set; }
    public int RoomId { get; set; }

    [ForeignKey(nameof(RoomId))]
    [InverseProperty(nameof(RoomDbEntry.Exams))]
    public virtual RoomDbEntry Room { get; set; } = null!;

    [InverseProperty(nameof(Question.Exam))]
    public virtual ICollection<Question> Questions { get; set; } = new HashSet<Question>();

    [InverseProperty(nameof(ExamResultDbEntry.Exam))]
    public virtual ICollection<ExamResultDbEntry> ExamResults { get; set; } = new HashSet<ExamResultDbEntry>();
}