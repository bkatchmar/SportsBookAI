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
public class TeamsController : ControllerBase
{
    private readonly string? _mongoDbConnectionString;

    public TeamsController(IConfiguration configuration)
    {
        _mongoDbConnectionString = configuration.GetConnectionString("MongoDb");
        if (!string.IsNullOrEmpty(_mongoDbConnectionString) && string.IsNullOrEmpty(ConnectionDetails.ConnectionString))
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

    [HttpGet("{league}/matchesThatNeedPredictions")]
    public async Task<IActionResult> GetMatchesThatNeedPredictions(string league)
    {
        string leagueName = league.ToUpper().Trim();
        MongoSportsBookRepository repo = new(leagueName);
        IList<IMatch> allMatches = await repo.MatchRepository.GetAllAsync();

        IAggregator baseAggregatorLeagueData = new BaseAggregator(leagueName, repo);
        await baseAggregatorLeagueData.AggregateAsync();

        IList<IMatch> matchesThatNeedPredictions = allMatches.Where(m => baseAggregatorLeagueData.DoesThisMatchNeedOverUnderPrediction(m)).ToList();
        IList<ApiMatch> rtnVal = [];

        foreach (IMatch matchFromRepo in matchesThatNeedPredictions)
        {
            if (matchFromRepo is MongoMatch forApi)
            {
                rtnVal.Add(new()
                {
                    Id = forApi.Id,
                    HomeTeam = forApi.HomeTeam,
                    AwayTeam = forApi.AwayTeam,
                    MatchDateTimeUTC = forApi.MatchDateTimeUTC,
                    MatchDateTimeLocal = forApi.MatchDateTimeLocal,
                    WeekNumber = forApi.WeekNumber
                });
            }
        }

        return Ok(rtnVal);
    }
}
