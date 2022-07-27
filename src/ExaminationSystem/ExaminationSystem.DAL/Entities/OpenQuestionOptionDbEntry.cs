using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class OpenQuestionOptionDbEntry : BaseEntity
{
    public string TextAnswer { get; set; } = null!;
    public bool IsCaseSensitive { get; set; }
    public int QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    [InverseProperty(nameof(OpenQuestionDbEntry.Options))]
    public OpenQuestionDbEntry Question { get; set; } = null!;
}