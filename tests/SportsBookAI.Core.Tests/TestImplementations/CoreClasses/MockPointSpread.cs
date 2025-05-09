using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

public class MockPointSpread : IPointSpread
{
    public IMatch Match { get; set; } = null!;
    public double Spread { get; set; }
    public string Result { get; set; } = string.Empty;
    public ITeam FavoredTeam { get; set; } = null!;
}