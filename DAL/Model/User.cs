using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.DAL.Model;

public class User
{

    [Key]
    public string Id { get; set; }

    public string Username { get; set; }

    [DefaultValue(100)]
    public int Rating { get; set; }

}

