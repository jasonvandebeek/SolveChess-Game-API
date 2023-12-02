using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class KingTests
{

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKingFromD5EmptyBoard()
    {
        //Arrange
        var expected = new List<Square>()
        {
            new Square("D6"),
            new Square("E6"),
            new Square("E5"),
            new Square("E4"),
            new Square("D4"),
            new Square("C4"),
            new Square("C5"),
            new Square("C6")
        };

        var board = new Board();
        var piece = new King(Side.WHITE);
        var square = new Square("D5");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKingFromE1CanCastleBothSides()
    {
        //Arrange
        var expected = new List<Square>()
        {
            new Square("D1"),
            new Square("F2"),
            new Square("D2"),
            new Square("E2"),
            new Square("F1"),
            new Square("C1"),
            new Square("G1")
        };

        var board = new Board("8/8/8/8/8/8/8/R3K2R", false, false, true, true);
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKingFromE1CantMoveIntoOpposingKingOnE3()
    {
        //Arrange
        var expected = new List<Square>()
        {
            new Square("D1"),
            new Square("F1")
        };

        var board = new Board();
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        var opposingPiece = new King(Side.BLACK);
        var opposingSquare = new Square("E3");

        board.PlacePieceAtSquare(piece, square);
        board.PlacePieceAtSquare(opposingPiece, opposingSquare);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKingFromE1OnlyMoveIsTakeOnE2()
    {
        //Arrange
        var expected = new List<Square>()
        {
            new Square("E2")
        };

        var board = new Board("8/8/8/8/8/8/4q3/4K3");
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void GetPossibleMovesTest_WhiteKingFromE1OnlyMoveIsGoToF1()
    {
        //Arrange
        var expected = new List<Square>()
        {
            new Square("F1")
        };

        var board = new Board("8/8/8/8/8/2q5/2q5/4K3");
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void IsCheckedTestByPawn()
    {
        //Arrange
        var board = new Board("8/8/8/8/8/8/3p4/4K3");
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.IsChecked(board);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsCheckedTestByRook()
    {
        //Arrange
        var board = new Board("8/8/8/8/8/4r3/8/4K3");
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.IsChecked(board);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsCheckedTestByKnight()
    {
        //Arrange
        var board = new Board("8/8/8/8/8/5n2/8/4K3");
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.IsChecked(board);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsCheckedTestByBishop()
    {
        //Arrange
        var board = new Board("8/8/8/8/1b6/8/8/4K3");
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.IsChecked(board);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsCheckedTestByQueen()
    {
        //Arrange
        var board = new Board("8/8/4q3/8/8/8/8/4K3");
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.IsChecked(board);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsCheckedTestByKing()
    {
        //Arrange
        var board = new Board("8/8/8/8/8/8/4k3/4K3");
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.IsChecked(board);

        //Assert
        Assert.IsTrue(result);
    }

}