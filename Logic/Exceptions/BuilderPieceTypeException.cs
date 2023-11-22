using System.Runtime.Serialization;

namespace SolveChess.Logic.Exceptions;

[Serializable]
public class BuilderPieceTypeException : Exception
{
    public BuilderPieceTypeException()
    {
    }

    public BuilderPieceTypeException(string? message) : base(message)
    {
    }

    public BuilderPieceTypeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected BuilderPieceTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}