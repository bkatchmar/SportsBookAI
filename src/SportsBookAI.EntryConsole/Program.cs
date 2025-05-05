using Microsoft.Extensions.Configuration;
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

Console.WriteLine("Getting high level settings");

// Use the currently selection connection and do the necessary setup
ISportsBookRepository? primarySportsBookRepo = null;

switch (mainSetting?.CurrentConnection)
{
    case "MongoDB":
        Connection? mongoConnection = mainSetting?.Connections.FirstOrDefault(conn => conn.Key == mainSetting?.CurrentConnection);
        if (mongoConnection != null)
        {
            ConnectionDetails.ConnectionString = mongoConnection.ConnectionString;
            primarySportsBookRepo = new MongoSportsBookRepository("UFL");
            Console.WriteLine("Mongo Connection Set");
        }
        else
        {
            Console.WriteLine("Mongo Connection Not Set - Check Your Appsettings File");
        }
        break;
    default:
        break;
}

// Connection set, ready to roll out!
if (primarySportsBookRepo != null)
{
    IList<ITeam> allTeams = primarySportsBookRepo.TeamRepository.GetAll();
    Console.WriteLine($"UFL Has {allTeams.Count} Teams");
    foreach (ITeam team in allTeams)
    {
        Console.WriteLine(team);
    }
}
else
{
    Console.WriteLine("Error when setting the main sportsbook repo");
}