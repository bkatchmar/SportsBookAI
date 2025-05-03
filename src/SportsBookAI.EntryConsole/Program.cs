using Microsoft.Extensions.Configuration;
using SportsBookAI.EntryConsole.SettingsModels;

// Program Start
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Read MongoDB settings from configuration
AppSetting? mainSetting = configuration.GetSection("MainSetting").Get<AppSetting>();

Console.WriteLine("Getting high level settings");
Console.WriteLine($"Targetted Database: {mainSetting?.CurrentConnection}");
Console.Write($"Total Connections: {mainSetting?.Connections.Count}");