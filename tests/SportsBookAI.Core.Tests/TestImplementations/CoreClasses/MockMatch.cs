using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

public class MockMatch : IMatch
{
    public int ID { get; set; }
    public DateTime MatchDateTimeUTC { get; set; }
    public DateTime MatchDateTimeLocal { get; set; }
    public required ITeam HomeTeam { get; set; }
    public required ITeam AwayTeam { get; set; }

    public override string ToString()
    {
        return $"Match Date: {MatchDateTimeLocal:dddd, MMMM dd, yyyy HH:mm:ss tt}, {HomeTeam?.ToString()} vs {AwayTeam?.ToString()}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (obj is MockMatch otherMatch)
        {
            return ID == otherMatch.ID && HomeTeam.TeamName == otherMatch.HomeTeam.TeamName && AwayTeam.TeamName == otherMatch.AwayTeam.TeamName;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ID, MatchDateTimeUTC, HomeTeam.TeamName, AwayTeam.TeamName);
    }
}