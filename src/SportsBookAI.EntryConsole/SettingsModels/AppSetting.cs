namespace SportsBookAI.EntryConsole.SettingsModels;

public class AppSetting
{
    public string CurrentConnection { get; set; } = string.Empty;
    public List<Connection> Connections { get; set; } = [];
}