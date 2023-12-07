using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.UnitTests.Helpers;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class BishopTests
{

    [TestMethod]
    [DataRow("8/8/8/3B4/8/8/8/8", "d5", new string[] { "e4", "f3", "g2", "h1", "c6", "b7", "a8", "e6", "f7", "g8", "c4", "b3", "a2" }, DisplayName = "AllMovesOnEmptyBoard")]
    [DataRow("8/8/2P1P3/3B4/2P1P3/8/8/8", "d5", new string[] { }, DisplayName = "AllSidesBlockedNoMoves")]
    [DataRow("8/8/8/3B4/b7/8/8/3K4", "d5", new string[] { "b3" }, DisplayName = "KingInCheckOnlyMoveIsBlock")]
    [DataRow("8/8/8/3B4/8/1b6/8/3K4", "d5", new string[] { "b3" }, DisplayName = "KingInCheckOnlyMoveIsTake")]
    public void GetPossibleMovesTest(string fen, string position, string[] moves)
    {
        //Arrange
        var expected = SquareBuilderHelper.GetSquaresOfStringNotations(moves);

        var board = new Board(fen);
        var piece = new Bishop(Side.WHITE);

        board.PlacePieceAtSquare(piece, new Square(position));

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

}