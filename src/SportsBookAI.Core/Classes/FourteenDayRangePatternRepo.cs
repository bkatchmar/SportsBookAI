using SportsBookAI.Core.Classes.Patterns;
using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes;

public class FourteenDayRangePatternRepo(IAggregator AggregationLogic, DateTime Point, double MarkUsing = -1) : IPatternRepo
{
    // Just add each pattern here as I make more class implementations of the IPredictionPattern
    private readonly List<Func<IAggregator, IMatch, IPredictionPattern>> _patternFactories =
    [
        (agg, match) => new BlindlyTakeTheOverIfOneTeamIsTopOverDateRange(agg, match, 14, 17),
        (agg, match) => new BlindlyTakeTheUnderIfOneTeamIsTopUnderOverDateRange(agg, match, 14, 18),
        (agg, match) => new MakePickIfTeamInOneExtremeButNotTheOtherOverDateRange(agg, match, 14, 19),
        (agg, match) => new PickMajorityOverUnderIfBothTeamsAreMiddleOfPackDateRange(agg, match, 14, 20),
        (agg, match) => new IfOneSideOfPointSpreadIsOverAmountBlindlyPickDateRange(agg, match, 0.6, 21, 14),
        (agg, match) => new PickPlusMinusIfOneSideRecordGreaterThanOtherDateRange(agg, match, 14, 14, Point),
        (agg, match) => new PickOverUnderFromPreviousMatchesBetweenTwoTeamsDateRange(agg, match, 14, 23, Point),
        (agg, match) => new FlipPickOverUnderFromPreviousMatchesBetweenTwoTeamsDateRange(agg, match, 14, 24, Point),
        (agg, match) => new TakeAverageOverUnderMarkIntoConsiderationBetweenTwoTeams(agg, match, 27, Point, 14, MarkUsing: MarkUsing)
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