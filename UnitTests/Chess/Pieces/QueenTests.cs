using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class QueenTests
{

    [TestMethod]
    public void GetPossibleMovesTest_WhiteQueenFromD5EmptyBoard()
    {
        //Arrange
        var expected = new List<Square>() 
        { 
            new Square("C5"),
            new Square("B5"),
            new Square("A5"),
            new Square("E5"),
            new Square("F5"),
            new Square("G5"),
            new Square("H5"),
            new Square("D6"),
            new Square("D7"),
            new Square("D8"),
            new Square("D4"),
            new Square("D3"),
            new Square("D2"),
            new Square("D1"),
            new Square("C6"),
            new Square("B7"),
            new Square("A8"),
            new Square("E6"),
            new Square("F7"),
            new Square("G8"),
            new Square("C4"),
            new Square("B3"),
            new Square("A2"),
            new Square("E4"),
            new Square("F3"),
            new Square("G2"),
            new Square("H1")
        };

        var board = new Board();
        var piece = new Queen(Side.WHITE);
        var square = new Square("D5");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteQueenFromD5BlockedOnEachSide()
    {
        //Arrange
        var expected = new List<Square>() { };

        var board = new Board("8/8/2RRR3/2RQR3/2RRR3/8/8/8");
        var piece = new Queen(Side.WHITE);
        var square = new Square("D5");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteQueenFromF5_KingInCheck_OnlyMoveIsBlockB1()
    {
        //Arrange
        var expected = new List<Square>() { new Square("B1") };

        var board = new Board("8/8/8/5Q2/8/8/8/r2K4");
        var piece = new Queen(Side.WHITE);
        var square = new Square("F5");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteQueenFromF6_KingInCheck_OnlyMoveIsTakeD4()
    {
        //Arrange
        var expected = new List<Square>() { new Square("D4") };

        var board = new Board("8/8/5Q2/8/3r4/8/8/3K4");
        var piece = new Queen(Side.WHITE);
        var square = new Square("F6");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

}