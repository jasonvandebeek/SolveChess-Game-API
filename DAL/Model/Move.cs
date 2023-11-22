
using SolveChess.Logic.Chess.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolveChess.DAL.Model;

public class Move
{

    [ForeignKey("GameId")]
    public Game Game { get; set; } = null!;
    [Key]
    public string GameId { get; set; } = null!;

    [Key]
    public int Number { get; set; }
    [Key]
    public Side Side { get; set; }

    [Required]
    public string Notation { get; set; } = null!;

}

