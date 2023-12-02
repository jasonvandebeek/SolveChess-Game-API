
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Exceptions;
using System.Text;
using System.Text.RegularExpressions;

namespace SolveChess.Logic.Chess.Utilities;

public static class BoardFenMapper
{

    public static string GetFenFromBoard(Board board)
    {
        var fen = new StringBuilder();
        var boardArray = board.BoardArray;

        for (int rank = 0; rank < boardArray.GetLength(0); rank++)
        {
            int empty = 0;
            for (int file = 0; file < boardArray.GetLength(1); file++)
            {
                PieceBase? piece = boardArray[rank, file];
                if (piece == null)
                {
                    empty += 1;
                    continue;
                }

                if (empty > 0)
                {
                    fen.Append(empty);
                    empty = 0;
                }
                    
                fen.Append(piece.Notation);
            }

            if (empty > 0)
                fen.Append(empty);

            fen.Append('/');
        }

        fen.Remove(fen.Length - 1, 1);
        return fen.ToString();
    }

    public static PieceBase?[,] GetBoardStateFromFen(string fen)
    {
        PieceBase?[,] board = new PieceBase?[8, 8];

        if (!IsValidFEN(fen))
            throw new InvalidFenException();

        string[] ranks = fen.Split(' ')[0].Split('/');

        for (int rank = 0; rank < 8; rank++)
        {
            int file = 0;
            foreach (char fenChar in ranks[rank])
            {
                if (char.IsDigit(fenChar))
                {
                    file += int.Parse(fenChar.ToString());
                    continue;
                }

                Side side = char.IsUpper(fenChar) ? Side.WHITE : Side.BLACK;

                board[rank, file] = PieceFactory.BuildPiece(fenChar, side);
                file++;
            }
        }

        return board;
    }

    private static bool IsValidFEN(string fen)
    {
        string pattern = @"^\s*(((?:[rnbqkpRNBQKP1-8]+\/){7})[rnbqkpRNBQKP1-8]+)$";

        try
        {
            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
            Match match = regex.Match(fen);

            if (!match.Success)
                return false;

            string[] ranks = fen.Split(' ')[0].Split('/');
            for (int rank = 0; rank < 8; rank++)
            {
                int pieceCount = 0;
                foreach (char fenChar in ranks[rank])
                {
                    pieceCount += char.IsDigit(fenChar) ? int.Parse(fenChar.ToString()) : 1;
                }

                if (pieceCount != 8)
                    return false;
            }

            return true;
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

}

