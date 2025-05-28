using System.Globalization;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using SportsBookAI.Core.Classes;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo;
using SportsBookAI.Core.Mongo.Repositories;
using SportsBookAI.Core.Structs;
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

        IAggregator baseAggregatorLeagueData = GetAggregator(repos);
        await baseAggregatorLeagueData.AggregateAsync();
        await PrintBaseAggregatorStats(baseAggregatorLeagueData);

        // San Antonio Brahmas Specific Example
        Console.WriteLine("Lets Get Specific Stats Focusing on the 'San Antonio Brahmas'");
        IAggregator brahmasData = GetAggregator(repos, "San Antonio Brahmas");
        await brahmasData.AggregateAsync();
        await PrintBaseAggregatorStats(brahmasData);

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

static IAggregator GetAggregator(KeyValuePair<string, ISportsBookRepository> CurrentRepo, string TeamToTarget = "")
{
    if (!string.IsNullOrEmpty(TeamToTarget))
    {
        ITeam? lookup = CurrentRepo.Value.TeamRepository.GetByName(TeamToTarget);
        if (lookup != null)
        {
            return new TeamSpecificAggregator(CurrentRepo.Key, lookup, CurrentRepo.Value);
        }
    }

    return new BaseAggregator(CurrentRepo.Key, CurrentRepo.Value);
}

static async Task PrintBaseAggregatorStats(IAggregator Aggregator)
{
    Console.WriteLine($"Total Over Percentage: {Aggregator.AllOverPercentage.ToString("P2")}");
    Console.WriteLine($"Total Under Percentage: {Aggregator.AllUnderPercentage.ToString("P2")}");
    Console.WriteLine($"Total Minus Percentage: {Aggregator.AllMinusSpreadsPercentage.ToString("P2")}");
    Console.WriteLine($"Total Plus Percentage: {Aggregator.AllPlusSpreadsPercentage.ToString("P2")}");
    Console.WriteLine($"Highest Over: {Aggregator.HighestOverHit.ToString("N2")}");
    Console.WriteLine($"Lowest Over: {Aggregator.LowestOverHit.ToString("N2")}");
    Console.WriteLine($"Average Over Mark: {Aggregator.AverageOverHit.ToString("N2")}");
    Console.WriteLine($"Highest Under: {Aggregator.HighestUnderHit.ToString("N2")}");
    Console.WriteLine($"Lowest Under: {Aggregator.LowestUnderHit.ToString("N2")}");
    Console.WriteLine($"Average Under Mark: {Aggregator.AverageUnderHit.ToString("N2")}\n");

    IList<ITeam> allTeamsFromLeague = await Aggregator.Repo.TeamRepository.GetAllAsync();

    foreach (ITeam team in allTeamsFromLeague)
    {
        Console.WriteLine($"Stats For {team}\n");

        if (Aggregator.OversByTeam.TryGetValue(team.TeamName, out int overMarks)) Console.WriteLine($"{overMarks} Over Mark(s)");
        if (Aggregator.UndersByTeam.TryGetValue(team.TeamName, out int underMarks)) Console.WriteLine($"{underMarks} Under Mark(s)");
        Console.WriteLine(GetPointSpreadRecordString(team, Aggregator.PointSpreadRecords));
        Console.WriteLine("\n");
    }
}

static string GetPointSpreadRecordString(ITeam TeamForRecord, IDictionary<string, List<PointSpreadRecord>> PointSpreadRecords)
{
    List<string> records = [];
    if (PointSpreadRecords.TryGetValue(TeamForRecord.TeamName, out List<PointSpreadRecord>? spreadRecord) && spreadRecord != null)
    {
        foreach (PointSpreadRecord record in spreadRecord)
        {
            records.Add($"{record.Side} Side; {record.Wins} Win(s); {record.Losses} Loss(es)");
        }
        return string.Join("\n", [..records]);
    }
    return "\n";
}