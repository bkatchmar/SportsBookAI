namespace SportsBookAI.Core.Interfaces;

public interface IMatch
{
    DateTime MatchDateTimeUTC { get; set; }
    DateTime MatchDateTimeLocal { get; set; }
    ITeam HomeTeam { get; set; }
    ITeam AwayTeam { get; set; }
}