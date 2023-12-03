
using SolveChess.Logic.Chess;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.Logic.Models;

public class GameInfoModel
{

    [Required]
    public string Id { get; set; } = null!;

    [Required]
    public string WhiteSideUserId { get; set; } = null!;
    [Required]
    public string BlackSideUserId { get; set; } = null!;

    [Required]
    public Game Game { get; set; } = null!;

} 

