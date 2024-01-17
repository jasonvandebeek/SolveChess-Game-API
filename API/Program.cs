using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolveChess.API.Exceptions;
using SolveChess.API.Service;
using SolveChess.API.Websocket;
using SolveChess.DAL;
using SolveChess.DAL.Model;
using SolveChess.Logic.DAL;
using SolveChess.Logic.Interfaces;
using SolveChess.Logic.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var mysqlConnectionString = Environment.GetEnvironmentVariable("SolveChess_MySQLConnectionString") ?? throw new MissingEnvVariableException("No connection string found in .env variables!");

    options.UseMySQL(mysqlConnectionString);
});

builder.Services.AddScoped<IGameDal, GameDal>();

builder.Services.AddScoped<IClientCommunicationService, WebsocketClientCommunicationService>();

builder.Services.AddScoped<IChessService, ChessService>();


string origins = Environment.GetEnvironmentVariable("SolveChess_CorsUrls") ?? throw new MissingEnvVariableException("No cors urls found in .env variables!");
string[] originArray = origins.Split(';');

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder => builder.WithOrigins(originArray)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSecret = Environment.GetEnvironmentVariable("SolveChess_JwtSecret") ?? throw new MissingEnvVariableException("No jwt secret string found in .env variables!");

    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidIssuer = "SolveChess Authenticator",
        ValidAudience = "SolveChess API",
    };

    options.TokenValidationParameters = tokenValidationParameters;
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["AccessToken"];
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowOrigin");

app.MapHub<SignalrHub>("/websocket");
app.MapControllers();

app.Run();
