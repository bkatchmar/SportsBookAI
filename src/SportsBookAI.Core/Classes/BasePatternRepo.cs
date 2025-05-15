using SportsBookAI.Core.Classes.Patterns;
using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes;

public class BasePatternRepo(IAggregator AggregationLogic)
{
    // Just add each pattern here as I make more class implementations of the IPredictionPattern
    private readonly List<Func<IAggregator, IMatch, IPredictionPattern>> _patternFactories =
    [
        (agg, match) => new BlindlyTakeTheOverIfOneTeamIsTopOver(agg, match),
        (agg, match) => new BlindlyTakeTheUnderIfOneTeamIsTopUnder(agg, match),
        (agg, match) => new MakePickIfTeamInOneExtremeButNotTheOther(agg, match),
        (agg, match) => new PickMajorityOverUnderIfBothTeamsAreMiddleOfPack(agg, match),
        (agg, match) => new IfOneSideOfPointSpreadIsOverAmountBlindlyPick(agg, match, 0.6, 5),
        (agg, match) => new PickPlusMinusIfOneSideRecordGreaterThanOther(agg, match),
        (agg, match) => new PickOverUnderFromPreviousMatchesBetweenTwoTeams(agg, match),
        (agg, match) => new FlipPickOverUnderFromPreviousMatchesBetweenTwoTeams(agg, match)
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