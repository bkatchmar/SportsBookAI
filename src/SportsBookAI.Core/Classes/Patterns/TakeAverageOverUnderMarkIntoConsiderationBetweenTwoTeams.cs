using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes.Patterns;

public class TakeAverageOverUnderMarkIntoConsiderationBetweenTwoTeams : IPredictionPattern
{
    private IAggregator _aggregator;
    private IMatch _matchData;
    private int _customId;
    private int _daysBack;
    private double _overUnderMarkUsing;
    private DateTime _point;
    private IAggregator _homeTeamAggregation;
    private IAggregator _awayTeamAggregation;

    public TakeAverageOverUnderMarkIntoConsiderationBetweenTwoTeams(IAggregator AggregationLogic, IMatch MatchData, int ID, DateTime Point, int DaysBack = -1, double MarkUsing = -1)
    {
        _aggregator = AggregationLogic;
        _matchData = MatchData;
        _customId = ID;
        _daysBack = DaysBack;
        _overUnderMarkUsing = MarkUsing;
        _point = Point;

        _homeTeamAggregation = new TeamSpecificAggregator(AggregationLogic.League, MatchData.HomeTeam, AggregationLogic.Repo);
        _awayTeamAggregation = new TeamSpecificAggregator(AggregationLogic.League, MatchData.AwayTeam, AggregationLogic.Repo);

        if (MarkUsing > 0)
        {
            if (DaysBack > 0)
            {
                _homeTeamAggregation = new TeamSpecificAggregator(AggregationLogic.League, MatchData.HomeTeam, AggregationLogic.Repo, Point, DaysBack);
                _awayTeamAggregation = new TeamSpecificAggregator(AggregationLogic.League, MatchData.AwayTeam, AggregationLogic.Repo, Point, DaysBack);
            }

            MakePreidction();
        }
    }

    public int ID => _customId;
    public string Name => _daysBack > 0 ? $"Take Over Under Average Marks Into Consideration For Both Teams For The Past {_daysBack} Days" : $"Take Over Under Average Marks Into Consideration For Both Teams";
    public bool PredictionMade { get; private set; } = false;
    public string PredictionText { get; private set; } = string.Empty;
    public string Match => _matchData.ToString() ?? "Match Data Not Available";

    private void MakePreidction()
    {
        PredictionMade = true;
        _homeTeamAggregation.Aggregate();
        _awayTeamAggregation.Aggregate();

        // If both teams average over mark is above the current mark, go ahead and blindly take the over
        if (_homeTeamAggregation.AverageOverHit > _overUnderMarkUsing && _awayTeamAggregation.AverageOverHit > _overUnderMarkUsing)
        {
            PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} Over {_overUnderMarkUsing} Points";
        }
        else
        {
            PredictionText = $"{_matchData.AwayTeam.TeamName}/{_matchData.HomeTeam.TeamName} Under {_overUnderMarkUsing} Points";
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