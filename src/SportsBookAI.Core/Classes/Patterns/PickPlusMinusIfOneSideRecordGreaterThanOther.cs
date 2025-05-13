using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class PickPlusMinusIfOneSideRecordGreaterThanOther : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;

    public PickPlusMinusIfOneSideRecordGreaterThanOther(IAggregator AggregationLogic, IMatch MatchData)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        MakePreidction();
    }

    public int ID => 6;
    public string Name => "Pick Plus Minus If One Side Record Greater Than Other";
    public bool PredictionMade => !string.IsNullOrEmpty(PredictionText);
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

            int minusSideWins = _aggregator.GetTeamMinusSideWins(favoredTeam.TeamName);
            int minusSideLosses = _aggregator.GetTeamMinusSideLosses(favoredTeam.TeamName);
            int totalMinusMatches = minusSideWins + minusSideLosses; 
            double minusSideWinPercentage = totalMinusMatches == 0 ? 0 : (double)minusSideWins / totalMinusMatches;

            int plusSideWins = _aggregator.GetTeamPlusSideWins(underdog.TeamName);
            int plusSideLosses = _aggregator.GetTeamPlusSideLosses(underdog.TeamName);
            int plusMinusMatches = minusSideWins + minusSideLosses; 
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