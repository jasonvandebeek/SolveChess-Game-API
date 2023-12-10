using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Exceptions;
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
        Assert.AreEqual(expected.Number, result?.Number);
        Assert.AreEqual(expected.Side, result?.Side);
        Assert.AreEqual(expected.Notation, result?.Notation);
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
    public void PlayMoveTest_NewGameMoveE2ToE4EnpassantSquareIsE3()
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
    public void PlayMoveTest_BlackKingMovesFromE8ToD8AfterNoRemainingCastlingRightsForBlack()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "r3k2r/8/8/8/8/8/8/8",
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.BLACK
        };

        var game = new Game(gameStateModel);
        var from = new Square("E8");
        var to = new Square("D8");

        //Act
        game.PlayMove(from, to, null);

        //Assert
        Assert.IsFalse(game.CastlingRightBlackKingSide);
        Assert.IsFalse(game.CastlingRightBlackQueenSide);
    }

    [TestMethod]
    public void PlayMoveTest_WhiteRookMovesFromA1ToA3AfterWhiteHasNoQueenSideCastlingRight()
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
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("A1");
        var to = new Square("A3");

        //Act
        game.PlayMove(from, to, null);

        //Assert
        Assert.IsTrue(game.CastlingRightWhiteKingSide);
        Assert.IsFalse(game.CastlingRightWhiteQueenSide);
    }

    [TestMethod]
    public void PlayMoveTest_BlackRookMovesFromH8ToG8AfterBlackHasNoKingSideCastlingRight()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "r3k2r/8/8/8/8/8/8/8",
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.BLACK
        };

        var game = new Game(gameStateModel);
        var from = new Square("H8");
        var to = new Square("G8");

        //Act
        game.PlayMove(from, to, null);

        //Assert
        Assert.IsFalse(game.CastlingRightBlackKingSide);
        Assert.IsTrue(game.CastlingRightBlackQueenSide);
    }

    [TestMethod]
    public void PlayMoveTest_BlackRookMovesFromH8ToG8AfterWhiteCastlingRightsRemainUntouched()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "r3k2r/8/8/8/8/8/8/8",
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.BLACK
        };

        var game = new Game(gameStateModel);
        var from = new Square("H8");
        var to = new Square("G8");

        //Act
        game.PlayMove(from, to, null);

        //Assert
        Assert.IsTrue(game.CastlingRightWhiteKingSide);
        Assert.IsTrue(game.CastlingRightWhiteQueenSide);
    }

    [TestMethod]
    public void PlayMoveTest_NewGameWhitePawnMovesFromE2ToE4HalfMoveClockIs0()
    {
        //Arrange
        var expected = 0;

        var game = new Game(_newGameStateModel);
        var from = new Square("E2");
        var to = new Square("E4");

        //Act
        game.PlayMove(from, to, null);
        var result = game.HalfMoveClock;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_BlackKnightMovesFromE6ToG5OnEmptyBoardHalfMoveClockIs1()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/8/4n3/8/8/8/8/8",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.BLACK
        };

        var expected = 1;
        var game = new Game(gameStateModel);
        var from = new Square("E6");
        var to = new Square("G5");

        //Act
        game.PlayMove(from, to, null);
        var result = game.HalfMoveClock;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_BlackKnightTakesFromE6OnG5HalfMoveClockHasResetAfter()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/8/4n3/6B1/8/8/8/8",
            FullMoveNumber = 1,
            HalfMoveClock = 10,
            SideToMove = Side.BLACK
        };

        var expected = 0;
        var game = new Game(gameStateModel);
        var from = new Square("E6");
        var to = new Square("G5");

        //Act
        game.PlayMove(from, to, null);
        var result = game.HalfMoveClock;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_BlackKnightTakesFromE6OnG5FullMoveNumberStarted1IsNow2()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/8/4n3/6B1/8/8/8/8",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.BLACK
        };

        var expected = 2;
        var game = new Game(gameStateModel);
        var from = new Square("E6");
        var to = new Square("G5");

        //Act
        game.PlayMove(from, to, null);
        var result = game.FullMoveNumber;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_NewGamePawnMovesFromE2ToE4FullMoveNumberWas1IsStill1()
    {
        //Arrange
        var expected = 1;

        var game = new Game(_newGameStateModel);
        var from = new Square("E2");
        var to = new Square("E4");

        //Act
        game.PlayMove(from, to, null);
        var result = game.FullMoveNumber;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_WhitePawnFromE7ToE8PromotionToQueen()
    {
        //Arrange
        var expected = "4Q3/8/8/8/8/8/8/8";

        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/4P3/8/8/8/8/8/8",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("E7");
        var to = new Square("E8");
        var promotion = PieceType.QUEEN;

        //Act
        game.PlayMove(from, to, promotion);
        var result = game.Fen;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_BlackPawnFromD2ToD1PromotionToBishop()
    {
        //Arrange
        var expected = "8/8/8/8/8/8/8/3b4";

        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/8/8/8/8/8/3p4/8",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.BLACK
        };

        var game = new Game(gameStateModel);
        var from = new Square("D2");
        var to = new Square("D1");
        var promotion = PieceType.BISHOP;

        //Act
        game.PlayMove(from, to, promotion);
        var result = game.Fen;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_WhitePawnFromE7ToE8PromotionToKingExceptionThrown()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/4P3/8/8/8/8/8/8",
            EnpassantSquare = null,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("E7");
        var to = new Square("E8");
        var promotion = PieceType.KING;

        //Act
        var result = game.Fen;

        //Assert
        Assert.ThrowsException<PromotionException>(() =>
        {
            //Act
            game.PlayMove(from, to, promotion);
        });
    }

    [TestMethod]
    public void PlayMoveTest_WhitePawnFromE7ToE8PromotionToPawnExceptionThrown()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/4P3/8/8/8/8/8/8",
            EnpassantSquare = null,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("E7");
        var to = new Square("E8");
        var promotion = PieceType.PAWN;

        //Act
        var result = game.Fen;

        //Assert
        Assert.ThrowsException<PromotionException>(() =>
        {
            //Act
            game.PlayMove(from, to, promotion);
        });
    }

    [TestMethod]
    public void PlayMoveTest_WhitePawnFromE7ToE8PromotionNotGivenExceptionThrown()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/4P3/8/8/8/8/8/8",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("E7");
        var to = new Square("E8");

        //Act
        var result = game.Fen;

        //Assert
        Assert.ThrowsException<PromotionException>(() =>
        {
            //Act
            game.PlayMove(from, to, null);
        });
    }

    [TestMethod]
    public void PlayMoveTest_WhiteKingFromE1ToC1Castles()
    {
        //Arrange
        var expected = "8/8/8/8/8/8/8/2KR3R";

        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/8/8/8/8/8/8/R3K2R",
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("E1");
        var to = new Square("C1");

        //Act
        game.PlayMove(from, to, null);
        var result = game.Fen;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_BlackKingFromE8ToG8Castles()
    {
        //Arrange
        var expected = "r4rk1/8/8/8/8/8/8/8";

        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "r3k2r/8/8/8/8/8/8/8",
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.BLACK
        };

        var game = new Game(gameStateModel);
        var from = new Square("E8");
        var to = new Square("G8");

        //Act
        game.PlayMove(from, to, null);
        var result = game.Fen;

        //Assert
        Assert.AreEqual(expected, result);
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

    [TestMethod]
    public void PlayMoveTest_NewGameWhitePawnMovesFromE2ToE4SideIsBlackAfter()
    {
        //Arrange
        var expected = Side.BLACK;

        var game = new Game(_newGameStateModel);
        var from = new Square("E2");
        var to = new Square("E4");

        //Act
        game.PlayMove(from, to, null);
        var result = game.SideToMove;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_BlackQueenMovesFromC4ToC1WhiteKingCheckMatedGameStateBlackVictory()
    {
        //Arrange
        var expected = GameState.BLACK_VICTORY;

        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "8/8/8/8/2q5/8/1r6/4K3",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.BLACK
        };

        var game = new Game(gameStateModel);
        var from = new Square("C4");
        var to = new Square("C1");

        //Act
        game.PlayMove(from, to, null);
        var result = game.State;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_WhiteBishopMovesFromC5ToD6CheckGameStateInProgress()
    {
        //Arrange
        var expected = GameState.IN_PROGRESS;

        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "1k6/8/8/2B5/8/8/8/8",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("C5");
        var to = new Square("D6");

        //Act
        game.PlayMove(from, to, null);
        var result = game.State;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_WhiteQueenMovesFromC5ToB5GameStateDraw()
    {
        //Arrange
        var expected = GameState.DRAW;

        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "k7/7R/8/2Q5/8/8/8/8",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("C5");
        var to = new Square("B5");

        //Act
        game.PlayMove(from, to, null);
        var result = game.State;

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayMoveTest_GameStateIsNotInProgressMoveNull()
    {
        //Arrange
        var gameStateModel = new GameStateModel()
        {
            State = GameState.WHITE_VICTORY,
            Fen = "k7/7R/8/2Q5/8/8/8/8",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        var game = new Game(gameStateModel);
        var from = new Square("C5");
        var to = new Square("B5");

        //Act
        var result = game.PlayMove(from, to, null);

        //Assert
        Assert.AreEqual(null, result);
    }

}