
using SolveChess.Logic.Chess.Attributes;

namespace SolveChess.Logic.DTO;

public class MoveDto
{

    public int Number { get; set; }
    public Side Side { get; set; }
    public string Notation { get; set; } = null!;

}

