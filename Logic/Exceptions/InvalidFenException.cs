using System.Runtime.Serialization;

namespace SolveChess.Logic.Exceptions;

[Serializable]
public class InvalidFenException : Exception
{
    public InvalidFenException()
    {
    }

    public InvalidFenException(string? message) : base(message)
    {
    }

    public InvalidFenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidFenException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}