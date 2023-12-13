using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlX.XDevAPI;
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

    private SolveChessWebApplicationFactory _factory = null!;
    private AppDbContext _dbContext = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new SolveChessWebApplicationFactory();

        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    [TestMethod]
    public async Task CreateGame_Returns201CreatedUserIsAuthenticated()
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

        var createdGame = await _dbContext.Game.FirstOrDefaultAsync(g => g.Id == gameId);
        Assert.IsNotNull(createdGame);
        Assert.AreEqual(createdGame.WhiteSideUserId, game.WhiteSideUserId);
        Assert.AreEqual(createdGame.BlackSideUserId, game.OpponentUserId);
    }

    [TestMethod]
    public async Task CreateGame_Returns401Unauthorized()
    {
        //Arrange
        var userId = "123";
        var client = _factory.CreateClient();

        var game = new
        {
            OpponentUserId = "331",
            WhiteSideUserId = userId
        };

        string jsonBody = JsonConvert.SerializeObject(game);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //Act
        var response = await client.PostAsync("/game", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task GetGame_Returns200OkAndGameData()
    {
        //Arrange
        var gameId = "300";

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
    public async Task GetMoves_Returns200OkAndMovesList()
    {
        //Arrange
        var json = new[]
        {
            new { number = 1, side = "WHITE", notation = "e4" },
            new { number = 1, side = "BLACK", notation = "c6" }
        };
        string expected = JsonConvert.SerializeObject(json, Formatting.None);

        var gameId = "200";

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
    public async Task PlayMove_Returns200Ok()
    {
        //Arrange
        var gameId = "100";

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
            from = new { rank = 6, file = 4 },
            to = new { rank = 4, file = 4 },
            promotion = null as object
        };

        string jsonBody = JsonConvert.SerializeObject(move);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //Act
        var response = await client.PostAsync($"/game/{gameId}/move", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var game = await _dbContext.Game.FirstOrDefaultAsync(g => g.Id == gameId);
        Assert.IsNotNull(game);
        _dbContext.Entry(game).Reload();

        Assert.AreEqual("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR", game.Fen);
        Assert.AreEqual(Side.BLACK, game.SideToMove);

        var moves = await _dbContext.Move.Where(m => m.GameId == gameId).ToListAsync();
        Assert.IsNotNull(moves);
        Assert.AreEqual(1, moves[0].Number);
        Assert.AreEqual(Side.WHITE, moves[0].Side);
        Assert.AreEqual("e4", moves[0].Notation);
    }

    [TestMethod]
    public async Task PlayMove_Returns401Unauthorized()
    {
        //Arrange
        var gameId = "100";
        var client = _factory.CreateClient();

        var move = new
        {
            from = new { rank = 6, file = 4 },
            to = new { rank = 4, file = 4 },
            promotion = null as object
        };

        string jsonBody = JsonConvert.SerializeObject(move);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //Act
        var response = await client.PostAsync($"/game/{gameId}/move", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

}

