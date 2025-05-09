namespace SportsBookAI.Core.Structs;

public readonly struct PointSpreadRecord(string SideOfSpread, int NumberOfWins, int NumberOfLosses)
{
    public string Side => SideOfSpread;
    public int Wins => NumberOfWins;
    public int Losses => NumberOfLosses;
}