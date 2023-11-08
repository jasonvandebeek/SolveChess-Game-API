
using SolveChess.Logic.Chess.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolveChess.DAL.Model;

public class Move
{

    [ForeignKey("GameId")]
    public Game Game { get; set; }
    [Key]
    public string GameId {  get; set; }

    [Key]
    public int Number { get; set; }
    [Key]
    public Side Side { get; set; }

    public string Notation { get; set; }
 
}

