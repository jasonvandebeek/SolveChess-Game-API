using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.UnitTests.Helpers;

namespace SolveChess.Logic.Chess.Pieces.Tests;

[TestClass]
public class KnightTests
{

    [TestMethod]
    [DataRow("8/8/8/3N4/8/8/8/8", "d5", new string[] { "c7", "b6", "b4", "c3", "e3", "f4", "f6", "e7" }, DisplayName = "AllMovesOnEmptyBoard")]
    [DataRow("8/2P1P3/1P3P2/3N4/1P3P2/2P1P3/8/8", "d5", new string[] { }, DisplayName = "AllSidesBlockedNoMoves")]
    [DataRow("7N/8/8/8/8/8/8/8", "h8", new string[] { "g6", "f7" }, DisplayName = "CantMoveOffBoard")]
    [DataRow("8/8/8/8/3N4/8/r2K4/8", "d4", new string[] { "c2" }, DisplayName = "KingInCheckOnlyMoveIsBlock")]
    [DataRow("8/8/8/8/8/2N5/8/1r1K4", "c3", new string[] { "b1" }, DisplayName = "KingInCheckOnlyMoveIsTake")]
    public void GetPossibleMovesTest(string fen, string position, string[] moves)
    {
        //Arrange
        var expected = SquareBuilderHelper.GetSquaresOfStringNotations(moves);

        var board = new Board(fen);
        var piece = new Knight(Side.WHITE);

        board.PlacePieceAtSquare(piece, new Square(position));

        //Act
        var result = piece.GetPossibleMoves(board).ToList();

        //Assert
        CollectionAssert.AreEquivalent(expected, result);
    }

}