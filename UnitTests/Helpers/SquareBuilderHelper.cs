
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.UnitTests.Helpers;

public class SquareBuilderHelper
{

    public static List<Square> GetSquaresOfStringNotations(IEnumerable<string> notations)
    {
        var squares = new List<Square>();

        foreach(var notation in notations)
        {
           squares.Add(new Square(notation));
        }

        return squares;
    }

}

