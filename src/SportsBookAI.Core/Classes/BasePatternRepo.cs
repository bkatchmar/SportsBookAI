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
        (agg, match) => new PickMajorityOverUnderIfBothTeamsAreMiddleOfPack(agg, match)
    ];

    public IList<IPredictionPattern> GetAllPredictions(IList<IMatch> matches)
    {
        IList<IPredictionPattern> rtnVal = [];

        foreach (IMatch match in matches)
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