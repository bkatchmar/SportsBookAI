using Microsoft.Extensions.Configuration;
using SportsBookAI.Core.Mongo;
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
switch (mainSetting?.CurrentConnection)
{
    case "MongoDB":
        Connection? mongoConnection = mainSetting?.Connections.FirstOrDefault(conn => conn.Key == mainSetting?.CurrentConnection);
        if (mongoConnection != null)
        {
            ConnectionDetails.ConnectionString = mongoConnection.ConnectionString;
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