using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Models;

namespace SolveChess.Logic.Chess.Tests;

[TestClass]
public class GameTests
{

    private GameStateModel _newGameStateModel = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _newGameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            EnpassantSquare = null,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

    }

    [TestMethod]
    public void PlayMoveTest_NewGameMoveE2ToE4()
    {
        //Arrange
        var expected = new Move(1, Side.WHITE, "e4");

        var game = new Game(_newGameStateModel);
        var from = new Square("E2");
        var to = new Square("E4");

        //Act
        var result = game.PlayMove(from, to, null);

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_NewGameMoveE4ToE5ResultNull()
    {
        //Arrange
        Move? expected = null;

        var game = new Game(_newGameStateModel);
        var from = new Square("E4");
        var to = new Square("E5");

        //Act
        var result = game.PlayMove(from, to, null);

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_NewGameMoveE7ToE6ResultNull()
    {
        //Arrange
        Move? expected = null;

        var game = new Game(_newGameStateModel);
        var from = new Square("E7");
        var to = new Square("E6");

        //Act
        var result = game.PlayMove(from, to, null);

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_NewGameMoveE2ToC3ResultNull()
    {
        //Arrange
        Move? expected = null;

        var game = new Game(_newGameStateModel);
        var from = new Square("E2");
        var to = new Square("C3");

        //Act
        var result = game.PlayMove(from, to, null);

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_NewGameMoveE2ToE4_EnpassantSquareIsE3()
    {
        //Arrange
        var expected = new Square("E3");

        var game = new Game(_newGameStateModel);
        var from = new Square("E2");
        var to = new Square("E4");

        //Act
        game.PlayMove(from, to, null);
        var result = game.EnpassantSquare;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_WhiteKingMovesFromE1ToE2AfterNoRemainingCastlingRightsForWhite()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/8/8/8/8/8/8/R3K2R",
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            EnpassantSquare = null,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("E1");
        var to = new Square("E2");

        //Act
        game.PlayMove(from, to, null);

        //Assert
        Assert.IsFalse(game.CastlingRightWhiteKingSide);
        Assert.IsFalse(game.CastlingRightWhiteQueenSide);
    }

    [TestMethod]
    public void PlayMoveTest_BlackQueenMovesFromD8ToE8AfterNoRemainingCastlingRightsForWhite()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "3q4/8/8/8/8/8/8/R3K2R",
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            EnpassantSquare = null,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.BLACK
        };

        var game = new Game(gameStateModel);
        var from = new Square("D8");
        var to = new Square("E8");

        //Act
        game.PlayMove(from, to, null);

        //Assert
        Assert.IsFalse(game.CastlingRightWhiteKingSide);
        Assert.IsFalse(game.CastlingRightWhiteQueenSide);
    }

}