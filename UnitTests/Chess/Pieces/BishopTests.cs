using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class BishopTests
{

    [TestMethod]
    public void GetPossibleMovesTest_EmptyBoard()
    {
        //Arrange
        var startingSquare = new Square(3, 3);
        var expected = new List<Square>() { new Square(4, 4), new Square(5, 5), new Square(6, 6), new Square(7, 7), new Square(2, 2), new Square(1, 1), new Square(0, 0), new Square(2, 4), new Square(1, 5), new Square(0, 6), new Square(4, 2), new Square(5, 1), new Square(6, 0) };

        var board = new Board("8/8/8/3B4/8/8/8/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_BlockedOnEachSide()
    {
        //Arrange
        var startingSquare = new Square(3, 3);
        var expected = new List<Square>() {};

        var board = new Board("8/8/2P1P3/3B4/2P1P3/8/8/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_KingInCheck_OnlyMoveIsBlock()
    {
        //Arrange
        var startingSquare = new Square(3, 3);
        var expected = new List<Square>() { new Square(5, 1) };

        var board = new Board("8/8/8/3B4/b7/8/8/3K4");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_KingInCheck_OnlyMoveIsTake()
    {
        //Arrange
        var startingSquare = new Square(3, 3);
        var expected = new List<Square>() { new Square(5, 1) };

        var board = new Board("8/8/8/3B4/8/1b6/8/3K4");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

}