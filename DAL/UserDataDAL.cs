using Microsoft.EntityFrameworkCore;
using SolveChess.DAL.Model;
using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;

namespace SolveChess.DAL;

public class UserDataDAL : AppDbContext, IUserDataDAL
{

    public UserDataDAL(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public string? GetUsername(string userID)
    {
        return User
            .Where(u => u.Id == userID)
            .Select(u => u.Username)
            .FirstOrDefault();
    }

    public int? GetUserRating(string userID)
    {
        return User
            .Where(u => u.Id == userID)
            .Select(u => u.Rating)
            .FirstOrDefault();
    }

    public UserDTO? GetUser(string userID)
    {
        var user = User.Where(u => u.Id == userID).FirstOrDefault();

        if (user == null)
            return null;

        var userDTO = new UserDTO()
        {
            Username = user.Username,
            Rating = user.Rating,
        };

        return userDTO;
    }

}

