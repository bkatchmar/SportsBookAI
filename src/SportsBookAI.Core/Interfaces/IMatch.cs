namespace SportsBookAI.Core.Interfaces;

public interface IMatch
{
    DateTime MatchDateTime { get; set; }
    ITeam HomeTeam { get; set; }
    ITeam AwayTeam { get; set; }
}