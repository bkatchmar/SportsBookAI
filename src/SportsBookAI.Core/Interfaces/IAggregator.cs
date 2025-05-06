namespace SportsBookAI.Core.Interfaces;

public interface IAggregator
{
    string League { get; }
    ISportsBookRepository Repo { get; }
    IDictionary<string, int> OversByTeam { get; }
    IDictionary<string, int> UndersByTeam { get; }
    IEnumerable<int> TotalUniqueOvers { get; }
    IEnumerable<int> TotalUniqueUnders { get; }
    void Aggregate();
    Task AggregateAsync();
    bool DoesThisMatchNeedOverUnderPrediction(IMatch MatchData);
}