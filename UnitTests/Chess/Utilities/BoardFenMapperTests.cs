using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
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

        board.PlacePieceAtSquare(new Rook(Side.BLACK), new Square(0, 0));
        board.PlacePieceAtSquare(new Queen(Side.BLACK), new Square(0, 3));
        board.PlacePieceAtSquare(new Rook(Side.BLACK), new Square(0, 5));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square(1, 0));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square(1, 2));
        board.PlacePieceAtSquare(new King(Side.BLACK), new Square(1, 4));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square(1, 5));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square(1, 6));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square(1, 7));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square(2, 1));
        board.PlacePieceAtSquare(new Bishop(Side.BLACK), new Square(2, 4));
        board.PlacePieceAtSquare(new Knight(Side.BLACK), new Square(2, 5));
        board.PlacePieceAtSquare(new Bishop(Side.BLACK), new Square(3, 2));
        board.PlacePieceAtSquare(new Pawn(Side.BLACK), new Square(3, 3));
        board.PlacePieceAtSquare(new Knight(Side.WHITE), new Square(3, 4));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square(4, 0));
        board.PlacePieceAtSquare(new Bishop(Side.WHITE), new Square(4, 5));
        board.PlacePieceAtSquare(new Rook(Side.WHITE), new Square(5, 0));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square(5, 2));
        board.PlacePieceAtSquare(new Queen(Side.WHITE), new Square(5, 3));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square(6, 1));
        board.PlacePieceAtSquare(new King(Side.WHITE), new Square(6, 3));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square(6, 4));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square(6, 5));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square(6, 6));
        board.PlacePieceAtSquare(new Pawn(Side.WHITE), new Square(6, 7));
        board.PlacePieceAtSquare(new Knight(Side.WHITE), new Square(7, 1));
        board.PlacePieceAtSquare(new Bishop(Side.WHITE), new Square(7, 5));
        board.PlacePieceAtSquare(new Rook(Side.WHITE), new Square(7, 7));

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