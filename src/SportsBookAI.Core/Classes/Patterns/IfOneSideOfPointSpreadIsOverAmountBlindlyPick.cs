using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class IfOneSideOfPointSpreadIsOverAmountBlindlyPick : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;
    private double _threshold;

    public IfOneSideOfPointSpreadIsOverAmountBlindlyPick(IAggregator AggregationLogic, IMatch MatchData, double Threshold = 0.6)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        _threshold = Threshold;
        MakePreidction();

        string? m = MatchData.ToString();
    }

    public int ID => 5;
    public string Name => "If Plus Minus Side Exceed Threshold, Blindly Pick That Side";
    public bool PredictionMade { get; private set; } = false;
    public string PredictionText { get; private set; } = string.Empty;
    public string Match => _matchData.ToString() ?? "Match Data Not Available";

    private void MakePreidction()
    {
        if (_aggregator.AllPlusSpreadsPercentage >= _threshold)
        {
            PredictionMade = true;
            PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} +";
        }
        else if (_aggregator.AllMinusSpreadsPercentage >= _threshold)
        {
            PredictionMade = true;
            PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} -";
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