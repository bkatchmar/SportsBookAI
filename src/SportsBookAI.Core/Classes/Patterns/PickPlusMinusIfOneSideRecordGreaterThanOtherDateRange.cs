using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class PickPlusMinusIfOneSideRecordGreaterThanOtherDateRange : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;
    private int _id;
    private int _daysBack;
    private DateTime _point;

    public PickPlusMinusIfOneSideRecordGreaterThanOtherDateRange(IAggregator AggregationLogic, IMatch MatchData, int ID, int DaysBack, DateTime Point)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        _id = ID;
        _daysBack = DaysBack;
        _point = Point;
        MakePreidction();
    }

    public int ID => _id;
    public string Name => $"Pick Plus Minus If One Side Record Greater Than Other From The Past {_daysBack} Days";
    public bool PredictionMade => !string.IsNullOrEmpty(PredictionText);
    public string PredictionText { get; private set; } = string.Empty;
    public string Match => _matchData.ToString() ?? "Match Data Not Available";

    private void MakePreidction()
    {
        IList<IPointSpread> allPointSpreads = _aggregator.Repo.PointSpreadRepository.GetFromDaysBack(_point, _daysBack);
        IPointSpread? lookup = allPointSpreads.FirstOrDefault(p => p.Match.Equals(_matchData));
        
        if (lookup != null)
        {
            ITeam favoredTeam = lookup.FavoredTeam;
            ITeam underdog = lookup.FavoredTeam.Equals(_matchData.HomeTeam) ? _matchData.AwayTeam : _matchData.HomeTeam;

            int minusSideWins = _aggregator.GetTeamMinusSideWins(favoredTeam.TeamName);
            int minusSideLosses = _aggregator.GetTeamMinusSideLosses(favoredTeam.TeamName);
            int totalMinusMatches = minusSideWins + minusSideLosses; 
            double minusSideWinPercentage = totalMinusMatches == 0 ? 0 : (double)minusSideWins / totalMinusMatches;

            int plusSideWins = _aggregator.GetTeamPlusSideWins(underdog.TeamName);
            int plusSideLosses = _aggregator.GetTeamPlusSideLosses(underdog.TeamName);
            int plusMinusMatches = plusSideWins + plusSideLosses; 
            double plusSideWinPercentage = plusMinusMatches == 0 ? 0 : (double)plusSideWins / plusMinusMatches;

            if (minusSideWinPercentage > plusSideWinPercentage)
            {
                PredictionText = $"{favoredTeam.TeamName} Over {underdog.TeamName} -";
            }
            else 
            {
                PredictionText = $"{underdog.TeamName} Over {favoredTeam.TeamName} +";
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