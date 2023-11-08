using SolveChess.API.Models;
using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;
using Microsoft.AspNetCore.Mvc;
using SolveChess.Logic.Service;

namespace SolveChess.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserDataController : ControllerBase
{

    private readonly IUserDataDAL _userDataDAL;

    public UserDataController(IUserDataDAL userDataDAL)
    {
        _userDataDAL = userDataDAL;
    }

    [HttpGet("GetUsername")]
    public IActionResult GetUsername(string userID)
    {
        try
        {
            var userService = new UserService(_userDataDAL);

            string? username = userService.GetUsername(userID);

            if (username != null)
            {
                return Ok(username);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("GetUserRating")]
    public IActionResult GetUserRating(string userID)
    {
        try
        {
            var userService = new UserService(_userDataDAL);

            int? userRating = userService.GetUserRating(userID);

            if (userRating != null)
            {
                return Ok(userRating);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("GetUser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
    public IActionResult GetUser(string userID)
    {
        try
        {
            var userService = new UserService(_userDataDAL);

            UserDTO? user = userService.GetUser(userID);

            if (user != null)
            {
                var response = new UserModel()
                {
                    Username = user.Username,
                    Rating = user.Rating,
                    ProfilePictureUrl = $"{Request.Scheme}://{Request.Host}/api/content/profilepicture/{userID}"
                };

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

