using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Structs;

namespace SportsBookAI.Core.Classes;

public class TeamSpecificAggregator(string LeagueName, ITeam TargetTeam, ISportsBookRepository Repoository, DateTime? Point = null, int DaysBack = -1) : IAggregator
{
    private readonly Dictionary<string, int> _oversDict = [];
    private readonly Dictionary<string, int> _undersDict = [];
    private readonly Dictionary<string, List<PointSpreadRecord>> _pointSpreadRecords = [];
    private readonly List<double> _allOverMarks = [];
    private readonly List<double> _allUnderarks = [];
    private const string OVER = "OVER";
    private const string UNDER = "UNDER";
    private const string PLUS = "PLUS";
    private const string MINUS = "MINUS";
    private int _numberOfPointSpreadWhereFavoredMatches = 0;
    private int _numberOfPointSpreadWhereUnderdogMatches = 0;
    private int _numberOfPointSpreadMinusWins = 0;
    private int _numberOfPointSpreadPlusWins = 0;

    // If these are not null, we are not getting all match data
    private DateTime _datePoint = Point ?? DateTime.Today;
    private int _daysBack = DaysBack;

    public string League => LeagueName;
    public ISportsBookRepository Repo => Repoository;
    public IDictionary<string, int> OversByTeam => _oversDict;
    public IDictionary<string, int> UndersByTeam => _undersDict;
    public IEnumerable<int> TotalUniqueOvers => _oversDict.Values.Distinct();
    public IEnumerable<int> TotalUniqueUnders => _undersDict.Values.Distinct();
    public double AllOverPercentage => CalculateUnderPercentage(OVER);
    public double AllUnderPercentage => CalculateUnderPercentage(UNDER);
    public double AllMinusSpreadsPercentage => (_numberOfPointSpreadWhereFavoredMatches == 0) ? 0 : (double)_numberOfPointSpreadMinusWins / _numberOfPointSpreadWhereFavoredMatches;
    public double AllPlusSpreadsPercentage => (_numberOfPointSpreadWhereUnderdogMatches == 0) ? 0 : (double)_numberOfPointSpreadPlusWins / _numberOfPointSpreadWhereUnderdogMatches;
    public double HighestOverHit => _allOverMarks.Any() ? _allOverMarks.Max() : 0;
    public double LowestOverHit => _allOverMarks.Any() ? _allOverMarks.Min() : 0;
    public double AverageOverHit => _allOverMarks.Any() ? _allOverMarks.Average() : 0;
    public double HighestUnderHit => _allUnderarks.Any() ? _allUnderarks.Max() : 0;
    public double LowestUnderHit => _allUnderarks.Any() ? _allUnderarks.Min() : 0;
    public double AverageUnderHit => _allUnderarks.Any() ? _allUnderarks.Average() : 0;
    public int GetTeamMinusSideWins(string TeamName) => GetWins(TeamName, MINUS);
    public int GetTeamMinusSideLosses(string TeamName) => GetLosses(TeamName, MINUS);
    public int GetTeamPlusSideWins(string TeamName) => GetWins(TeamName, PLUS);
    public int GetTeamPlusSideLosses(string TeamName) => GetLosses(TeamName, PLUS);
    public IEnumerable<int> MinusWinPoints => GetWinPoints(MINUS).Distinct();
    public IEnumerable<int> MinusPlusPoints => GetWinPoints(PLUS).Distinct();
    public IDictionary<string, List<PointSpreadRecord>> PointSpreadRecords => _pointSpreadRecords;

    public void Aggregate()
    {
        IList<IOverUnder> marks = GetAllOverUnders();
        IList<IPointSpread> spreads = GetAllPointSpreads();
        CompileAllOversAndUnders(marks);
        CompileAllPointSpreads(spreads);
    }
    public async Task AggregateAsync()
    {
        IList<IOverUnder> marks = await GetAllOverUndersAsync();
        IList<IPointSpread> spreads = await GetAllPointSpreadsASync();
        CompileAllOversAndUnders(marks);
        CompileAllPointSpreads(spreads);
    }

    public bool DoesThisMatchNeedOverUnderPrediction(IMatch MatchData)
    {
        if (_oversDict.Count > 0 || _undersDict.Count > 0)
        {
            IList<IOverUnder> marks = GetAllOverUnders();
            return !marks.Select(m => m.Match).Contains(MatchData);
        }
        return true;
    }

    public bool DoesThisMatchNeedPointSpreadPrediction(IMatch MatchData)
    {
        IList<IPointSpread> allSpreads = GetAllPointSpreads();
        if (allSpreads.Select(m => m.Match).Contains(MatchData))
        {
            IPointSpread lookup = allSpreads.First(s => s.Match.Equals(MatchData));
            if (string.IsNullOrEmpty(lookup.Result))
            {
                return true;
            }
        }

        return false;
    }

    private void CompileAllOversAndUnders(IList<IOverUnder> Marks)
    {
        if (_oversDict.Count == 0) CompileOvers(Marks);
        if (_undersDict.Count == 0) CompileUnders(Marks);
    }

    private void CompileOvers(IList<IOverUnder> Results)
    {
        foreach (IOverUnder result in Results)
        {
            _ = _oversDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = _oversDict.TryAdd(result.Match.AwayTeam.TeamName, 0);

            if (result.Hit.Equals(OVER, StringComparison.OrdinalIgnoreCase))
            {
                _oversDict[result.Match.HomeTeam.TeamName] += 1;
                _oversDict[result.Match.AwayTeam.TeamName] += 1;
                _allOverMarks.Add(result.Mark);
            }
        }

        if (_oversDict.ContainsKey(TargetTeam.TeamName)) _oversDict.Remove(TargetTeam.TeamName);
    }

    private void CompileUnders(IList<IOverUnder> Results)
    {
        foreach (IOverUnder result in Results)
        {
            _ = _undersDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = _undersDict.TryAdd(result.Match.AwayTeam.TeamName, 0);

            if (result.Hit.Equals(UNDER, StringComparison.OrdinalIgnoreCase))
            {
                _undersDict[result.Match.HomeTeam.TeamName] += 1;
                _undersDict[result.Match.AwayTeam.TeamName] += 1;
                _allUnderarks.Add(result.Mark);
            }
        }

        if (_undersDict.ContainsKey(TargetTeam.TeamName)) _undersDict.Remove(TargetTeam.TeamName);
    }

    private double CalculateUnderPercentage(string HitMark)
    {
        IList<IOverUnder> allMarks = GetAllOverUnders();
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
        Dictionary<string, int> plusWinsDict = [];
        Dictionary<string, int> minusWinsDict = [];
        Dictionary<string, int> plusLossesDict = [];
        Dictionary<string, int> minusLossesDict = [];

        // Collect data
        foreach (IPointSpread result in PointSpreads)
        {
            _ = minusWinsDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = minusWinsDict.TryAdd(result.Match.AwayTeam.TeamName, 0);
            _ = minusLossesDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = minusLossesDict.TryAdd(result.Match.AwayTeam.TeamName, 0);
            _ = plusWinsDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = plusWinsDict.TryAdd(result.Match.AwayTeam.TeamName, 0);
            _ = plusLossesDict.TryAdd(result.Match.HomeTeam.TeamName, 0);
            _ = plusLossesDict.TryAdd(result.Match.AwayTeam.TeamName, 0);

            if (!string.IsNullOrEmpty(result.Result))
            {
                if (result.FavoredTeam.Equals(TargetTeam))
                {
                    if (result.Match.HomeTeam.Equals(TargetTeam))
                    {
                        if (result.Result.Equals(MINUS, StringComparison.OrdinalIgnoreCase))
                        {
                            _numberOfPointSpreadMinusWins += 1;
                            minusWinsDict[result.Match.AwayTeam.TeamName] += 1;
                        }
                        else if (result.Result.Equals(PLUS, StringComparison.OrdinalIgnoreCase))
                        {
                            minusLossesDict[result.Match.AwayTeam.TeamName] += 1;
                        }
                    }
                    else
                    {
                        if (result.Result.Equals(MINUS, StringComparison.OrdinalIgnoreCase))
                        {
                            _numberOfPointSpreadMinusWins += 1;
                            minusWinsDict[result.Match.HomeTeam.TeamName] += 1;
                        }
                        else if (result.Result.Equals(PLUS, StringComparison.OrdinalIgnoreCase))
                        {
                            minusLossesDict[result.Match.HomeTeam.TeamName] += 1;
                        }
                    }

                    _numberOfPointSpreadWhereFavoredMatches += 1;
                }
                else
                {
                    if (result.Match.HomeTeam.Equals(TargetTeam))
                    {
                        if (result.Result.Equals(PLUS, StringComparison.OrdinalIgnoreCase))
                        {
                            _numberOfPointSpreadPlusWins += 1;
                            plusWinsDict[result.Match.AwayTeam.TeamName] += 1;
                        }
                        else
                        {
                            plusLossesDict[result.Match.AwayTeam.TeamName] += 1;
                        }
                    }
                    else
                    {
                        if (result.Result.Equals(PLUS, StringComparison.OrdinalIgnoreCase))
                        {
                            _numberOfPointSpreadPlusWins += 1;
                            plusWinsDict[result.Match.HomeTeam.TeamName] += 1;
                        }
                        else
                        {
                            plusLossesDict[result.Match.HomeTeam.TeamName] += 1;
                        }
                    }

                    _numberOfPointSpreadWhereUnderdogMatches += 1;
                }
            }
        }

        // Build the dictionary the object will expose
        IEnumerable<string> teamNames = minusWinsDict.Keys.Concat(minusLossesDict.Keys).Concat(plusWinsDict.Keys).Concat(plusLossesDict.Keys).Distinct();
        foreach (string name in teamNames)
        {
            _ = _pointSpreadRecords.TryAdd(name, []);

            _pointSpreadRecords[name].Add(new PointSpreadRecord(MINUS, minusWinsDict[name], minusLossesDict[name]));
            _pointSpreadRecords[name].Add(new PointSpreadRecord(PLUS, plusWinsDict[name], plusLossesDict[name]));
        }
        if (_pointSpreadRecords.ContainsKey(TargetTeam.TeamName)) _pointSpreadRecords.Remove(TargetTeam.TeamName);
    }

    private int GetWins(string TeamName, string Side)
    {
        if (_pointSpreadRecords.ContainsKey(TeamName))
        {
            foreach (PointSpreadRecord record in _pointSpreadRecords[TeamName])
            {
                if (record.Side.Equals(Side, StringComparison.OrdinalIgnoreCase))
                {
                    return record.Wins;
                }
            }
        }

        return 0;
    }

    private int GetLosses(string TeamName, string Side)
    {
        if (_pointSpreadRecords.ContainsKey(TeamName))
        {
            foreach (PointSpreadRecord record in _pointSpreadRecords[TeamName])
            {
                if (record.Side.Equals(Side, StringComparison.OrdinalIgnoreCase))
                {
                    return record.Losses;
                }
            }
        }

        return 0;
    }

    private IEnumerable<int> GetWinPoints(string Side)
    {
        foreach (KeyValuePair<string, List<PointSpreadRecord>> record in _pointSpreadRecords)
        {
            foreach (PointSpreadRecord pointSpreadRecords in record.Value)
            {
                if (pointSpreadRecords.Side.Equals(Side, StringComparison.OrdinalIgnoreCase))
                {
                    yield return pointSpreadRecords.Wins;
                }
            }
        }
    }

    private IList<IOverUnder> GetAllOverUnders()
    {
        IList<IOverUnder> allOverUnderMarks;
        if (_daysBack > -1)
        {
            allOverUnderMarks = Repo.OverUnderRepository.GetFromDaysBack(_datePoint, _daysBack);
        }
        else
        {
            allOverUnderMarks = Repo.OverUnderRepository.GetAll();
        }

        IList<IOverUnder> rtnVal = [];
        foreach (IOverUnder mark in allOverUnderMarks)
        {
            if (mark.Match.HomeTeam.Equals(TargetTeam) || mark.Match.AwayTeam.Equals(TargetTeam))
            {
                rtnVal.Add(mark);
            }
        }
        return rtnVal;
    }

    private async Task<IList<IOverUnder>> GetAllOverUndersAsync()
    {
        IList<IOverUnder> allOverUnderMarks;
        if (_daysBack > -1)
        {
            allOverUnderMarks = await Repo.OverUnderRepository.GetFromDaysBackAsync(_datePoint, _daysBack);
        }
        else
        {
            allOverUnderMarks = await Repo.OverUnderRepository.GetAllAsync();
        }

        IList<IOverUnder> rtnVal = [];
        foreach (IOverUnder mark in allOverUnderMarks)
        {
            if (mark.Match.HomeTeam.Equals(TargetTeam) || mark.Match.AwayTeam.Equals(TargetTeam))
            {
                rtnVal.Add(mark);
            }
        }
        return rtnVal;
    }

    private IList<IPointSpread> GetAllPointSpreads()
    {
        IList<IPointSpread> allOverUnderMarks;
        if (_daysBack > -1)
        {
            allOverUnderMarks = Repo.PointSpreadRepository.GetFromDaysBack(_datePoint, _daysBack);
        }
        else
        {
            allOverUnderMarks = Repo.PointSpreadRepository.GetAll();
        }

        IList<IPointSpread> rtnVal = [];
        foreach (IPointSpread spread in allOverUnderMarks)
        {
            if (spread.Match.HomeTeam.Equals(TargetTeam) || spread.Match.AwayTeam.Equals(TargetTeam))
            {
                rtnVal.Add(spread);
            }
        }
        return rtnVal;
    }

    private async Task<IList<IPointSpread>> GetAllPointSpreadsASync()
    {
        IList<IPointSpread> allOverUnderMarks;
        if (_daysBack > -1)
        {
            allOverUnderMarks = await Repo.PointSpreadRepository.GetFromDaysBackAsync(_datePoint, _daysBack);
        }
        else
        {
            allOverUnderMarks = await Repo.PointSpreadRepository.GetAllAsync();
        }

        IList<IPointSpread> rtnVal = [];
        foreach (IPointSpread spread in allOverUnderMarks)
        {
            if (spread.Match.HomeTeam.Equals(TargetTeam) || spread.Match.AwayTeam.Equals(TargetTeam))
            {
                rtnVal.Add(spread);
            }
        }
        return rtnVal;
    }
}
