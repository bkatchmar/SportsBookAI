using Microsoft.AspNetCore.Mvc;
using SportsBookAI.Api.Models;
using SportsBookAI.Core.Classes;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo;
using SportsBookAI.Core.Mongo.Repositories;

namespace SportsBookAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AggregatorController : ControllerBase
{
    private readonly string? _mongoDbConnectionString;

    public AggregatorController(IConfiguration configuration)
    {
        _mongoDbConnectionString = configuration.GetConnectionString("MongoDb");
        if (!string.IsNullOrEmpty(_mongoDbConnectionString) && string.IsNullOrEmpty(ConnectionDetails.ConnectionString))
        {
            ConnectionDetails.ConnectionString = _mongoDbConnectionString;
        }
    }

    [HttpGet("{league}")]
    public async Task<IActionResult> GetTotalAggregationByLeague(string league)
    {
        string leagueName = league.ToUpper().Trim();
        MongoSportsBookRepository repo = new(leagueName);
        IAggregator baseAggregatorLeagueData = new BaseAggregator(leagueName, repo);
        await baseAggregatorLeagueData.AggregateAsync();

        AggregationReturnModel rtnVal = new(baseAggregatorLeagueData);
        return Ok(rtnVal);
    }
}
