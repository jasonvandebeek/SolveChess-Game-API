using SolveChess.Logic.Chess.Attributes;

namespace SolveChess.Logic.Chess.Utilities;

public class Move
{

    public int Number { get; set; }
    public Side Side { get; set; }
    public string Notation { get; set; }

    public Move(int number, Side side, string notation)
    {
        Number = number;
        Side = side;
        Notation = notation;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Move other = (Move)obj;

        return other.Side == Side && other.Number == Number && other.Notation == Notation;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Number, Side, Notation);
    }
}

