using Microsoft.AspNetCore.Mvc;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo;
using SportsBookAI.Core.Mongo.Repositories;

namespace SportsBookAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly string? _mongoDbConnectionString;

    public TeamsController(IConfiguration configuration)
    {
        _mongoDbConnectionString = configuration.GetConnectionString("MongoDb");
        if (!string.IsNullOrEmpty(_mongoDbConnectionString))
        {
            ConnectionDetails.ConnectionString = _mongoDbConnectionString;
        }
    }

    [HttpGet("{league}")]
    public async Task<IActionResult> GetTeamsByLeague(string league)
    {
        MongoSportsBookRepository repo = new(league.ToUpper().Trim());
        IList<ITeam> allTeams = await repo.TeamRepository.GetAllAsync();
        return Ok(allTeams);
    }
}
