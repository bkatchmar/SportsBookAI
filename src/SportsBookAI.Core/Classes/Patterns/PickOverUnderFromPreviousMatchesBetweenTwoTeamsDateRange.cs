using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class PickOverUnderFromPreviousMatchesBetweenTwoTeamsDateRange : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;
    private int _id;
    private int _daysBack;
    private DateTime _point;
    private const string OVER = "OVER";
    private const string UNDER = "UNDER";

    public PickOverUnderFromPreviousMatchesBetweenTwoTeamsDateRange(IAggregator AggregationLogic, IMatch MatchData, int DaysBack, int ID, DateTime Point)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        _id = ID;
        _daysBack = DaysBack;
        _point = Point;
        MakePreidction();
    }

    public int ID => _id;
    public string Name => $"More Over or Unders From Previous Matches Between Two Teams From The Past {_daysBack} Days";
    public bool PredictionMade { get; private set; } = false;
    public string PredictionText { get; private set; } = string.Empty;
    public string Match => _matchData.ToString() ?? "Match Data Not Available";

    private void MakePreidction()
    {
        // Get all previous matches between these teams
        IList<IMatch> allPreviousMatchesBetweenTwoTeams = [.. _aggregator.Repo.MatchRepository.GetFromDaysBack(_point, _daysBack).Where(m =>
            ((
                (m.HomeTeam.Equals(_matchData.HomeTeam) && m.AwayTeam.Equals(_matchData.AwayTeam))
                || (m.HomeTeam.Equals(_matchData.AwayTeam) && m.AwayTeam.Equals(_matchData.HomeTeam))
            )
            && !m.Equals(_matchData)))];

        // Init Variables
        int totalOvers = 0, totalUnders = 0;

        // Compile
        foreach (IMatch match in allPreviousMatchesBetweenTwoTeams)
        {
            IOverUnder? lookup = _aggregator.Repo.OverUnderRepository.GetAll().FirstOrDefault(m => m.Match.Equals(match));
            if (lookup != null)
            {
                if (lookup.Hit.Equals(OVER, StringComparison.OrdinalIgnoreCase))
                {
                    totalOvers += 1;
                }
                if (lookup.Hit.Equals(UNDER, StringComparison.OrdinalIgnoreCase))
                {
                    totalUnders += 1;
                }
            }
        }

        // Take this data into consideration, make a prediction
        if (totalOvers != totalUnders)
        {
            PredictionMade = true;

            if (totalOvers > totalUnders)
            {
                PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} Over";
            }
            else
            {
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