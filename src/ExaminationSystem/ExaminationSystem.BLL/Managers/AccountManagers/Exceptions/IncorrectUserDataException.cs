using System.Runtime.Serialization;

namespace ExaminationSystem.BLL.Managers.AccountManagers.Exceptions;

public class IncorrectUserDataException : Exception
{
    public IncorrectUserDataException()
    {
    }

    public IncorrectUserDataException(string message) : base(message)
    {
    }

    public IncorrectUserDataException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected IncorrectUserDataException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}