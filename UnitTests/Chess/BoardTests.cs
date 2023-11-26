using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Tests;

[TestClass]
public class BoardTests
{

    [TestMethod]
    public void GetSquareOfPieceTest()
    {
        //Arrange
        var board = new Board("6b1/1b6/8/3B4/8/8/6B1/B7");
        var expected = new Square(3, 3);
        var piece = board.BoardArray[3, 3]!;

        //Act
        var result = board.GetSquareOfPiece(piece);

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetPieceAtTest()
    {
        //Arrange
        var board = new Board("r1b1k1nr/1pp1q1pp/p1n1p3/1N6/8/2P2N2/P3PPPP/R2QKB1R");
        var square = new Square(2, 2);
        var expected = board.BoardArray[2, 2]!;

        //Act
        var result = board.GetPieceAt(square);

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void MovePieceTest()
    {
        //Arrange
        var board = new Board("8/8/8/3P4/8/8/8/8");
        var from = new Square(3, 3);
        var to = new Square(2, 3);
        var piece = board.BoardArray[3, 3]!;

        //Act
        board.MovePiece(from, to);
        var pieceAtOldLocation = board.BoardArray[3, 3];
        var pieceAtNewLocation = board.BoardArray[2, 3];

        //Assert
        Assert.AreEqual(null, pieceAtOldLocation);
        Assert.AreEqual(piece, pieceAtNewLocation);
    }

    [TestMethod]
    public void MovePieceTest_MovingNull()
    {
        //Arrange
        var board = new Board("8/8/8/3P4/8/8/8/8");
        var from = new Square(4, 3);
        var to = new Square(3, 3);
        var piece = board.BoardArray[3, 3]!;

        //Act
        board.MovePiece(from, to);
        var pieceAtOldLocation = board.BoardArray[4, 3];
        var pieceAtNewLocation = board.BoardArray[3, 3];

        //Assert
        Assert.AreEqual(null, pieceAtOldLocation);
        Assert.AreEqual(piece, pieceAtNewLocation);
    }

    [TestMethod]
    public void PromotePieceTest()
    {
        //Arrange
        var board = new Board("8/3P4/8/8/8/8/8/8");
        var from = new Square(1, 3);
        var to = new Square(0, 3);
        var piece = board.BoardArray[1, 3]!;
        var promotionType = PieceType.QUEEN;

        //Act
        board.PromotePiece(from, to, promotionType);
        var result = board.BoardArray[0, 3]!;

        //Assert
        Assert.AreEqual(result.Side, piece.Side);
        Assert.AreEqual(result.Type, promotionType);
    }

    [TestMethod]
    public void PromotePieceTest_PromotingNull()
    {
        //Arrange
        var board = new Board("8/3P4/8/8/8/8/8/8");
        var from = new Square(1, 4);
        var to = new Square(0, 4);
        var promotionType = PieceType.QUEEN;

        //Act
        board.PromotePiece(from, to, promotionType);
        var OldLocationPiece = board.BoardArray[0, 3];
        var NewLocationPiece = board.BoardArray[0, 3];

        //Assert
        Assert.AreEqual(null, OldLocationPiece);
        Assert.AreEqual(null, NewLocationPiece);
    }

    [TestMethod]
    public void KingInCheckTest()
    {
        //Arrange
        var board = new Board("8/8/7p/7P/8/1r6/8/1K6");

        //Act
        var result = board.KingInCheck(Side.WHITE);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void KingInDrawTest()
    {
        //Arrange
        var board = new Board("8/8/7p/7P/8/r7/3q4/1K6");

        //Act
        var result = board.KingInDraw(Side.WHITE);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void KingInDrawTest_KingInCheck()
    {
        //Arrange
        var board = new Board("8/8/7p/7P/8/r7/8/1K1q4");

        //Act
        var result = board.KingInDraw(Side.WHITE);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void KingInDrawTest_PlayerHasMovesLeft()
    {
        //Arrange
        var board = new Board("8/8/8/7P/8/r7/3q4/1K6");

        //Act
        var result = board.KingInDraw(Side.WHITE);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void KingIsMatedTest()
    {
        //Arrange
        var board = new Board("8/8/7p/7P/8/8/3q4/1K2r3");

        //Act
        var result = board.KingIsMated(Side.WHITE);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void KingIsMatedTest_KingNotInCheck()
    {
        //Arrange
        var board = new Board("8/8/7p/7P/8/4r3/3q4/1K6");

        //Act
        var result = board.KingIsMated(Side.WHITE);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void KingIsMatedTest_KingHasMovesLeft()
    {
        //Arrange
        var board = new Board("8/8/7p/7P/8/8/rr6/1K6");

        //Act
        var result = board.KingIsMated(Side.WHITE);

        //Assert
        Assert.IsFalse(result);
    }

}