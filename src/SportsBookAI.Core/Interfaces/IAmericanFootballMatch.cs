namespace SportsBookAI.Core.Interfaces;

public interface IAmericanFootballMatch : IMatch
{
    int WeekNumber { get; set; }
}