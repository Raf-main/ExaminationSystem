using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class OpenQuestionAnswerDbEntry : BaseEntity
{
    public string Answer { get; set; } = null!;
    public int OpenQuestionId { get; set; }

    [ForeignKey(nameof(OpenQuestionId))]
    [InverseProperty(nameof(OpenQuestionDbEntry.Answers))]
    public virtual OpenQuestionDbEntry OpenQuestion { get; set; } = null!;
}