using System.Runtime.Serialization;

namespace ExaminationSystem.BLL.Managers.AccountManagers.Exceptions;

public class IncorrectUserPasswordException : Exception
{
    public IncorrectUserPasswordException()
    {
    }

    public IncorrectUserPasswordException(string message) : base(message)
    {
    }

    public IncorrectUserPasswordException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected IncorrectUserPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}