using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class MultipleQuestionDbEntry : Question
{
    [InverseProperty(nameof(MultipleQuestionOptionDbEntry.Question))]
    public virtual ICollection<MultipleQuestionOptionDbEntry> Options { get; set; } =
        new HashSet<MultipleQuestionOptionDbEntry>();

    [InverseProperty(nameof(MultipleQuestionAnswerDbEntry.MultipleQuestion))]
    public virtual ICollection<MultipleQuestionAnswerDbEntry> Answers { get; set; } =
        new HashSet<MultipleQuestionAnswerDbEntry>();
}