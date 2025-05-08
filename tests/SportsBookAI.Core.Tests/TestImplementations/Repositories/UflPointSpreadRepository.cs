using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class UflPointSpreadRepository : IRepository<IPointSpread>
{
    private List<MockPointSpread> _allPointSpreads;
    private IRepository<IMatch> _matchRepo;

    public UflPointSpreadRepository(IRepository<IMatch> MatchRepo)
    {
        _matchRepo = MatchRepo;
        _allPointSpreads = GetAllHardcodedOverUnders();
    }

    public IList<IPointSpread> GetAll()
    {
        List<IPointSpread> rtnVal = [];
        rtnVal.AddRange(_allPointSpreads);
        return rtnVal;
    }
    public Task<IList<IPointSpread>> GetAllAsync() => Task.FromResult(GetAll());

    public IPointSpread? GetById(dynamic ObjectId) => throw new NotImplementedException("MockMatchRepository class does not use or need this method");

    /// <summary>Not used in mock implementation. Not relevant for teams</summary>
    public IPointSpread? GetByName(string Name) => throw new NotImplementedException("MockMatchRepository class does not use or need this method");

    public IList<IPointSpread> GetFromDaysBack(DateTime CurrentDate, int DaysBack)
    {
        DateTime earliestDate = CurrentDate.AddDays(-DaysBack);

        // Filter matches that fall within the range [earliestDate, CurrentDate)
        return GetAll()
            .Where(m => m.Match.MatchDateTimeLocal >= earliestDate && m.Match.MatchDateTimeLocal < CurrentDate)
            .Cast<IPointSpread>()
            .ToList();
    }

    private List<MockPointSpread> GetAllHardcodedOverUnders()
    {
        List<MockPointSpread> rtnVal = [];

        // Week 1
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(1) ?? _matchRepo.GetAll().First(),
            Spread = 4.5,
            Result = "MINUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(2) ?? _matchRepo.GetAll().First(),
            Spread = 1,
            Result = "MINUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(3) ?? _matchRepo.GetAll().First(),
            Spread = 7,
            Result = "MINUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(4) ?? _matchRepo.GetAll().First(),
            Spread = 7.5,
            Result = "PLUS"
        });

        // Week 2
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(5) ?? _matchRepo.GetAll().First(),
            Spread = 1,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(6) ?? _matchRepo.GetAll().First(),
            Spread = 6.5,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(7) ?? _matchRepo.GetAll().First(),
            Spread = 12,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(8) ?? _matchRepo.GetAll().First(),
            Spread = 8,
            Result = "MINUS"
        });

        // Week 3
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(9) ?? _matchRepo.GetAll().First(),
            Spread = 5.5,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(10) ?? _matchRepo.GetAll().First(),
            Spread = 6,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(11) ?? _matchRepo.GetAll().First(),
            Spread = 7,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(12) ?? _matchRepo.GetAll().First(),
            Spread = 7,
            Result = "PLUS"
        });

        // Week 4
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(13) ?? _matchRepo.GetAll().First(),
            Spread = 7,
            Result = "MINUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(14) ?? _matchRepo.GetAll().First(),
            Spread = 1,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(15) ?? _matchRepo.GetAll().First(),
            Spread = 7.5,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(16) ?? _matchRepo.GetAll().First(),
            Spread = 7.5,
            Result = "PLUS"
        });

        // Week 5
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(17) ?? _matchRepo.GetAll().First(),
            Spread = 11,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(18) ?? _matchRepo.GetAll().First(),
            Spread = 1,
            Result = "MINUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(19) ?? _matchRepo.GetAll().First(),
            Spread = 3.5,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(20) ?? _matchRepo.GetAll().First(),
            Spread = 3.5,
            Result = "MINUS"
        });

        // Week 6
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(21) ?? _matchRepo.GetAll().First(),
            Spread = 3,
            Result = "MINUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(22) ?? _matchRepo.GetAll().First(),
            Spread = 5.5,
            Result = "PLUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(23) ?? _matchRepo.GetAll().First(),
            Spread = 2.5,
            Result = "MINUS"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(24) ?? _matchRepo.GetAll().First(),
            Spread = 8.5,
            Result = "MINUS"
        });

        return rtnVal;
    }
}