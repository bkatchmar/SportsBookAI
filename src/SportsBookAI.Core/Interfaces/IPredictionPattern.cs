namespace SportsBookAI.Core.Interfaces;

public interface IPredictionPattern
{
    int ID { get; }
    string Name { get; }
    bool PredictionMade { get; }
    string PredictionText { get; }
    string Match { get; }
}