using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Chess.Utilities;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace Logic.Chess.Utilities.Tests;

[TestClass]
public class NotationBuilderTests
{

    [TestMethod]
    public void NotationBuilderTest_WhitePawnMovesFromD2ToD4()
    {
        //Arrange
        var from = new Square("D2");
        var to = new Square("D4");
        var piece = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        var expected = "d4";

        var moveInfo = new MoveInfo()
        {
            Piece = piece,
            From = from,
            To = to
        };

        //Act
        var result = new NotationBuilder(moveInfo).Notation;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void NotationBuilderTest_WhiteKnightTakesBlackBishopFromB1ToC3()
    {
        //Arrange
        var from = new Square("B1");
        var to = new Square("C3");
        var piece = PieceFactory.BuildPiece(PieceType.KNIGHT, Side.WHITE);
        var taken = PieceFactory.BuildPiece(PieceType.BISHOP, Side.BLACK);
        var expected = "Nxc3";

        var moveInfo = new MoveInfo()
        {
            Piece = piece,
            From = from,
            TargetPiece = taken,
            To = to
        };

        //Act
        var result = new NotationBuilder(moveInfo).Notation;

        //Assert
        Assert.AreEqual(expected, result);
    }

}