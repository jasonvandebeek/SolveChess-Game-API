using System.Runtime.Serialization;

namespace SolveChess.Logic.Exceptions;

[Serializable]
public class PromotionException : Exception
{
    public PromotionException()
    {
    }

    public PromotionException(string? message) : base(message)
    {
    }

    public PromotionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PromotionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}