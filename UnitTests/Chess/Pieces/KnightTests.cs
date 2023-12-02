using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class KnightTests
{

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKnightFromD5EmptyBoard()
    {
        //Arrange
        var expected = new List<Square>()
        {
            new Square("C7"),
            new Square("B6"),
            new Square("B4"),
            new Square("C3"),
            new Square("E3"),
            new Square("F4"),
            new Square("F6"),
            new Square("E7")
        };

        var board = new Board();
        var piece = new Knight(Side.WHITE);
        var square = new Square("D5");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKnightFromD5BlockedOnEachSide()
    {
        //Arrange
        var expected = new List<Square>() { };

        var board = new Board("8/2P1P3/1P3P2/3N4/1P3P2/2P1P3/8/8");
        var piece = new Knight(Side.WHITE);
        var square = new Square("D5");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKnightFromH8NoMovesOffBoardOnEmptyBoard()
    {
        //Arrange
        var expected = new List<Square>() 
        {
            new Square("G6"),
            new Square("F7")
        };

        var board = new Board();
        var piece = new Knight(Side.WHITE);
        var square = new Square("H8");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKnightFromD4_KingInCheck_OnlyMoveIsBlockC2()
    {
        //Arrange
        var expected = new List<Square>() { new Square("C2") };

        var board = new Board("8/8/8/8/3N4/8/r2K4/8");
        var piece = new Knight(Side.WHITE);
        var square = new Square("D4");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKnightFromC3_KingInCheck_OnlyMoveIsTakeB1()
    {
        //Arrange
        var expected = new List<Square>() { new Square("B1") };

        var board = new Board("8/8/8/8/8/2N5/8/1r1K4");
        var piece = new Knight(Side.WHITE);
        var square = new Square("C3");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

}