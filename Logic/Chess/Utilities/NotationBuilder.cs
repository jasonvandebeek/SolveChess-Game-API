
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Chess;
using System.Text;

namespace Logic.Chess.Utilities;

public class NotationBuilder
{

    public string Notation { get; private set; }
    private readonly StringBuilder _notation;

    public NotationBuilder(PieceBase piece, PieceBase? targetPiece, Square from, Square to, PieceType? promotion, bool isCheck, bool isMate, Square? enpassantSquare)
    {
        _notation = new StringBuilder();

        AddPieceType(piece, targetPiece);
        AddTakes(targetPiece);
        AddTargetLocation(to);
        AddCastling(piece, from, to);
        AddPromotion(piece, promotion);
        AddCheck(isCheck, isMate);
        AddEnpassant(piece, to, enpassantSquare);

        Notation = _notation.ToString();
    }

    private void AddPieceType(PieceBase piece, PieceBase? targetPiece)
    {
        if (piece.Type == PieceType.PAWN && targetPiece != null)
        {
            _notation.Append('e');
        }
        else
        {
            _notation.Append(piece.Notation);
        }
    }

    private void AddTakes(PieceBase? targetPiece)
    {
        if (targetPiece != null)
            _notation.Append('x');
    }

    private void AddTargetLocation(Square to)
    {
        _notation.Append(to.Notation);
    }

    private void AddCastling(PieceBase piece, Square from, Square to)
    {
        if (piece.Type == PieceType.KING)
        {
            if ((from.Rank == 0 || from.Rank == 7) && to.File == 1)
                _notation.Append("O-O-O");
            else if ((from.Rank == 0 || from.Rank == 7) && to.File == 6)
                _notation.Append("O-O");
        }
    }

    private void AddPromotion(PieceBase piece, PieceType? promotion)
    {
        if (promotion != null)
        {
            var tempPiece = PieceFactory.BuildPiece(promotion, piece.Side);

            _notation.Append('=');
            _notation.Append(tempPiece.Notation);
        }
    }

    private void AddCheck(bool isCheck, bool isMate)
    {
        if (isMate)
            _notation.Append('#');
        else if (isCheck)
            _notation.Append('+');
    }

    private void AddEnpassant(PieceBase piece, Square to, Square? enpassantSquare)
    {
        if (piece.Type == PieceType.PAWN && to.Equals(enpassantSquare))
            _notation.Append(" e.p.");
    }

}

