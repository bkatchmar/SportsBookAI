using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Structs;

namespace SportsBookAI.Core.Classes;

public interface IAmericanFootballAggregator : IAggregator
{
    IList<AmericanFootballWeekRecord> AllWeekRecords { get; }
}
