using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class RookTests
{

    [TestMethod]
    public void GetPossibleMovesTest_EmptyBoard()
    {
        //Arrange
        var startingSquare = new Square(3, 3);
        var expected = new List<Square>() { new Square(4, 3), new Square(5, 3), new Square(6, 3), new Square(7, 3), new Square(2, 3), new Square(1, 3), new Square(0, 3), new Square(3, 4), new Square(3, 5), new Square(3, 6), new Square(3, 7), new Square(3, 2), new Square(3, 1), new Square(3, 0) };

        var board = new Board("8/8/8/3R4/8/8/8/8");
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

        var board = new Board("8/8/3P4/2PRP3/3P4/8/8/8");
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
        var startingSquare = new Square(2, 3);
        var expected = new List<Square>() { new Square(5, 3) };

        var board = new Board("8/8/3R4/1b6/8/8/4K3/8");
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
        var expected = new List<Square>() { new Square(3, 7) };

        var board = new Board("8/8/8/3R3b/8/8/4K3/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.GetPossibleMoves(board).ToArray();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

}