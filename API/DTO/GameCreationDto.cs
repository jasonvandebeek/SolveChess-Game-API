using System;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.Models;

public class GameCreationDto
{

    [Required]
    public string PlayerOneUserId { get; set; } = null!;
    [Required]
    public string PlayerTwoUserId { get; set; } = null!;

    public string? WhiteSideUserId { get; set; }

}


