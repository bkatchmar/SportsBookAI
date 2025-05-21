using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class IfOneSideOfPointSpreadIsOverAmountBlindlyPickDateRange : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;
    private double _threshold;
    private int _customId;
    private int _daysBack;

    public IfOneSideOfPointSpreadIsOverAmountBlindlyPickDateRange(IAggregator AggregationLogic, IMatch MatchData, double Threshold, int ID, int DaysBack)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        _threshold = Threshold;
        _customId = ID;
        _daysBack = DaysBack;
        MakePreidction();
    }

    public int ID => _customId;
    public string Name => $"If Plus Minus Side Exceed {_threshold.ToString("P0")} Threshold, Blindly Pick That Side From The Past {_daysBack} Days";
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