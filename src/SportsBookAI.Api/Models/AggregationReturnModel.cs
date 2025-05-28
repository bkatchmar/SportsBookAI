using SportsBookAI.Core.Classes;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Structs;

namespace SportsBookAI.Api.Models;

public class AggregationReturnModel(IAggregator Aggregator)
{
    public IDictionary<string, int> OversByTeam => Aggregator.OversByTeam;
    public IDictionary<string, int> UndersByTeam => Aggregator.UndersByTeam;
    public double AllOverPercentage => Aggregator.AllOverPercentage;
    public double AllUnderPercentage => Aggregator.AllUnderPercentage;
    public double AllMinusSpreadsPercentage => Aggregator.AllMinusSpreadsPercentage;
    public double AllPlusSpreadsPercentage => Aggregator.AllPlusSpreadsPercentage;
    public double HighestOverHit => Aggregator.HighestOverHit;
    public double LowestOverHit => Aggregator.LowestOverHit;
    public double AverageOverHit => Aggregator.AverageOverHit;
    public double HighestUnderHit => Aggregator.HighestUnderHit;
    public double LowestUnderHit => Aggregator.LowestUnderHit;
    public double AverageUnderHit => Aggregator.AverageUnderHit;
    public IEnumerable<int> MinusWinPoints => Aggregator.MinusWinPoints;
    public IEnumerable<int> MinusPlusPoints => Aggregator.MinusPlusPoints;
    public IDictionary<string, List<PointSpreadRecord>> PointSpreadRecords => Aggregator.PointSpreadRecords;
    public IList<AmericanFootballWeekRecord>? AllWeekRecords => (Aggregator is IAmericanFootballAggregator football) ? football.AllWeekRecords : null;
}
