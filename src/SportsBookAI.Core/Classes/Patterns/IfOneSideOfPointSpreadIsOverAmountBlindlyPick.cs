using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class IfOneSideOfPointSpreadIsOverAmountBlindlyPick : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;
    private double _threshold;
    private int _customId;

    public IfOneSideOfPointSpreadIsOverAmountBlindlyPick(IAggregator AggregationLogic, IMatch MatchData, double Threshold, int ID)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        _threshold = Threshold;
        _customId = ID;
        MakePreidction();
    }

    public int ID => _customId;
    public string Name => $"If Plus Minus Side Exceed {_threshold.ToString("P0")} Threshold, Blindly Pick That Side";
    public bool PredictionMade { get; private set; } = false;
    public string PredictionText { get; private set; } = string.Empty;
    public string Match => _matchData.ToString() ?? "Match Data Not Available";

    private void MakePreidction()
    {
        IList<IPointSpread> allPointSpreads = _aggregator.Repo.PointSpreadRepository.GetAll();
        IPointSpread? lookup = allPointSpreads.FirstOrDefault(p => p.Match.Equals(_matchData));

        if (lookup != null)
        {
            ITeam favoredTeam = lookup.FavoredTeam;
            ITeam underdog = lookup.FavoredTeam.Equals(_matchData.HomeTeam) ? _matchData.AwayTeam : _matchData.HomeTeam;

            if (_aggregator.AllPlusSpreadsPercentage >= _threshold)
            {
                PredictionMade = true;
                if (_matchData.AwayTeam.Equals(underdog))
                {
                    PredictionText = $"{_matchData.AwayTeam.TeamName} Over {_matchData.HomeTeam.TeamName} +";
                }
                else
                {
                    PredictionText = $"{_matchData.HomeTeam.TeamName} Over {_matchData.AwayTeam.TeamName} +";
                }
            }
            else if (_aggregator.AllMinusSpreadsPercentage >= _threshold)
            {
                PredictionMade = true;
                if (_matchData.AwayTeam.Equals(favoredTeam))
                {
                    PredictionText = $"{_matchData.AwayTeam.TeamName} Over {_matchData.HomeTeam.TeamName} -";
                }
                else
                {
                    PredictionText = $"{_matchData.HomeTeam.TeamName} Over {_matchData.AwayTeam.TeamName} -";
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