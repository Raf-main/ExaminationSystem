using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Entities;

public class OpenQuestionDbEntry : Question
{
    [InverseProperty(nameof(OpenQuestionOptionDbEntry.Question))]
    public virtual ICollection<OpenQuestionOptionDbEntry> Options { get; set; } =
        new HashSet<OpenQuestionOptionDbEntry>();

    [InverseProperty(nameof(OpenQuestionAnswerDbEntry.OpenQuestion))]
    public virtual ICollection<OpenQuestionAnswerDbEntry> Answers { get; set; } =
        new HashSet<OpenQuestionAnswerDbEntry>();
}