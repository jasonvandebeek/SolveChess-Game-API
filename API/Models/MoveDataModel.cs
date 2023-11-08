namespace SolveChess.API.Models;

public class MoveDataModel
{

    public Square From { get; set; }
    public Square To { get; set; }

}

public class Square
{

    public int Rank { get; set; }
    public int File { get; set; }

}

