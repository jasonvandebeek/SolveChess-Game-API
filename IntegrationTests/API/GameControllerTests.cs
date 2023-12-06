using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SolveChess.DAL.Model;
using SolveChess.IntegrationTests.Helpers;
using SolveChess.Logic.Chess.Attributes;
using System.Net;
using System.Text;

namespace SolveChess.API.IntegrationTests;

[TestClass]
public class GameControllerTests
{

    private readonly SolveChessWebApplicationFactory _factory;
    private readonly AppDbContext _dbContext;

    public GameControllerTests()
    {
        _factory = new SolveChessWebApplicationFactory();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .ConfigureWarnings(warnings =>
            {
                warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning);
            });

        _dbContext = new AppDbContext(optionsBuilder.Options);
    }

    [TestMethod]
    public async Task CreateGame_Returns201Created_WhenUserIsAuthenticated()
    {
        //Arrange
        var userId = "123";
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var game = new
        {
            OpponentUserId = "331",
            WhiteSideUserId = userId
        };

        string jsonBody = JsonConvert.SerializeObject(game);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //Act
        var response = await client.PostAsync("/game", content);
        response.EnsureSuccessStatusCode();

        var gameId = await response.Content.ReadAsStringAsync();

        if (gameId == null)
            Assert.Fail();

        //Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var createdGame = _dbContext.Game.FirstOrDefault(g => g.Id == gameId);
        Assert.IsNotNull(createdGame);
        Assert.AreEqual(createdGame.WhiteSideUserId, game.WhiteSideUserId);
        Assert.AreEqual(createdGame.BlackSideUserId, game.OpponentUserId);
    }

    [TestMethod]
    public async Task CreateGame_Returns400BadRequest_WhenOpponentIdIsTheSameAsUserId()
    {
        //Arrange
        var userId = "123";
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var game = new
        {
            OpponentUserId = "123"
        };

        string jsonBody = JsonConvert.SerializeObject(game);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //Act
        var response = await client.PostAsync("/game", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task GetGame_Returns200Ok_AndGameData()
    {
        //Arrange
        var gameId = Guid.NewGuid().ToString();

        var json = new
        {
            id = gameId,
            whiteSideUserId = "123",
            blackSideUserId = "231",
            state = "IN_PROGRESS",
            fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
            sideToMove = "WHITE",
            castlingRightBlackKingSide = true,
            castlingRightBlackQueenSide = true,
            castlingRightWhiteKingSide = true,
            castlingRightWhiteQueenSide = true,
            enpassantSquare = null as object
        };
        string expected = JsonConvert.SerializeObject(json, Formatting.None);

        GameModel gameModel = new()
        {
            Id = gameId,
            WhiteSideUserId = "123",
            BlackSideUserId = "231",
            State = GameState.IN_PROGRESS,
            Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE,
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            EnpassantSquareRank = null,
            EnpassantSquareFile = null
        };

        _dbContext.Game.Add(gameModel);
        _dbContext.SaveChanges();

        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync($"/game/{gameId}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task GetGame_Returns404NotFound_WhenGameIsNonExistentInDatabase()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync($"/game/6000");

        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task GetMoves_Returns200Ok_AndMovesList()
    {
        //Arrange
        var json = new[]
        {
            new { number = 1, side = "WHITE", notation = "e4" },
            new { number = 1, side = "BLACK", notation = "c6" }
        };
        string expected = JsonConvert.SerializeObject(json, Formatting.None);

        var gameId = Guid.NewGuid().ToString();

        GameModel gameModel = new()
        {
            Id = gameId,
            WhiteSideUserId = "123",
            BlackSideUserId = "231",
            State = GameState.IN_PROGRESS,
            Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE,
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            EnpassantSquareRank = null,
            EnpassantSquareFile = null
        };

        _dbContext.Game.Add(gameModel);

        MoveModel moveOne = new()
        {
            GameId = gameId,
            Number = 1,
            Side = Side.WHITE,
            Notation = "e4"
        };

        MoveModel moveTwo = new()
        {
            GameId = gameId,
            Number = 1,
            Side = Side.BLACK,
            Notation = "c6"
        };

        _dbContext.Move.Add(moveOne);
        _dbContext.Move.Add(moveTwo);

        await _dbContext.SaveChangesAsync();

        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync($"/game/{gameId}/moves");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task GetMoves_Returns404NotFound_WhenGameIsNonExistentInDatabase()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync($"/game/6000/moves");

        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task PlayMove_Returns200Ok()
    {
        //Arrange
        var gameId = Guid.NewGuid().ToString();

        GameModel gameModel = new()
        {
            Id = gameId,
            WhiteSideUserId = "123",
            BlackSideUserId = "231",
            State = GameState.IN_PROGRESS,
            Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE,
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            EnpassantSquareRank = null,
            EnpassantSquareFile = null
        };

        _dbContext.Game.Add(gameModel);
        await _dbContext.SaveChangesAsync();

        var userId = "123";
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var move = new
        {
            from = new
            {
                rank = 6,
                file = 4
            },
            to = new 
            { 
                rank = 4,
                file = 4
            },
            promotion = null as object
        };

        string jsonBody = JsonConvert.SerializeObject(move);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //Act
        var response = await client.PostAsync($"/game/{gameId}/move", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task PlayMove_Returns400BadRequest_WhenUserHasNoGameAccess()
    {
        //Arrange
        var gameId = Guid.NewGuid().ToString();

        GameModel gameModel = new()
        {
            Id = gameId,
            WhiteSideUserId = "123",
            BlackSideUserId = "231",
            State = GameState.IN_PROGRESS,
            Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE,
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            EnpassantSquareRank = null,
            EnpassantSquareFile = null
        };

        _dbContext.Game.Add(gameModel);
        await _dbContext.SaveChangesAsync();

        var userId = "300";
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var move = new
        {
            from = new
            {
                rank = 6,
                file = 4
            },
            to = new
            {
                rank = 4,
                file = 4
            },
            promotion = null as object
        };

        string jsonBody = JsonConvert.SerializeObject(move);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //Act
        var response = await client.PostAsync($"/game/{gameId}/move", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

}

