using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class BlindlyTakeTheUnderIfOneTeamIsTopUnder : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;

    public BlindlyTakeTheUnderIfOneTeamIsTopUnder(IAggregator AggregationLogic, IMatch MatchData)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        MakePreidction();
    }

    public int ID => 2;
    public string Name => "Blindly Take The Under If One Team Is Top Under";
    public bool PredictionMade { get; private set; } = false;
    public string PredictionText { get; private set; } = string.Empty;
    public string Match => _matchData.ToString() ?? "Match Data Not Available";

    private void MakePreidction()
    {
        int maxOvers = _aggregator.UndersByTeam.Values.Max();
        if (_aggregator.UndersByTeam.ContainsKey(_matchData.HomeTeam.TeamName))
        {
            if (_aggregator.UndersByTeam[_matchData.HomeTeam.TeamName] == maxOvers)
            {
                PredictionMade = true;
                PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} Under";
            }

            if (_aggregator.UndersByTeam[_matchData.AwayTeam.TeamName] == maxOvers)
            {
                PredictionMade = true;
                PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} Under";
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