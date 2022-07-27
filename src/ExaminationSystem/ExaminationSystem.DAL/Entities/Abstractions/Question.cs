using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Entities.Abstractions;

public abstract class Question : BaseEntity
{
    public string Description { get; set; } = null!;
    public int ExamId { get; set; }

    [ForeignKey(nameof(ExamId))]
    [InverseProperty(nameof(ExamDbEntry.Questions))]
    public virtual ExamDbEntry Exam { get; set; } = null!;
}