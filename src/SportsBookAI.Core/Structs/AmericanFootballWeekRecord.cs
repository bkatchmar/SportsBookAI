namespace SportsBookAI.Core.Structs;

public readonly struct AmericanFootballWeekRecord(int WeekNumber, int NumberOfOvers, int NumberOfUnders)
{
    public int Week => WeekNumber;
    public int Overs => NumberOfOvers;
    public int Unders => NumberOfUnders;
}
