using SportsBookAI.Core.Classes.Patterns;
using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes;

public class SevenDayRangePatternRepo(IAggregator AggregationLogic, DateTime Point) : IPatternRepo
{
    // Just add each pattern here as I make more class implementations of the IPredictionPattern
    private readonly List<Func<IAggregator, IMatch, IPredictionPattern>> _patternFactories =
    [
        (agg, match) => new BlindlyTakeTheOverIfOneTeamIsTopOverDateRange(agg, match, 7, 9),
        (agg, match) => new BlindlyTakeTheUnderIfOneTeamIsTopUnderOverDateRange(agg, match, 7, 10),
        (agg, match) => new MakePickIfTeamInOneExtremeButNotTheOtherOverDateRange(agg, match, 7, 11),
        (agg, match) => new PickMajorityOverUnderIfBothTeamsAreMiddleOfPackDateRange(agg, match, 7, 12),
        (agg, match) => new IfOneSideOfPointSpreadIsOverAmountBlindlyPickDateRange(agg, match, 0.6, 13, 7),
        (agg, match) => new PickPlusMinusIfOneSideRecordGreaterThanOtherDateRange(agg, match, 7, 14, Point),
        (agg, match) => new PickOverUnderFromPreviousMatchesBetweenTwoTeamsDateRange(agg, match, 7, 15, Point),
        (agg, match) => new FlipPickOverUnderFromPreviousMatchesBetweenTwoTeamsDateRange(agg, match, 7, 16, Point)
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