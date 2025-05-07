using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

public class MockOverUnder : IOverUnder
{
    public IMatch Match { get; set; } = null!;
    public double Mark { get; set; }
    public string Hit { get; set; } = string.Empty;
}