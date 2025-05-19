using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class FlipPickOverUnderFromPreviousMatchesBetweenTwoTeams : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;
    private const string OVER = "OVER";
    private const string UNDER = "UNDER";

    public FlipPickOverUnderFromPreviousMatchesBetweenTwoTeams(IAggregator AggregationLogic, IMatch MatchData)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        MakePreidction();
    }

    public int ID => 8;
    public string Name => "Flipped Over or Unders From Previous Matches Between Two Teams";
    public bool PredictionMade { get; private set; } = false;
    public string PredictionText { get; private set; } = string.Empty;
    public string Match => _matchData.ToString() ?? "Match Data Not Available";

    private void MakePreidction()
    {
        // Get all previous matches between these teams
        IList<IMatch> allPreviousMatchesBetweenTwoTeams = [.. _aggregator.Repo.MatchRepository.GetAll().Where(m =>
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
                PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} Under";
            }
            else
            {
                PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} Over";   
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