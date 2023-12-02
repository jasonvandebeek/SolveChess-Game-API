using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class PawnTests
{

    [TestMethod]
    public void GetPossibleMovesTest_WhitePawnFromE2EmptyBoard()
    {
        //Arrange
        var startingSquare = new Square("E2");
        var expected = new List<Square>() { new Square("E3"), new Square("E4") };

        var board = new Board("8/8/8/8/8/8/4P3/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_BlackPawnFromE2EmptyBoard()
    {
        //Arrange
        var startingSquare = new Square("E2");
        var expected = new List<Square>() { new Square("E1") };

        var board = new Board("8/8/8/8/8/8/4p3/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhitePawnFromE2Blocked()
    {
        //Arrange
        var startingSquare = new Square("E2");
        var expected = new List<Square>() { };

        var board = new Board("8/8/8/8/8/4N3/4P3/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhitePawnFromE2BlockedSecondMove()
    {
        //Arrange
        var startingSquare = new Square("E2");
        var expected = new List<Square>() { new Square("E3") };

        var board = new Board("8/8/8/8/4N3/8/4P3/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhitePawnFromE2TakablePieceNorthWest()
    {
        //Arrange
        var startingSquare = new Square("E2");
        var expected = new List<Square>() { new Square("E3"), new Square("E4"), new Square("D3") };

        var board = new Board("8/8/8/8/8/3n4/4P3/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhitePawnFromE3TakablePieceNorthEast()
    {
        //Arrange
        var startingSquare = new Square("E3");
        var expected = new List<Square>() { new Square("E4"), new Square("F4") };

        var board = new Board("8/8/8/8/5n2/4P3/8/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_BlackPawnFromC6EmptyBoard()
    {
        //Arrange
        var startingSquare = new Square("C6");
        var expected = new List<Square>() { new Square("C5") };

        var board = new Board("8/8/2p5/8/8/8/8/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_BlackPawnFromC6TakablesOnBothDiagonalsAndBlocked()
    {
        //Arrange
        var startingSquare = new Square("C6");
        var expected = new List<Square>() { new Square("D5"), new Square("B5") };

        var board = new Board("8/8/2p5/1NRN4/8/8/8/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

}