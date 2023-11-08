using SolveChess.Logic.DTO;

namespace SolveChess.Logic.DAL;

public interface IUserDataDAL
{

    public string? GetUsername(string userID);
    public int? GetUserRating(string userID);
    public UserDTO? GetUser(string userID);

}

