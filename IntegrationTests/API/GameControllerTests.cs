using IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolveChess.API.DTO;
using SolveChess.DAL.Model;
using SolveChess.IntegrationTests.Helpers;
using SolveChess.Logic.Chess.Attributes;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

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
            .UseInMemoryDatabase("TestDatabase");

        _dbContext = new AppDbContext(optionsBuilder.Options);
    }

    [TestMethod]
    public async Task CreateGame_Returns201_WhenUserIsAuthenticated()
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

        if (response.Headers.Location == null)
            Assert.Fail();

        var gameId = UriHelper.ExtractGameIdFromCreatedLocationUri(response.Headers.Location);

        if (gameId == null)
            Assert.Fail();

        //Assert
        Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

        var createdGame = _dbContext.Game.FirstOrDefault(g => g.Id == gameId);
        Assert.IsNotNull(createdGame);
        Assert.AreEqual(createdGame.WhiteSideUserId, game.WhiteSideUserId);
        Assert.AreEqual(createdGame.BlackSideUserId, game.OpponentUserId);
    }

    [TestMethod]
    public async Task GetGame_Returns200AndGameData()
    {
        //Arrange
        var json = new
        {
            id = "200",
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
            Id = "200",
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

        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync($"/game/{gameModel.Id}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.AreEqual(expected, result);
    }


}

