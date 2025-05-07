using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

internal class MockTeam : ITeam
{
    public int ID { get; set; }
    public string TeamName { get; set; } = "";
    public string? Conference { get; set; }
    public string? Division { get; set; }

    public override string ToString() => TeamName;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (obj is MockTeam otherTeam)
        {
            return ID == otherTeam.ID && TeamName == otherTeam.TeamName;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ID, TeamName);
    }
}