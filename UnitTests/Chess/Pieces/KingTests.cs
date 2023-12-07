using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.UnitTests.Helpers;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class KingTests
{

    [TestMethod]
    [DataRow("8/8/8/3K4/8/8/8/8", Side.WHITE, "d5", new string[] { "d6", "e6", "e5", "e4", "d4", "c4", "c5", "c6" }, DisplayName = "AllMovesWhiteKingOnEmptyBoard")]
    [DataRow("8/8/8/8/8/8/8/R3K2R", Side.WHITE, "e1", new string[] { "d1", "f2", "d2", "e2", "f1", "c1", "g1" }, DisplayName = "AllMovesAndCastlingMovesWhiteKingCanCastleOnBothSides")]
    [DataRow("r3k2r/8/8/8/8/8/8/8", Side.BLACK, "e8", new string[] { "c8", "d8", "f8", "g8", "d7", "e7", "f7" }, DisplayName = "AllMovesAndCastlingMovesBlackKingCanCastleOnBothSides")]
    [DataRow("8/8/8/8/8/4k3/8/4K3", Side.WHITE, "e1", new string[] { "d1", "f1" }, DisplayName = "WhiteKingCantMoveIntoOpponentAttacks")]
    [DataRow("8/8/8/8/8/8/4q3/4K3", Side.WHITE, "e1", new string[] { "e2" }, DisplayName = "WhiteKingOnlyMoveIsTake")]
    [DataRow("8/8/8/8/8/2q5/2q5/4K3", Side.WHITE, "e1", new string[] { "f1" }, DisplayName = "SingleMoveWhiteKing")]
    public void GetPossibleMovesTest(string fen, Side side, string position, string[] moves)
    {
        //Arrange
        var expected = SquareBuilderHelper.GetSquaresOfStringNotations(moves);

        var board = new Board(fen, true, true, true, true);
        var piece = new King(side);

        board.PlacePieceAtSquare(piece, new Square(position));

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    [DataRow("8/8/8/8/8/8/3p4/4K3", DisplayName = "ByPawn")]
    [DataRow("8/8/8/8/8/4r3/8/4K3", DisplayName = "ByRook")]
    [DataRow("8/8/8/8/8/5n2/8/4K3", DisplayName = "ByKnight")]
    [DataRow("8/8/8/8/1b6/8/8/4K3", DisplayName = "ByBishop")]
    [DataRow("8/8/4q3/8/8/8/8/4K3", DisplayName = "ByQueen")]
    [DataRow("8/8/8/8/8/8/4k3/4K3", DisplayName = "ByKing")]
    public void IsCheckedTest(string fen)
    {
        //Arrange
        var board = new Board(fen);
        var piece = new King(Side.WHITE);
        var square = new Square("E1");

        board.PlacePieceAtSquare(piece, square);

        //Act
        var result = piece.IsChecked(board);

        //Assert
        Assert.IsTrue(result);
    }

}