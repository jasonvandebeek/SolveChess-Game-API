using System.Runtime.Serialization;

namespace SolveChess.Logic.Exceptions;
[Serializable]
public class InvalidSquareNotationException : SolveChessBaseException
{
    public InvalidSquareNotationException()
    {
    }

    public InvalidSquareNotationException(string? message) : base(message)
    {
    }

    public InvalidSquareNotationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidSquareNotationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}