using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class PieceBaseTests
{

    [TestMethod]
    public void GetNotationOfWhitePawn()
    {
        //Arrange
        var piece = new Pawn(Side.WHITE);
        var expected = 'P';

        //Act
        var result = piece.Notation;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetNotationOfBlackRook()
    {
        //Arrange
        var piece = new Rook(Side.BLACK);
        var expected = 'r';

        //Act
        var result = piece.Notation;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void CanMoveToSquareTest_ReturnSucces()
    {
        //Arrange
        var startingSquare = new Square(3, 3);
        var targetSquare = new Square(4, 4);

        var board = new Board("8/8/8/3B4/8/8/8/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.CanMoveToSquare(targetSquare, board);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanMoveToSquareTest_ReturnFalse()
    {
        //Arrange
        var startingSquare = new Square(3, 3);
        var targetSquare = new Square(4, 5);

        var board = new Board("8/8/8/3B4/8/8/8/8");
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            return;

        //Act
        var result = piece.CanMoveToSquare(targetSquare, board);

        //Assert
        Assert.IsFalse(result);
    }

}