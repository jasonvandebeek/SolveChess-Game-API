
using SolveChess.Logic.Chess;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.Logic.DTO;

public class GameInfoModel
{

    [Required]
    public string Id { get; set; } = null!;

    [Required]
    public string WhitePlayerId { get; set; } = null!;
    [Required]
    public string BlackPlayerId { get; set; } = null!;

    [Required]
    public Game Game { get; set; } = null!;

} 

