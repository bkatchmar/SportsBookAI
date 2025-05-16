namespace SportsBookAI.Core.Interfaces;

public interface IPatternRepo
{
    IList<IPredictionPattern> GetAllPredictions(IList<IMatch> Matches);
}
