using SportsBookAI.Core.Classes.Patterns;
using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes;

public class SevenDayRangePatternRepo(IAggregator AggregationLogic, DateTime Point) : IPatternRepo
{
    // Just add each pattern here as I make more class implementations of the IPredictionPattern
    private readonly List<Func<IAggregator, IMatch, IPredictionPattern>> _patternFactories =
    [
        (agg, match) => new BlindlyTakeTheOverIfOneTeamIsTopOverDateRange(agg, match, 7, 9),
        (agg, match) => new BlindlyTakeTheUnderIfOneTeamIsTopUnderOverDateRange(agg, match, 7, 10)
    ];

    public IList<IPredictionPattern> GetAllPredictions(IList<IMatch> Matches)
    {
        IList<IPredictionPattern> rtnVal = [];

        foreach (IMatch match in Matches)
        {
            foreach (var factory in _patternFactories)
            {
                IPredictionPattern pattern = factory(AggregationLogic, match);
                if (pattern.PredictionMade)
                {
                    rtnVal.Add(pattern);
                }
            }
        }

        return rtnVal;
    }
}