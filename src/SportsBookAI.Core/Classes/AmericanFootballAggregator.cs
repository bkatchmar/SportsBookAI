using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Structs;

namespace SportsBookAI.Core.Classes;

public class AmericanFootballAggregator(IAggregator BaseLevel) : IAmericanFootballAggregator
{
    private Dictionary<int, int> _overs = [];
    private Dictionary<int, int> _unders = [];
    private const string OVER = "OVER";
    private const string UNDER = "UNDER";
    private List<AmericanFootballWeekRecord> _weekRecords = [];

    public string League => BaseLevel.League;
    public ISportsBookRepository Repo => BaseLevel.Repo;
    public IDictionary<string, int> OversByTeam => BaseLevel.OversByTeam;
    public IDictionary<string, int> UndersByTeam => BaseLevel.UndersByTeam;
    public IEnumerable<int> TotalUniqueOvers => BaseLevel.TotalUniqueOvers;
    public IEnumerable<int> TotalUniqueUnders => BaseLevel.TotalUniqueUnders;
    public double AllOverPercentage => BaseLevel.AllOverPercentage;
    public double AllUnderPercentage => BaseLevel.AllUnderPercentage;
    public double AllMinusSpreadsPercentage => BaseLevel.AllMinusSpreadsPercentage;
    public double AllPlusSpreadsPercentage => BaseLevel.AllPlusSpreadsPercentage;
    public int GetTeamMinusSideWins(string TeamName) => BaseLevel.GetTeamMinusSideWins(TeamName);
    public int GetTeamMinusSideLosses(string TeamName) => BaseLevel.GetTeamMinusSideLosses(TeamName);
    public int GetTeamPlusSideWins(string TeamName) => BaseLevel.GetTeamPlusSideWins(TeamName);
    public int GetTeamPlusSideLosses(string TeamName) => BaseLevel.GetTeamPlusSideLosses(TeamName);
    public IEnumerable<int> MinusWinPoints => BaseLevel.MinusWinPoints;
    public IEnumerable<int> MinusPlusPoints => BaseLevel.MinusPlusPoints;
    public IDictionary<string, List<PointSpreadRecord>> PointSpreadRecords => BaseLevel.PointSpreadRecords;
    public void Aggregate()
    {
        BaseLevel.Aggregate();
        CompileOverUnderMarksByWeek();
    }
    public async Task AggregateAsync()
    {
        await BaseLevel.AggregateAsync();
        CompileOverUnderMarksByWeek();
    }
    public bool DoesThisMatchNeedOverUnderPrediction(IMatch MatchData) => BaseLevel.DoesThisMatchNeedOverUnderPrediction(MatchData);
    public bool DoesThisMatchNeedPointSpreadPrediction(IMatch MatchData) => BaseLevel.DoesThisMatchNeedPointSpreadPrediction(MatchData);

    public IList<AmericanFootballWeekRecord> AllWeekRecords => _weekRecords;

    private void CompileOverUnderMarksByWeek()
    {
        // Assumption, this was called beforehand and the repo has these matches already stored, but in case it isn't for some odd reason, this will grab from the DB
        foreach (IMatch match in Repo.MatchRepository.GetAll())
        {
            if (match is IAmericanFootballMatch footballMatch && footballMatch.WeekNumber.HasValue)
            {
                _ = _overs.TryAdd(footballMatch.WeekNumber.Value, 0);
                _ = _unders.TryAdd(footballMatch.WeekNumber.Value, 0);

                IOverUnder? lookup = Repo.OverUnderRepository.GetAll().FirstOrDefault(ou => ou.Match.Equals(match));
                if (lookup != null)
                {
                    if (lookup.Hit.Equals(OVER, StringComparison.OrdinalIgnoreCase))
                    {
                        _overs[footballMatch.WeekNumber.Value] += 1;
                    }
                    else if (lookup.Hit.Equals(UNDER, StringComparison.OrdinalIgnoreCase))
                    {
                        _unders[footballMatch.WeekNumber.Value] += 1;
                    }
                }
            }
        }

        foreach (int week in _overs.Keys)
        {
            _weekRecords.Add(new(week, _overs[week], _unders[week]));
        }
    }
}
