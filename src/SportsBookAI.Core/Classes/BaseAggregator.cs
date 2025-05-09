using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Classes;

public class BaseAggregator(string LeagueName, ISportsBookRepository Repoository) : IAggregator
{
    private readonly Dictionary<string, int> oversDict = [];
    private readonly Dictionary<string, int> undersDict = [];
    private readonly Dictionary<string, int> plusWinsDict = [];
    private readonly Dictionary<string, int> minusWinsDict = [];
    private readonly Dictionary<string, int> plusLossesDict = [];
    private readonly Dictionary<string, int> minusLossesDict = [];
    private const string OVER = "OVER";
    private const string UNDER = "UNDER";
    private const string PLUS = "PLUS";
    private const string MINUS = "MINUS";

    public string League => LeagueName;
    public ISportsBookRepository Repo => Repoository;
    public IDictionary<string, int> OversByTeam => oversDict;
    public IDictionary<string, int> UndersByTeam => undersDict;
    public IEnumerable<int> TotalUniqueOvers => oversDict.Values.Distinct();
    public IEnumerable<int> TotalUniqueUnders => undersDict.Values.Distinct();
    public double AllOverPercentage => CalculateUnderPercentage(OVER);
    public double AllUnderPercentage => CalculateUnderPercentage(UNDER);

    public void Aggregate()
    {
        IList<IOverUnder> marks = Repo.OverUnderRepository.GetAll();
        IList<IPointSpread> spreads = Repo.PointSpreadRepository.GetAll();
        CompileAllOversAndUnders(marks);
        CompileAllPointSpreads(spreads);
    }
    public async Task AggregateAsync()
    {
        IList<IOverUnder> marks = await Repo.OverUnderRepository.GetAllAsync();
        CompileAllOversAndUnders(marks);
    }

    public bool DoesThisMatchNeedOverUnderPrediction(IMatch MatchData)
    {
        if (oversDict.Count > 0 || undersDict.Count > 0)
        {
            IList<IOverUnder> marks = Repo.OverUnderRepository.GetAll();
            return !marks.Select(m => m.Match).Contains(MatchData);
        }
        return true;
    }

    private void CompileAllOversAndUnders(IList<IOverUnder> Marks)
    {
        if (oversDict.Count == 0) CompileOvers(Marks);
        if (undersDict.Count == 0) CompileUnders(Marks);
    }

    private void CompileOvers(IList<IOverUnder> Results)
    {
        foreach (IOverUnder result in Results)
        {
            _ = oversDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = oversDict.TryAdd(result.Match.AwayTeam.TeamName, 0);

            if (result.Hit.Equals(OVER, StringComparison.OrdinalIgnoreCase))
            {
                oversDict[result.Match.HomeTeam.TeamName] += 1;
                oversDict[result.Match.AwayTeam.TeamName] += 1;
            }
        }
    }

    private void CompileUnders(IList<IOverUnder> Results)
    {
        foreach (IOverUnder result in Results)
        {
            _ = undersDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = undersDict.TryAdd(result.Match.AwayTeam.TeamName, 0);

            if (result.Hit.Equals(UNDER, StringComparison.OrdinalIgnoreCase))
            {
                undersDict[result.Match.HomeTeam.TeamName] += 1;
                undersDict[result.Match.AwayTeam.TeamName] += 1;
            }
        }
    }

    private double CalculateUnderPercentage(string HitMark)
    {
        IList<IOverUnder> allMarks = Repo.OverUnderRepository.GetAll();
        IEnumerable<IOverUnder> marks = allMarks.Where(m => m.Hit.Equals(HitMark, StringComparison.OrdinalIgnoreCase));

        // Yeah, lets be a little more careful to not have a Divide By Zero exception thrown
        if (allMarks.Count == 0)
        {
            return 0;
        }
        else
        {
            return (double)marks.Count() / allMarks.Count;
        }
    }

    private void CompileAllPointSpreads(IList<IPointSpread> PointSpreads)
    {
        foreach (IPointSpread result in PointSpreads)
        {
            _ = minusWinsDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = minusLossesDict.TryAdd(result.Match.AwayTeam.TeamName, 0);
            _ = plusWinsDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = plusLossesDict.TryAdd(result.Match.AwayTeam.TeamName, 0);
        }
    }
}