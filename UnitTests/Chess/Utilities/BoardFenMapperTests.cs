using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Exceptions;

namespace SolveChess.Logic.Chess.Utilities.Tests;

[TestClass]
public class BoardFenMapperTests
{

    private Board board = null!;
    private string fen = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        board = new Board();

        board.PlacePieceAtSquare(new Rook(Side.BLACK), new Square("a8"));
        board.PlacePieceAtSquare(new Queen(Side.BLACK), new Square("d8"));
        board.PlacePieceAtSquare(new Rook(Side.BLACK), new Square("f8"));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square("a7"));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square("c7"));
        board.PlacePieceAtSquare(new King(Side.BLACK), new Square("e7"));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square("f7"));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square("g7"));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square("h7"));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square("b6"));
        board.PlacePieceAtSquare(new Bishop(Side.BLACK), new Square("e6"));
        board.PlacePieceAtSquare(new Knight(Side.BLACK), new Square("f6"));
        board.PlacePieceAtSquare(new Bishop(Side.BLACK), new Square("c5"));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square("d5"));
        board.PlacePieceAtSquare(new Knight(Side.WHITE), new Square("e5"));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square("a4"));
        board.PlacePieceAtSquare(new Bishop(Side.WHITE), new Square("f4"));
        board.PlacePieceAtSquare(new Rook(Side.WHITE), new Square("a3"));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square("c3"));
        board.PlacePieceAtSquare(new Queen(Side.WHITE), new Square("d3"));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square("b2"));
        board.PlacePieceAtSquare(new King(Side.WHITE), new Square("d2"));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square("e2"));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square("f2"));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square("g2"));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square("h2"));
        board.PlacePieceAtSquare(new Knight(Side.WHITE), new Square("b1"));
        board.PlacePieceAtSquare(new Bishop(Side.WHITE), new Square("f1"));
        board.PlacePieceAtSquare(new Rook(Side.WHITE), new Square("h1"));

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
        var boardArray = board.BoardArray;

        //Assert
        for(int rank = 0; rank < boardArray.GetLength(0); rank++) 
        { 
            for(int file = 0; file < boardArray.GetLength(1); file++)
            {
                var expectedPiece = boardArray[rank, file];
                var actualPiece = result[rank, file];

                if (expectedPiece?.Type != actualPiece?.Type || expectedPiece?.Side != actualPiece?.Side)
                    Assert.Fail();
            }
        }
    }

    [TestMethod]
    public void GetBoardStateFromFenTest_InvalidFenRankLengthThrowsException()
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
    public void GetBoardStateFromFenTest_InvalidFenStructureThrowsException()
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