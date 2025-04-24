namespace SportsBookAI.Core.Interfaces;

public interface ITeam
{
    string TeamName { get; set; }
    string? Conference { get; set; }
    string? Division { get; set; }
}