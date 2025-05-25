using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SportsBookAI.Api.Models;
using SportsBookAI.Core.Classes;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo;
using SportsBookAI.Core.Mongo.Repositories;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SportsBookAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AggregatorController : ControllerBase
{
    private readonly string? _mongoDbConnectionString;
    private Dictionary<string, DateTime> _openingDays;
    private const int SEVEN_DAYS = 7;
    private const int FOURTEEN_DAYS = 14;

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
    public async Task<IActionResult> GetTotalAggregationByLeague(string league, bool? usesWeeks, bool? usesPithcers)
    {
        string leagueName = league.ToUpper().Trim();
        MongoSportsBookRepository repo = new(leagueName);

        // TODO: Maybe make this a `IAggregator` factory
        IAggregator baseAggregatorLeagueData;
        BaseAggregator basicLevel = new(leagueName, repo);
        if (usesWeeks.HasValue && usesWeeks.Value)
        {
            baseAggregatorLeagueData = new AmericanFootballAggregator(basicLevel);
        }
        else if (usesPithcers.HasValue && usesPithcers.Value)
        {
            baseAggregatorLeagueData = new BaseAggregator(leagueName, repo);
        }
        else
        {
            baseAggregatorLeagueData = basicLevel;
        }
        await baseAggregatorLeagueData.AggregateAsync();

        AggregationReturnModel rtnVal = new(baseAggregatorLeagueData);
        return Ok(rtnVal);
    }

    [HttpGet("{league}/{team}")]
    public async Task<IActionResult> GetTotalAggregationByLeague(string league, string team)
    {
        string leagueName = league.ToUpper().Trim();
        string normalizedTeamName = SlugToNormalCase(team);
        MongoSportsBookRepository repo = new(leagueName);
        ITeam? lookup = repo.TeamRepository.GetByName(normalizedTeamName);

        // First check if I can get all and just find the team name if I were to "slug" it
        if (lookup == null)
        {
            lookup = repo.TeamRepository.GetAll().FirstOrDefault(t => NormalCaseToSlug(t.TeamName).Equals(team.ToLower()));
        }

        if (lookup == null)
        {
            return NotFound($"{normalizedTeamName} for {league} not found");
        }

        TeamSpecificAggregator forTeam = new(leagueName, lookup, repo);
        await forTeam.AggregateAsync();

        AggregationReturnModel rtnVal = new(forTeam);
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
            if (daysPassed >= SEVEN_DAYS)
            {
                BaseAggregator pastSeventDays = new(leagueName, repo, TODAY, SEVEN_DAYS);
                await pastSeventDays.AggregateAsync();
                allPatternRepos.Add(new SevenDayRangePatternRepo(pastSeventDays, TODAY));
            }

            // Put in 14 day lookups for `allPatternRepos`
            if (daysPassed >= FOURTEEN_DAYS)
            {
                BaseAggregator pastSeventDays = new(leagueName, repo, TODAY, FOURTEEN_DAYS);
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

    private static string SlugToNormalCase(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return slug;

        string[] words = slug.Split('-');
        IEnumerable<string> capitalizedWords = words.Select(word =>
            char.ToUpperInvariant(word[0]) + word.Substring(1).ToLowerInvariant());

        return string.Join(" ", capitalizedWords);
    }

    private static string NormalCaseToSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        // Normalize the string to remove accents (e.g., "é" => "e")
        string normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char c in normalized)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        string cleaned = sb.ToString().Normalize(NormalizationForm.FormC);

        // Convert to lowercase
        cleaned = cleaned.ToLowerInvariant();

        // Replace all non-alphanumeric characters with hyphens
        cleaned = Regex.Replace(cleaned, @"[^a-z0-9]+", "-");

        // Trim any leading or trailing hyphens
        cleaned = cleaned.Trim('-');

        return cleaned;
    }
}
