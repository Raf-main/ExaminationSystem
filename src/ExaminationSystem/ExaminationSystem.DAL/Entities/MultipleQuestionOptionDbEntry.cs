using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class MultipleQuestionOptionDbEntry : BaseEntity
{
    public bool IsCorrect { get; set; }
    public string Description { get; set; } = null!;
    public int QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    [InverseProperty(nameof(MultipleQuestionDbEntry.Options))]
    public virtual MultipleQuestionDbEntry Question { get; set; } = null!;

    [InverseProperty(nameof(MultipleQuestionOptionAnswerDbEntry.Option))]
    public virtual ICollection<MultipleQuestionOptionAnswerDbEntry> Answers { get; set; } =
        new HashSet<MultipleQuestionOptionAnswerDbEntry>();
}