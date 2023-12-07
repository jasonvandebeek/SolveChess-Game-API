using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class PieceBaseTests
{

    [TestMethod]
    [DataRow("d5", "e4", "8/8/8/3B4/8/8/8/8", true, DisplayName = "PossibleMove")]
    [DataRow("d5", "f4", "8/8/8/3B4/8/8/8/8", false, DisplayName = "ImpossibleMove")]
    public void CanMoveToSquareTest_ReturnFalse(string startingSquareNotation, string targetSquareNotation, string fen, bool expected)
    {
        //Arrange
        var startingSquare = new Square(startingSquareNotation);
        var targetSquare = new Square(targetSquareNotation);

        var board = new Board(fen);
        var piece = board.GetPieceAt(startingSquare);
        if (piece == null)
            Assert.Fail();

        //Act
        var result = piece.CanMoveToSquare(targetSquare, board);

        //Assert
        Assert.AreEqual(expected, result);
    }

}