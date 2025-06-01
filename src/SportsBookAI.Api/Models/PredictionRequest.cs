namespace SportsBookAI.Api.Models;

public class PredictionRequest
{
    public dynamic MatchId { get; set; } = null!;
    public string LeagueName { get; set; } = string.Empty;
    public double? OverUnderMark { get; set; }
}
