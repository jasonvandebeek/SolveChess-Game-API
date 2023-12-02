using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Utilities.Tests;

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

    [TestMethod]
    public void NotationBuilderTest_WhitePawnTakesBlackKnightFromD2ToE3()
    {
        //Arrange
        var from = new Square("D2");
        var to = new Square("E3");
        var piece = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        var taken = PieceFactory.BuildPiece(PieceType.KNIGHT, Side.BLACK);
        var expected = "dxe3";

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

    [TestMethod]
    public void NotationBuilderTest_WhiteKingCastlesQueenSide()
    {
        //Arrange
        var from = new Square("E1");
        var to = new Square("C1");
        var piece = PieceFactory.BuildPiece(PieceType.KING, Side.WHITE);
        var expected = "O-O-O";

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
    public void NotationBuilderTest_BlackKingCastlesKingSide()
    {
        //Arrange
        var from = new Square("E8");
        var to = new Square("G8");
        var piece = PieceFactory.BuildPiece(PieceType.KING, Side.BLACK);
        var expected = "O-O";

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
    public void NotationBuilderTest_BlackPawnPromotesToQueenFromE2ToE1()
    {
        //Arrange
        var from = new Square("E2");
        var to = new Square("E1");
        var piece = PieceFactory.BuildPiece(PieceType.PAWN, Side.BLACK);
        var promotion = PieceType.QUEEN;
        var expected = "e1=Q";

        var moveInfo = new MoveInfo()
        {
            Piece = piece,
            From = from,
            To = to,
            Promotion = promotion
        };

        //Act
        var result = new NotationBuilder(moveInfo).Notation;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void NotationBuilderTest_BlackRookChecksWhiteKingFromE5ToE2()
    {
        //Arrange
        var from = new Square("E5");
        var to = new Square("E2");
        var piece = PieceFactory.BuildPiece(PieceType.ROOK, Side.BLACK);
        var check = true;
        var expected = "Re2+";

        var moveInfo = new MoveInfo()
        {
            Piece = piece,
            From = from,
            To = to,
            IsCheck = check
        };

        //Act
        var result = new NotationBuilder(moveInfo).Notation;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void NotationBuilderTest_BlackQueenCheckmatesWhiteKingFromG6ToB1()
    {
        //Arrange
        var from = new Square("G6");
        var to = new Square("B1");
        var piece = PieceFactory.BuildPiece(PieceType.QUEEN, Side.BLACK);
        var check = true;
        var mate = true;
        var expected = "Qb1#";

        var moveInfo = new MoveInfo()
        {
            Piece = piece,
            From = from,
            To = to,
            IsCheck = check,
            IsMate = mate
        };

        //Act
        var result = new NotationBuilder(moveInfo).Notation;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void NotationBuilderTest_BlackPawnEnpassantWhitePawnFromF4ToE3()
    {
        //Arrange
        var from = new Square("F4");
        var to = new Square("E3");
        var piece = PieceFactory.BuildPiece(PieceType.PAWN, Side.BLACK);
        var takes = PieceFactory.BuildPiece(PieceType.PAWN, Side.WHITE);
        var EnpassantSquare = new Square("E3");
        var expected = "fxe3 e.p.";

        var moveInfo = new MoveInfo()
        {
            Piece = piece,
            TargetPiece = takes,
            From = from,
            To = to,
            EnpassantSquare = EnpassantSquare
        };

        //Act
        var result = new NotationBuilder(moveInfo).Notation;

        //Assert
        Assert.AreEqual(expected, result);
    }

}