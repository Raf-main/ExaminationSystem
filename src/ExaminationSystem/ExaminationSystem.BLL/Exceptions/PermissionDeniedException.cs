﻿using System.Runtime.Serialization;

namespace ExaminationSystem.BLL.Exceptions;

public class PermissionDeniedException : Exception
{
    public PermissionDeniedException()
    {
    }

    public PermissionDeniedException(string message) : base(message)
    {
    }

    public PermissionDeniedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PermissionDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}