using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class MultipleQuestionOptionAnswerDbEntry : BaseEntity
{
    public int AnswerId;
    public int OptionId;

    [ForeignKey(nameof(AnswerId))]
    [InverseProperty(nameof(MultipleQuestionAnswerDbEntry.ChosenAnswers))]
    public MultipleQuestionAnswerDbEntry Answer { get; set; } = null!;

    [ForeignKey(nameof(OptionId))]
    [InverseProperty(nameof(MultipleQuestionOptionDbEntry.Answers))]
    public MultipleQuestionOptionDbEntry Option { get; set; } = null!;
}