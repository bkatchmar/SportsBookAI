using Microsoft.Extensions.Configuration;
using SportsBookAI.Core.Classes;
using SportsBookAI.Core.Classes.Patterns;
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

        MongoSportsBookRepository repo = new(repos.Key);
        IList<IMatch> allMatches = await repo.MatchRepository.GetAllAsync();

        BaseAggregator baseAggregatorLeagueData = new(repos.Key, repo);
        await baseAggregatorLeagueData.AggregateAsync();

        IList<IMatch> matchesThatNeedPredictions = [.. allMatches.Where(m => baseAggregatorLeagueData.DoesThisMatchNeedOverUnderPrediction(m))];

        foreach (IMatch match in matchesThatNeedPredictions)
        {
            Console.WriteLine(match.ToString());

            // TakeAverageOverUnderMarkIntoConsiderationBetweenTwoTeams newPredictionPattern = new(baseAggregatorLeagueData, match, 25, DateTime.Today, MarkUsing: 173);
            // Console.WriteLine(newPredictionPattern.PredictionText);
            // Highest ID; 27
        }

        Console.WriteLine("==================\n");
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