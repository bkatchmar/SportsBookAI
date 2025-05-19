using Microsoft.AspNetCore.Mvc;
using SportsBookAI.Api.Models;
using SportsBookAI.Core.Classes;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo;
using SportsBookAI.Core.Mongo.Base;
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

    [HttpPost("getPredictions")]
    public async Task<IActionResult> GetPredictions(PredictionRequest predictionReq)
    {
        string leagueName = predictionReq.LeagueName.ToUpper().Trim();
        MongoSportsBookRepository repo = new(leagueName);
        IAggregator baseAggregatorLeagueData = new BaseAggregator(leagueName, repo);
        await baseAggregatorLeagueData.AggregateAsync();

        // Make a mock match
        IMatch? lookupMatch = repo.MatchRepository.GetById(predictionReq.MatchId);

        if (lookupMatch == null)
        {
            return NotFound("Match Not Found");
        }

        IPatternRepo basePatternRepo = new BasePatternRepo(baseAggregatorLeagueData);
        IList<IPredictionPattern> currentPredictions = basePatternRepo.GetAllPredictions([lookupMatch]);

        return Ok(currentPredictions);
    }
}
