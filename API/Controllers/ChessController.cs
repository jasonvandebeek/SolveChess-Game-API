using SolveChess.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SolveChess.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChessController : Controller
{

    [HttpPost("MakeMove")]
    public async Task<IActionResult> MakeMove([FromQuery] string gameId, [FromBody] MoveDataModel moveData)
    {
        //Go through logic

        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7001/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var data = new
        {
            moveData.From,
            moveData.To,
            Notation = ""
        };

        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"api/websocket/Move/SendMove?gameId={gameId}", content);

        if (response.IsSuccessStatusCode)
        {
            return Ok("Move processed successfully.");
        }
        else
        {
            return BadRequest(response);
        }
    }

}

