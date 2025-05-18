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
    public IEnumerable<int> MinusWinPoints => Aggregator.MinusWinPoints;
    public IEnumerable<int> MinusPlusPoints => Aggregator.MinusPlusPoints;
    public IDictionary<string, List<PointSpreadRecord>> PointSpreadRecords => Aggregator.PointSpreadRecords;
}
