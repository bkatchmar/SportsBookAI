namespace SportsBookAI.Api.Models;

public class PredictionRequest
{
    public string LeagueName { get; set; } = string.Empty;
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
}
