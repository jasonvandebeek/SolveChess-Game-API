using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.UnitTests.Helpers;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class RookTests
{

    [TestMethod]
    [DataRow("8/8/8/3R4/8/8/8/8", "d5", new string[] { "d4", "d3", "d2", "d1", "d6", "d7", "d8", "c5", "b5", "a5", "e5", "f5", "g5", "h5" }, DisplayName = "AllMovesOnEmptyBoard")]
    [DataRow("8/8/3P4/2PRP3/3P4/8/8/8", "d5", new string[] { }, DisplayName = "AllSidesBlockedNoMoves")]
    [DataRow("8/8/3R4/1b6/8/8/4K3/8", "d6", new string[] { "d3" }, DisplayName = "KingInCheckOnlyMoveIsBlock")]
    [DataRow("8/8/8/3R3b/8/8/4K3/8", "d5", new string[] { "h5" }, DisplayName = "KingInCheckOnlyMoveIsTake")]
    public void GetPossibleMovesTest(string fen, string position, string[] moves)
    {
        //Arrange
        var expected = SquareBuilderHelper.GetSquaresOfStringNotations(moves);

        var board = new Board(fen);
        var piece = new Rook(Side.WHITE);

        board.PlacePieceAtSquare(piece, new Square(position));

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

}