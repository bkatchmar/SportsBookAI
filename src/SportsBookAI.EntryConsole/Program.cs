using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using SportsBookAI.Core.Classes;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo;
using SportsBookAI.Core.Mongo.Repositories;
using SportsBookAI.EntryConsole.SettingsModels;

// Program Start
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Read MongoDB settings from configuration
AppSetting? mainSetting = configuration.GetSection("MainSetting").Get<AppSetting>();
OutputSettings? outputSettings = configuration.GetSection("OutputSettings").Get<OutputSettings>();
Dictionary<string, DateTime>? openingDays = configuration.GetSection("OpeningDays").Get<Dictionary<string, DateTime>>();

Console.WriteLine("Getting high level settings\n");

// Use the currently selection connection and do the necessary setup
Dictionary<string, ISportsBookRepository> reposByLeague = [];

switch (mainSetting?.CurrentConnection)
{
    case "MongoDB":
        Connection? mongoConnection = mainSetting?.Connections.FirstOrDefault(conn => conn.Key == mainSetting?.CurrentConnection);
        if (mongoConnection != null && mainSetting != null)
        {
            ConnectionDetails.ConnectionString = mongoConnection.ConnectionString;
            foreach (string league in mainSetting.Leagues)
            {
                MongoSportsBookRepository repo = new(league);
                reposByLeague.Add(league, repo);
            }
            Console.WriteLine("Mongo Connection Set\n");
        }
        else
        {
            Console.WriteLine("Mongo Connection Not Set - Check Your Appsettings File\n");
        }
        break;
    default:
        break;
}

DateTime TODAY = DateTime.Today;

// Connection set, ready to roll out!
if (reposByLeague.Count > 0)
{
    foreach (KeyValuePair<string, ISportsBookRepository> repos in reposByLeague)
    {
        Console.WriteLine($"League = {repos.Key}\n");
        IList<ITeam> allTeams = await repos.Value.TeamRepository.GetAllAsync();
        IList<IMatch> allMatches = await repos.Value.MatchRepository.GetAllAsync();
        IList<IOverUnder> allOverUnderMarks = await repos.Value.OverUnderRepository.GetAllAsync();
        IList<IPointSpread> allPointSpreads = await repos.Value.PointSpreadRepository.GetAllAsync();
        Console.WriteLine($"{repos.Key} Has {allTeams.Count} Teams\n");
        foreach (ITeam team in allTeams)
        {
            Console.WriteLine(team);
        }
        Console.WriteLine("");
        Console.WriteLine($"Stored {allMatches.Count} Matches");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine($"Stored {allPointSpreads.Count} Point Spread Records");
        Console.WriteLine("");
        Console.WriteLine("===========\n");

        Console.WriteLine("Lets make some preidctions!\n");

        IAggregator baseAggregatorLeagueData = new BaseAggregator(repos.Key, repos.Value);
        baseAggregatorLeagueData.Aggregate();

        IList<IMatch> matchesThatNeedPredictions = allMatches.Where(m => baseAggregatorLeagueData.DoesThisMatchNeedOverUnderPrediction(m)).ToList();
        Console.WriteLine($"I need to make predictions for {matchesThatNeedPredictions.Count} Matches");

        // Get All necessary pattern repositories, base pattern repo to start off with
        List<IPatternRepo> allPatternRepos = [new BasePatternRepo(baseAggregatorLeagueData)];

        // TODO: Start looking at possible ways to grab various date range patterns, like a last 14 days, some leagues fair better looking at this range than others
        if (openingDays != null && openingDays.TryGetValue(repos.Key, out DateTime value))
        {
            int daysPassed = (TODAY - value).Days;

            // Put in 7 day lookups for `allPatternRepos`
            if (daysPassed >= 7)
            {
                IAggregator pastSeventDays = new BaseAggregator(repos.Key, repos.Value, TODAY, 7);
                pastSeventDays.Aggregate();
                allPatternRepos.Add(new SevenDayRangePatternRepo(pastSeventDays, TODAY));
            }

            // Put in 14 day lookups for `allPatternRepos`
            if (daysPassed >= 14)
            {
                IAggregator pastFourteenDays = new BaseAggregator(repos.Key, repos.Value, TODAY, 14);
                pastFourteenDays.Aggregate();
                allPatternRepos.Add(new SevenDayRangePatternRepo(pastFourteenDays, TODAY));
            }

            // Put in 21 day lookups for `allPatternRepos`
            if (daysPassed >= 21)
            {
                IAggregator pastTwentyOneDays = new BaseAggregator(repos.Key, repos.Value, TODAY, 21);
                pastTwentyOneDays.Aggregate();
                allPatternRepos.Add(new SevenDayRangePatternRepo(pastTwentyOneDays, TODAY));
            }
        }

        // Collect and write all predictions
        List<IPredictionPattern> allPredictions = [];
        foreach (IPatternRepo patterns in allPatternRepos)
        {
            // Time to make some predictions
            IList<IPredictionPattern> currentPredictions = patterns.GetAllPredictions(matchesThatNeedPredictions);
            foreach (IPredictionPattern pattern in currentPredictions)
            {
                Console.WriteLine(pattern.PredictionText);
            }
            allPredictions.AddRange(currentPredictions);
        }

        Console.WriteLine("\nWrite To File\n");
        if (!string.IsNullOrEmpty(outputSettings?.FileDestinationToWriteTo))
        {
            using StreamWriter writer = new(string.Concat(outputSettings?.FileDestinationToWriteTo, repos.Key, " ", "Predictions.csv"));
            using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(allPredictions);
        }
    }
}
else
{
    Console.WriteLine("Error when setting the main sportsbook repo");
}

// Run needed disposing
foreach (KeyValuePair<string, ISportsBookRepository> repos in reposByLeague)
{
    if (repos.Value is IDisposable obj)
    {
        obj.Dispose();
    }
}