namespace SportsBookAI.Core.Interfaces;

public interface IPointSpread
{
    IMatch Match { get; set; }
    double Spread { get; set; }
    string Result { get; set; }
    ITeam FavoredTeam { get; set; }
}