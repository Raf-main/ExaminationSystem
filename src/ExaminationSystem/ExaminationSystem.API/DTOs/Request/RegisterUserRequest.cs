using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.API.DTOs.Request;

public class RegisterUserRequest
{
    [Required] public string Name { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}