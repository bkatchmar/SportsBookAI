using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class UflOverUnderRepository : IRepository<IOverUnder>
{
    private List<MockOverUnder> _allOverUnderMarks;
    private IRepository<IMatch> _matchRepo;

    public UflOverUnderRepository(IRepository<IMatch> MatchRepo)
    {
        _matchRepo = MatchRepo;
        _allOverUnderMarks = GetAllHardcodedOverUnders();
    }

    public IList<IOverUnder> GetAll()
    {
        List<IOverUnder> rtnVal = [];
        rtnVal.AddRange(_allOverUnderMarks);
        return rtnVal;
    }
    public Task<IList<IOverUnder>> GetAllAsync() => Task.FromResult(GetAll());

    public IOverUnder? GetById(dynamic ObjectId) => throw new NotImplementedException("MockMatchRepository class does not use or need this method");

    /// <summary>Not used in mock implementation. Not relevant for teams</summary>
    public IOverUnder? GetByName(string Name) => throw new NotImplementedException("MockMatchRepository class does not use or need this method");

    public IList<IOverUnder> GetFromDaysBack(DateTime CurrentDate, int DaysBack)
    {
        DateTime earliestDate = CurrentDate.AddDays(-DaysBack);
        
        // Filter matches that fall within the range [earliestDate, CurrentDate)
        return GetAll()
            .Where(m => m.Match.MatchDateTimeLocal >= earliestDate && m.Match.MatchDateTimeLocal < CurrentDate)
            .Cast<IOverUnder>()
            .ToList();
    }

    private List<MockOverUnder> GetAllHardcodedOverUnders() 
    {
        List<MockOverUnder> rtnVal = [];

        // Week 1
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(1) ?? _matchRepo.GetAll().First(),
            Mark = 37.5,
            Hit = "Under"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(2) ?? _matchRepo.GetAll().First(),
            Mark = 39,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(3) ?? _matchRepo.GetAll().First(),
            Mark = 39,
            Hit = "Under"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(4) ?? _matchRepo.GetAll().First(),
            Mark = 42,
            Hit = "Under"
        });

        // Week 2
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(5) ?? _matchRepo.GetAll().First(),
            Mark = 38,
            Hit = "Under"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(6) ?? _matchRepo.GetAll().First(),
            Mark = 37,
            Hit = "Under"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(7) ?? _matchRepo.GetAll().First(),
            Mark = 40,
            Hit = "Under"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(8) ?? _matchRepo.GetAll().First(),
            Mark = 40,
            Hit = "Under"
        });

        // Week 3
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(9) ?? _matchRepo.GetAll().First(),
            Mark = 39.5,
            Hit = "Under"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(10) ?? _matchRepo.GetAll().First(),
            Mark = 34.5,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(11) ?? _matchRepo.GetAll().First(),
            Mark = 34.5,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(12) ?? _matchRepo.GetAll().First(),
            Mark = 39,
            Hit = "Over"
        });

        // Week 4
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(13) ?? _matchRepo.GetAll().First(),
            Mark = 36,
            Hit = "Push"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(14) ?? _matchRepo.GetAll().First(),
            Mark = 40,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(15) ?? _matchRepo.GetAll().First(),
            Mark = 37,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(16) ?? _matchRepo.GetAll().First(),
            Mark = 37.5,
            Hit = "Over"
        });

        // Week 5
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(17) ?? _matchRepo.GetAll().First(),
            Mark = 37.5,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(18) ?? _matchRepo.GetAll().First(),
            Mark = 37.5,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(19) ?? _matchRepo.GetAll().First(),
            Mark = 39,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(20) ?? _matchRepo.GetAll().First(),
            Mark = 38.5,
            Hit = "Under"
        });

        // Week 6
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(21) ?? _matchRepo.GetAll().First(),
            Mark = 42.5,
            Hit = "Under"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(22) ?? _matchRepo.GetAll().First(),
            Mark = 36.5,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(23) ?? _matchRepo.GetAll().First(),
            Mark = 41.5,
            Hit = "Over"
        });
        rtnVal.Add(new()
        {
            Match = _matchRepo.GetById(24) ?? _matchRepo.GetAll().First(),
            Mark = 37.5,
            Hit = "Under"
        });

        return rtnVal;
    }
}