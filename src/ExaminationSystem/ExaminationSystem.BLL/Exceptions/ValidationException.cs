using System.Runtime.Serialization;

namespace ExaminationSystem.BLL.Exceptions;

public class ValidationException : ApplicationException
{
    public ValidationException()
    {
    }

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, IEnumerable<string> validationErrors) : base(message)
    {
        ValidationErrors = validationErrors;
    }

    public ValidationException(string message, IEnumerable<string> validationErrors, Exception innerException) : base(
        message,
        innerException)
    {
        ValidationErrors = validationErrors;
    }

    protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public IEnumerable<string> ValidationErrors { get; set; } = new List<string>();
}