using System;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.Models;

public class GameCreationDto
{

    [Required]
    public string OpponentUserId { get; set; } = null!;

    public string? WhiteSideUserId { get; set; }

}


