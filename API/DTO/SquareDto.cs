using SolveChess.API.ValidationAttributes;
using SolveChess.Logic.Chess.Interfaces;

namespace SolveChess.API.DTO;

public class SquareDto : ISquare
{

    [IsWithinBounds]
    public int Rank { get; set; }

    [IsWithinBounds]
    public int File { get; set; }

}
