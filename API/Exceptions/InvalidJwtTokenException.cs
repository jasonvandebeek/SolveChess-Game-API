﻿using SolveChess.Logic.Exceptions;
using System.Runtime.Serialization;

namespace SolveChess.API.Exceptions;

[Serializable]
public class InvalidJwtTokenException : SolveChessBaseException
{
    public InvalidJwtTokenException()
    {
    }

    public InvalidJwtTokenException(string? message) : base(message)
    {
    }

    public InvalidJwtTokenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidJwtTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}