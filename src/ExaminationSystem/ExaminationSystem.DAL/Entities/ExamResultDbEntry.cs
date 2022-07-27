using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class ExamResultDbEntry : BaseEntity
{
    public string UserId { get; set; } = null!;
    public int ExamId { get; set; }
    public int NumberOfCorrectAnswers { get; set; }

    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(ApplicationUser.ExamResults))]
    public virtual ApplicationUser User { get; set; } = null!;

    [ForeignKey(nameof(ExamId))]
    [InverseProperty(nameof(ExamDbEntry.ExamResults))]
    public virtual ExamDbEntry Exam { get; set; } = null!;
}