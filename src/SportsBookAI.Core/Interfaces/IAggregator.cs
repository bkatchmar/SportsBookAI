using SportsBookAI.Core.Structs;

namespace SportsBookAI.Core.Interfaces;

public interface IAggregator
{
    string League { get; }
    ISportsBookRepository Repo { get; }
    IDictionary<string, int> OversByTeam { get; }
    IDictionary<string, int> UndersByTeam { get; }
    IEnumerable<int> TotalUniqueOvers { get; }
    IEnumerable<int> TotalUniqueUnders { get; }
    double AllOverPercentage { get; }
    double AllUnderPercentage { get; }
    double AllMinusSpreadsPercentage { get; }
    double AllPlusSpreadsPercentage { get; }
    int GetTeamMinusSideWins(string TeamName);
    int GetTeamMinusSideLosses(string TeamName);
    int GetTeamPlusSideWins(string TeamName);
    int GetTeamPlusSideLosses(string TeamName);
    IEnumerable<int> MinusWinPoints { get; }
    IEnumerable<int> MinusPlusPoints { get; }
    IDictionary<string, List<PointSpreadRecord>> PointSpreadRecords { get; }
    void Aggregate();
    Task AggregateAsync();
    bool DoesThisMatchNeedOverUnderPrediction(IMatch MatchData);
    bool DoesThisMatchNeedPointSpreadPrediction(IMatch MatchData);
}