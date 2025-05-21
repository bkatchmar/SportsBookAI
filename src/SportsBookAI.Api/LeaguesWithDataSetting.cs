namespace SportsBookAI.Api;

public class LeaguesWithDataSetting
{
    public string[] Leagues { get; set; } = [];
    public Dictionary<string, DateTime> OpeningDays { get; set; } = [];
}
