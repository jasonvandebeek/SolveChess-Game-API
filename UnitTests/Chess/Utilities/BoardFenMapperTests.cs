using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Exceptions;

namespace SolveChess.Logic.Chess.Utilities.Tests;

[TestClass]
public class BoardFenMapperTests
{

    private PieceBase?[,] board = null!;
    private string fen = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        board = new PieceBase[8, 8];

        board[0, 0] = PieceFactory.BuildPiece(PieceType.ROOK, Side.BLACK);
        board[0, 3] = PieceFactory.BuildPiece(PieceType.QUEEN, Side.BLACK);
        board[0, 5] = PieceFactory.BuildPiece(PieceType.ROOK, Side.BLACK);
        board[1, 0] = PieceFactory.BuildPiece(PieceType.PAWN, Side.BLACK);
        board[1, 2] = PieceFactory.BuildPiece(PieceType.PAWN, Side.BLACK);
        board[1, 4] = PieceFactory.BuildPiece(PieceType.KING, Side.BLACK);
        board[1, 5] = PieceFactory.BuildPiece(PieceType.PAWN, Side.BLACK);
        board[1, 6] = PieceFactory.BuildPiece(PieceType.PAWN, Side.BLACK);
        board[1, 7] = PieceFactory.BuildPiece(PieceType.PAWN, Side.BLACK);
        board[2, 1] = PieceFactory.BuildPiece(PieceType.PAWN, Side.BLACK);
        board[2, 4] = PieceFactory.BuildPiece(PieceType.BISHOP, Side.BLACK);
        board[2, 5] = PieceFactory.BuildPiece(PieceType.KNIGHT, Side.BLACK);
        board[3, 2] = PieceFactory.BuildPiece(PieceType.BISHOP, Side.BLACK);
        board[3, 3] = PieceFactory.BuildPiece(PieceType.PAWN, Side.BLACK);
        board[3, 4] = PieceFactory.BuildPiece(PieceType.KNIGHT, Side.WHITE);
        board[4, 0] = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        board[4, 5] = PieceFactory.BuildPiece(PieceType.BISHOP, Side.WHITE);
        board[5, 0] = PieceFactory.BuildPiece(PieceType.ROOK, Side.WHITE);
        board[5, 2] = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        board[5, 3] = PieceFactory.BuildPiece(PieceType.QUEEN, Side.WHITE);
        board[6, 1] = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        board[6, 3] = PieceFactory.BuildPiece(PieceType.KING, Side.WHITE);
        board[6, 4] = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        board[6, 5] = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        board[6, 6] = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        board[6, 7] = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        board[7, 1] = PieceFactory.BuildPiece(PieceType.KNIGHT, Side.WHITE);
        board[7, 5] = PieceFactory.BuildPiece(PieceType.BISHOP, Side.WHITE);
        board[7, 7] = PieceFactory.BuildPiece(PieceType.ROOK, Side.WHITE);

        fen = "r2q1r2/p1p1kppp/1p2bn2/2bpN3/P4B2/R1PQ4/1P1KPPPP/1N3B1R";
    }

    [TestMethod]
    public void GetFenFromBoardTest()
    {
        //Arrange

        //Act
        var result = BoardFenMapper.GetFenFromBoard(board);

        //Assert
        Assert.AreEqual(fen, result);
    }

    [TestMethod]
    public void GetBoardStateFromFenTest()
    {
        //Arrange

        //Act
        var result = BoardFenMapper.GetBoardStateFromFen(fen);

        //Assert
        for(int rank = 0; rank < board.GetLength(0); rank++) 
        { 
            for(int file = 0; file < board.GetLength(1); file++)
            {
                var expectedPiece = board[rank, file];
                var actualPiece = result[rank, file];

                if (expectedPiece?.Type != actualPiece?.Type || expectedPiece?.Side != actualPiece?.Side)
                    Assert.Fail();
            }
        }
    }

    [TestMethod]
    public void GetBoardStateFromFenTest_InvalidFenRankLength_ThrowsException()
    {
        //Arrange

        //Assert
        Assert.ThrowsException<InvalidFenException>(() =>
        {
            //Act
            var result = BoardFenMapper.GetBoardStateFromFen("r2q1/p1p1kppp/1p2b2/2bpN3/4B2/R1PQ4/1P1KPP/1N3B1R");
        });
    }

    [TestMethod]
    public void GetBoardStateFromFenTest_InvalidFenStructure_ThrowsException()
    {
        //Arrange

        //Assert
        Assert.ThrowsException<InvalidFenException>(() =>
        {
            //Act
            var result = BoardFenMapper.GetBoardStateFromFen("r2q1/p1p1kppp/1p2b2/2bpN3/4B2/R1PQ4/1P1KPP/1N3B1RWWQR");
        });
    }

}