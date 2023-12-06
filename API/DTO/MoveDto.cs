using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.DTO;

public class MoveDto
{

    [Required]
    public int Number { get; set; }

    [Required]
    public string Side { get; set; } = null!;

    [Required]
    public string Notation { get; set; } = null!;

}
