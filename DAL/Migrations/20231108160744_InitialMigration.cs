using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Username = table.Column<string>(type: "varchar(255)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    WhitePlayerId = table.Column<string>(type: "varchar(255)", nullable: false),
                    BlackPlayerId = table.Column<string>(type: "varchar(255)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Fen = table.Column<string>(type: "longtext", nullable: false),
                    FullMoveNumber = table.Column<int>(type: "int", nullable: false),
                    HalfMoveClock = table.Column<int>(type: "int", nullable: false),
                    SideToMove = table.Column<int>(type: "int", nullable: false),
                    CastlingRightBlackKingSide = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CastlingRightBlackQueenSide = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CastlingRightWhiteKingSide = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CastlingRightWhiteQueenSide = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EnpassantSquareRank = table.Column<int>(type: "int", nullable: true),
                    EnpassantSquareFile = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_User_BlackPlayerId",
                        column: x => x.BlackPlayerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Game_User_WhitePlayerId",
                        column: x => x.WhitePlayerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Move",
                columns: table => new
                {
                    GameId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false),
                    Notation = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Move", x => new { x.GameId, x.Number, x.Side });
                    table.ForeignKey(
                        name: "FK_Move_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Game_BlackPlayerId",
                table: "Game",
                column: "BlackPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_WhitePlayerId",
                table: "Game",
                column: "WhitePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Move");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
