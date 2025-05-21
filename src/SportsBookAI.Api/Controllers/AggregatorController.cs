using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    private Dictionary<string, DateTime> _openingDays;

    public AggregatorController(IOptions<LeaguesWithDataSetting> options, IConfiguration configuration)
    {
        _mongoDbConnectionString = configuration.GetConnectionString("MongoDb");
        _openingDays = options.Value.OpeningDays;
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
        BaseAggregator baseAggregatorLeagueData = new(leagueName, repo);
        await baseAggregatorLeagueData.AggregateAsync();

        AggregationReturnModel rtnVal = new(baseAggregatorLeagueData);
        return Ok(rtnVal);
    }

    [HttpPost("getPredictions")]
    public async Task<IActionResult> GetPredictions(PredictionRequest predictionReq)
    {
        string leagueName = predictionReq.LeagueName.ToUpper().Trim();
        DateTime TODAY = DateTime.Today;
        MongoSportsBookRepository repo = new(leagueName);
        BaseAggregator baseAggregatorLeagueData = new(leagueName, repo);
        await baseAggregatorLeagueData.AggregateAsync();

        // Make a mock match
        IMatch? lookupMatch = repo.MatchRepository.GetById(predictionReq.MatchId);

        if (lookupMatch == null)
        {
            return NotFound("Match Not Found");
        }

        // Init collection and get the base pattern first, we will add the other pattern repos as needed
        IList<IPatternRepo> allPatternRepos = [];
        allPatternRepos.Add(new BasePatternRepo(baseAggregatorLeagueData));

        if (_openingDays.TryGetValue(leagueName, out DateTime value))
        {
            int daysPassed = (TODAY - value).Days;

            // Put in 7 day lookups for `allPatternRepos`
            if (daysPassed >= 7)
            {
                BaseAggregator pastSeventDays = new(leagueName, repo, TODAY, 7);
                await pastSeventDays.AggregateAsync();
                allPatternRepos.Add(new SevenDayRangePatternRepo(pastSeventDays, TODAY));
            }
        }

        // Collect the predictions themselves
        IList<IPredictionPattern> currentPredictions = [];
        foreach (IPatternRepo patternRepo in allPatternRepos)
        {
            foreach (IPredictionPattern pattern in patternRepo.GetAllPredictions([lookupMatch]))
            {
                currentPredictions.Add(pattern);
            }
        }

        return Ok(currentPredictions);
    }
}
