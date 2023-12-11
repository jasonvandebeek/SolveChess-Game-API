using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DAL;
using SolveChess.Logic.Interfaces;
using SolveChess.Logic.Models;
using SolveChess.Logic.Service;

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

}