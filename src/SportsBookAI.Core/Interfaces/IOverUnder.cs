namespace SportsBookAI.Core.Interfaces;

public interface IOverUnder
{
    IMatch Match { get; set; }
    double Mark { get; set; }
    string Hit { get; set; }
}