using System.Runtime.Serialization;

namespace SolveChess.Logic.Exceptions;

[Serializable]
public class ArgumentsOutOfBoundsException : SolveChessBaseException
{
    public ArgumentsOutOfBoundsException()
    {
    }

    public ArgumentsOutOfBoundsException(string? message) : base(message)
    {
    }

    public ArgumentsOutOfBoundsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ArgumentsOutOfBoundsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}