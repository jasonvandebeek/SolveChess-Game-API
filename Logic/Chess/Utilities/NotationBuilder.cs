
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Pieces;
using System.Text;

namespace SolveChess.Logic.Chess.Utilities;

public class NotationBuilder
{

    public string Notation { get; private set; }
    private readonly StringBuilder _notation;

    public NotationBuilder(MoveInfo moveInfo)
    {
        _notation = new StringBuilder();

        AddPieceType(moveInfo.Piece, moveInfo.TargetPiece, moveInfo.From);
        AddTakes(moveInfo.TargetPiece);
        AddTargetLocation(moveInfo.To);
        AddCastling(moveInfo.Piece, moveInfo.From, moveInfo.To);
        AddPromotion(moveInfo.Piece, moveInfo.Promotion);
        AddCheck(moveInfo.IsCheck, moveInfo.IsMate);
        AddEnpassant(moveInfo.Piece, moveInfo.To, moveInfo.EnpassantSquare);

        Notation = _notation.ToString();
    }

    private void AddPieceType(PieceBase piece, PieceBase? targetPiece, Square from)
    {
        if (piece.Type == PieceType.PAWN && targetPiece != null)
        {
            _notation.Append(from.Notation[0].ToString().ToLower());
        }
        else if(piece.Type != PieceType.PAWN)
        {
            _notation.Append(piece.Notation.ToString().ToUpper());
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
            if ((from.Rank == 0 || from.Rank == 7) && to.File == 2)
            {
                _notation.Clear();
                _notation.Append("O-O-O");
            }
            else if ((from.Rank == 0 || from.Rank == 7) && to.File == 6)
            {
                _notation.Clear();
                _notation.Append("O-O");
            }
        }
    }

    private void AddPromotion(PieceBase piece, PieceType? promotion)
    {
        if (promotion != null)
        {
            var tempPiece = PieceFactory.BuildPiece(promotion, piece.Side);

            _notation.Append('=');
            _notation.Append(tempPiece.Notation.ToString().ToUpper());
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

