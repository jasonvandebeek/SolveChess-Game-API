using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DAL;
using SolveChess.Logic.Interfaces;
using SolveChess.Logic.Models;

namespace SolveChess.Logic.Service.Tests;

[TestClass]
public class ChessServiceTests
{

    [TestMethod]
    public async Task PlayMoveOnGameTest()
    {
        //Arrange
        var expected = new Move(1, Side.WHITE, "e4");

        var gameDalMock = new Mock<IGameDal>();
        gameDalMock.Setup(dal => dal.GetGameWithId(It.IsAny<string>()))
            .ReturnsAsync
            (
                new GameInfoModel()
                {
                    Id = "123",
                    WhiteSideUserId = "330",
                    BlackSideUserId = "400",
                    Game = new Game(
                        new GameStateModel()
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
                        }
                    )
                }
            );

        var communicationServiceMock = new Mock<IClientCommunicationService>();

        var service = new ChessService(gameDalMock.Object, communicationServiceMock.Object);

        //Act 
        var result = await service.PlayMoveOnGame("123", "330", new Square("e2"), new Square("e4"), null);

        //Assert
        Assert.AreEqual(expected.Number, result?.Number);
        Assert.AreEqual(expected.Side, result?.Side);
        Assert.AreEqual(expected.Notation, result?.Notation);
    }

    [TestMethod]
    public async Task PlayMoveOnGameTest_NoGameWithId()
    {
        //Arrange
        var gameDalMock = new Mock<IGameDal>();
        gameDalMock.Setup(dal => dal.GetGameWithId(It.IsAny<string>()))
            .ReturnsAsync(null as GameInfoModel);

        var communicationServiceMock = new Mock<IClientCommunicationService>();

        var service = new ChessService(gameDalMock.Object, communicationServiceMock.Object);

        //Act 
        var result = await service.PlayMoveOnGame("123", "330", new Square("e2"), new Square("e4"), null);

        //Assert
        Assert.AreEqual(null, result);
    }

    [TestMethod]
    public async Task PlayMoveOnGameTest_UserHasNoAccessToGame()
    {
        //Arrange
        var gameDalMock = new Mock<IGameDal>();
        gameDalMock.Setup(dal => dal.GetGameWithId(It.IsAny<string>()))
            .ReturnsAsync
            (
                new GameInfoModel()
                {
                    Id = "123",
                    WhiteSideUserId = "444",
                    BlackSideUserId = "400",
                    Game = new Game(
                        new GameStateModel()
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
                        }
                    )
                }
            );

        var communicationServiceMock = new Mock<IClientCommunicationService>();

        var service = new ChessService(gameDalMock.Object, communicationServiceMock.Object);

        //Act 
        var result = await service.PlayMoveOnGame("123", "330", new Square("e2"), new Square("e4"), null);

        //Assert
        Assert.AreEqual(null, result);
    }

    [TestMethod]
    public async Task PlayMoveOnGameTest_UserIsntToMove()
    {
        //Arrange
        var gameDalMock = new Mock<IGameDal>();
        gameDalMock.Setup(dal => dal.GetGameWithId(It.IsAny<string>()))
            .ReturnsAsync
            (
                new GameInfoModel()
                {
                    Id = "123",
                    WhiteSideUserId = "330",
                    BlackSideUserId = "400",
                    Game = new Game(
                        new GameStateModel()
                        {
                            State = GameState.IN_PROGRESS,
                            Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
                            CastlingRightBlackKingSide = true,
                            CastlingRightBlackQueenSide = true,
                            CastlingRightWhiteKingSide = true,
                            CastlingRightWhiteQueenSide = true,
                            FullMoveNumber = 2,
                            HalfMoveClock = 0,
                            SideToMove = Side.BLACK
                        }
                    )
                }
            );

        var communicationServiceMock = new Mock<IClientCommunicationService>();

        var service = new ChessService(gameDalMock.Object, communicationServiceMock.Object);

        //Act 
        var result = await service.PlayMoveOnGame("123", "330", new Square("e2"), new Square("e4"), null);

        //Assert
        Assert.AreEqual(null, result);
    }

    [TestMethod]
    public async Task GetPlayedMovesForGame()
    {
        //Arrange
        var expected = new List<Move>() { new Move(1, Side.WHITE, "e4") };

        var gameDalMock = new Mock<IGameDal>();
        gameDalMock.Setup(dal => dal.GetMovesForGame(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var communicationServiceMock = new Mock<IClientCommunicationService>();

        var service = new ChessService(gameDalMock.Object, communicationServiceMock.Object);

        //Act
        var result = await service.GetPlayedMovesForGame("123");

        //Assert
        CollectionAssert.AreEquivalent(expected, result?.ToList());
    }

    [TestMethod]
    public async Task CreateNewGame()
    {
        //Arrange
        var gameDalMock = new Mock<IGameDal>();
        var communicationServiceMock = new Mock<IClientCommunicationService>();

        var service = new ChessService(gameDalMock.Object, communicationServiceMock.Object);

        //Act
        var result = await service.CreateNewGame("123", "321", "123");

        //Assert
        Assert.AreEqual(typeof(String), result?.GetType());
    }

    [TestMethod]
    public async Task CreateNewGame_BothPlayersIdsAreTheSame()
    {
        //Arrange
        var gameDalMock = new Mock<IGameDal>();
        var communicationServiceMock = new Mock<IClientCommunicationService>();

        var service = new ChessService(gameDalMock.Object, communicationServiceMock.Object);

        //Act
        var result = await service.CreateNewGame("123", "123", "123");

        //Assert
        Assert.AreEqual(null, result);
    }

    [TestMethod]
    public async Task CreateNewGame_WhiteSideIdIsNonOfGivenPlayers()
    {
        //Arrange
        var gameDalMock = new Mock<IGameDal>();
        var communicationServiceMock = new Mock<IClientCommunicationService>();

        var service = new ChessService(gameDalMock.Object, communicationServiceMock.Object);

        //Act
        var result = await service.CreateNewGame("123", "321", "222");

        //Assert
        Assert.AreEqual(null, result);
    }

    [TestMethod]
    public async Task GetGameWithId()
    {
        //Arrange
        var expected = new GameInfoModel()
        {
            Id = "123",
            WhiteSideUserId = "330",
            BlackSideUserId = "400",
            Game = new Game(new GameStateModel()
            {
                State = GameState.IN_PROGRESS,
                Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
                CastlingRightBlackKingSide = true,
                CastlingRightBlackQueenSide = true,
                CastlingRightWhiteKingSide = true,
                CastlingRightWhiteQueenSide = true,
                FullMoveNumber = 2,
                HalfMoveClock = 0,
                SideToMove = Side.BLACK
            })
        };

        var gameDalMock = new Mock<IGameDal>();
        gameDalMock.Setup(dal => dal.GetGameWithId(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var communicationServiceMock = new Mock<IClientCommunicationService>();

        var service = new ChessService(gameDalMock.Object, communicationServiceMock.Object);

        //Act 
        var result = await service.GetGameWithId("123");

        //Assert
        Assert.AreEqual(expected, result);
    }

}