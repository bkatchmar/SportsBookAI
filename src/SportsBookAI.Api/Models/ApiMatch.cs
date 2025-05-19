using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo.Base;

namespace SportsBookAI.Api.Models;

public class ApiMatch : IMatch
{
    public string Id { get; set; } = null!;

    public ITeam HomeTeam { get; set; } = null!;

    public ITeam AwayTeam { get; set; } = null!;

    public DateTime MatchDateTimeUTC { get; set; } = DateTime.Today.ToUniversalTime();

    public DateTime MatchDateTimeLocal { get; set; } = DateTime.Today;

    public int? WeekNumber { get; set; }

    public string Display => ToString();

    public override string ToString()
    {
        if (WeekNumber > 0)
        {
            return $"Match Date: {MatchDateTimeLocal:dddd, MMMM dd, yyyy HH:mm:ss tt}, Week {WeekNumber}, {HomeTeam?.ToString()} vs {AwayTeam?.ToString()}";
        }

        return $"Match Date: {MatchDateTimeLocal:dddd, MMMM dd, yyyy HH:mm:ss tt}, {HomeTeam?.ToString()} vs {AwayTeam?.ToString()}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (obj is ApiMatch otherMatch)
        {
            return MatchDateTimeLocal == otherMatch.MatchDateTimeLocal && MatchDateTimeUTC == otherMatch.MatchDateTimeUTC;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(MatchDateTimeLocal, MatchDateTimeUTC);
    }
}
