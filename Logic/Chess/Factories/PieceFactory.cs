
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;

namespace SolveChess.Logic.Chess.Factories;

public class PieceFactory
{

    public PieceBase BuildPiece(char type, Side side)
    {
        char lowerChar = char.ToLower(type);

        return lowerChar switch
        {
            'p' => new Pawn(side),
            'r' => new Rook(side),
            'n' => new Knight(side),
            'b' => new Bishop(side),
            'q' => new Queen(side),
            'k' => new King(side),
            _ => throw new Exception("Invalid piece type!")
        };
    }

    public PieceBase BuildPiece(PieceType? type, Side side)
    {
        return type switch
        {
            PieceType.PAWN => new Pawn(side),
            PieceType.ROOK => new Rook(side),
            PieceType.KNIGHT => new Knight(side),
            PieceType.BISHOP => new Bishop(side),
            PieceType.QUEEN => new Queen(side),
            PieceType.KING => new King(side),
            _ => throw new Exception("Invalid piece type!")
        };
    }

}

