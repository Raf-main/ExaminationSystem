using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class MultipleQuestionAnswerDbEntry : BaseEntity
{
    public int MultipleQuestionId { get; set; }

    [ForeignKey(nameof(MultipleQuestionId))]
    [InverseProperty(nameof(MultipleQuestionDbEntry.Answers))]
    public virtual MultipleQuestionDbEntry MultipleQuestion { get; set; } = null!;

    [InverseProperty(nameof(MultipleQuestionOptionAnswerDbEntry.Answer))]
    public virtual ICollection<MultipleQuestionOptionAnswerDbEntry> ChosenAnswers { get; set; } =
        new HashSet<MultipleQuestionOptionAnswerDbEntry>();
}