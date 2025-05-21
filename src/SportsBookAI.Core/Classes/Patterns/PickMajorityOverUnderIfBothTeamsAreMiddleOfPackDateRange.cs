using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class PickMajorityOverUnderIfBothTeamsAreMiddleOfPackDateRange : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;
    private int _daysBack;
    private int _id;

    public PickMajorityOverUnderIfBothTeamsAreMiddleOfPackDateRange(IAggregator AggregationLogic, IMatch MatchData, int DaysBack, int ID)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        _daysBack = DaysBack;
        _id = ID;
        MakePreidction();
    }

    public int ID => _id;
    public string Name => $"If Both Teams Are Middle Of Over Under Pack, Pick The Majority From The Past {_daysBack} Days";
    public bool PredictionMade { get; private set; } = false;
    public string PredictionText { get; private set; } = string.Empty;
    public string Match => _matchData.ToString() ?? "Match Data Not Available";

    private void MakePreidction()
    {
        int maxOvers = _aggregator.OversByTeam.Values.Max();
        int maxUnders = _aggregator.UndersByTeam.Values.Max();
        if (_aggregator.UndersByTeam.ContainsKey(_matchData.HomeTeam.TeamName) && _aggregator.OversByTeam.ContainsKey(_matchData.HomeTeam.TeamName))
        {
            int homeTeamOvers =  _aggregator.OversByTeam[_matchData.HomeTeam.TeamName];
            int homeTeamUnders =  _aggregator.UndersByTeam[_matchData.HomeTeam.TeamName];
            int awayTeamOvers =  _aggregator.OversByTeam[_matchData.AwayTeam.TeamName];
            int awayTeamUnders =  _aggregator.UndersByTeam[_matchData.AwayTeam.TeamName];

            if (homeTeamOvers < maxOvers && awayTeamOvers < maxOvers && homeTeamUnders < maxUnders && awayTeamUnders < maxUnders)
            {
                if (_aggregator.AllOverPercentage > _aggregator.AllUnderPercentage)
                {
                    PredictionMade = true;
                    PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} Over";
                }
                else if (_aggregator.AllUnderPercentage > _aggregator.AllOverPercentage)
                {
                    PredictionMade = true;
                    PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} Under";
                }
            }
        }
    }

    public override string ToString() => PredictionText;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (obj is IPredictionPattern pattern)
        {
            return ID == pattern.ID && Name == pattern.Name && PredictionText == pattern.PredictionText;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ID, Name, PredictionText);
    }
}