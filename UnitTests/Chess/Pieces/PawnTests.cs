using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.UnitTests.Helpers;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class PawnTests
{

    [TestMethod]
    [DataRow("8/8/8/8/8/8/4P3/8", Side.WHITE, "e2", new string[] { "e3", "e4" }, DisplayName = "AllMovesWhitePawnOnStartingRankEmptyBoard")]
    [DataRow("8/8/8/8/8/8/4p3/8", Side.BLACK, "e2", new string[] { "e1" }, DisplayName = "SingleMoveBlackPawnOnEmptyBoard")]
    [DataRow("8/8/8/8/8/4N3/4P3/8", Side.WHITE, "e2", new string[] { }, DisplayName = "NoMovesWhitePawnOnStartingRankBlocked")]
    [DataRow("8/8/8/8/4N3/8/4P3/8", Side.WHITE, "e2", new string[] { "e3" }, DisplayName = "SingleMoveWhitePawnOnStartingRankSecondSquareBlocked")]
    [DataRow("8/8/8/8/8/3n4/4P3/8", Side.WHITE, "e2", new string[] { "e3", "e4", "d3" }, DisplayName = "AllMovesAndTakeWhitePawnOnStartingRank")]
    [DataRow("8/8/8/8/5n2/4P3/8/8", Side.WHITE, "e3", new string[] { "e4", "f4" }, DisplayName = "MoveForwardAndTakeWhitePawn")]
    [DataRow("8/8/2p5/1NRN4/8/8/8/8", Side.BLACK, "c6", new string[] { "d5", "b5" }, DisplayName = "TwoTakesBlackPawnBlocked")]
    public void GetPossibleMovesTest(string fen, Side side, string position, string[] moves)
    {
        //Arrange
        var expected = SquareBuilderHelper.GetSquaresOfStringNotations(moves);

        var board = new Board(fen);
        var piece = new Pawn(side);

        board.PlacePieceAtSquare(piece, new Square(position));

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    } 

}