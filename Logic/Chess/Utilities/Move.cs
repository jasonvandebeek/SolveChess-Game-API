using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using System.ComponentModel.DataAnnotations;

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

}

