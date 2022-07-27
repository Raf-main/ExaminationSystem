using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.BLL.Helpers;

public class GenericRange<T> where T : IComparable<T>
{
    [Required] public T? Start { get; set; }

    [Required] public T? End { get; set; }
}