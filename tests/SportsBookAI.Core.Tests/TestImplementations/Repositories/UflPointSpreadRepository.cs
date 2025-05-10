using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class UflPointSpreadRepository : IRepository<IPointSpread>
{
    private List<MockPointSpread> _allPointSpreads;
    private IRepository<IMatch> _matchRepo;
    private IRepository<ITeam> _teamRepo;

    public UflPointSpreadRepository(IRepository<IMatch> MatchRepo, IRepository<ITeam> TeamRepo)
    {
        _matchRepo = MatchRepo;
        _teamRepo = TeamRepo;
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

        // Get team variables
        ITeam? arlington = _teamRepo.GetByName("Arlington Renegades");
        ITeam? birmingham = _teamRepo.GetByName("Birmingham Stallions");
        ITeam? dc = _teamRepo.GetByName("DC Defenders");
        ITeam? houston = _teamRepo.GetByName("Houston Roughnecks");
        ITeam? memphis = _teamRepo.GetByName("Memphis Showboats");
        ITeam? michigan = _teamRepo.GetByName("Michigan Panthers");
        ITeam? sanAntonio = _teamRepo.GetByName("San Antonio Brahmas");
        ITeam? stLouis = _teamRepo.GetByName("St. Louis Battlehawks");

        if (arlington != null && stLouis != null && birmingham != null && dc != null && houston != null && memphis != null && michigan != null && sanAntonio != null)
        {
            // Week 1
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(1) ?? _matchRepo.GetAll().First(),
                Spread = 4.5,
                Result = "MINUS",
                FavoredTeam = stLouis
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(2) ?? _matchRepo.GetAll().First(),
                Spread = 1,
                Result = "MINUS",
                FavoredTeam = arlington
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(3) ?? _matchRepo.GetAll().First(),
                Spread = 7,
                Result = "MINUS",
                FavoredTeam = michigan
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(4) ?? _matchRepo.GetAll().First(),
                Spread = 7.5,
                Result = "PLUS",
                FavoredTeam = birmingham
            });

            // Week 2
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(5) ?? _matchRepo.GetAll().First(),
                Spread = 1,
                Result = "PLUS",
                FavoredTeam = michigan
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(6) ?? _matchRepo.GetAll().First(),
                Spread = 6.5,
                Result = "PLUS",
                FavoredTeam = dc
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(7) ?? _matchRepo.GetAll().First(),
                Spread = 12,
                Result = "PLUS",
                FavoredTeam = arlington
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(8) ?? _matchRepo.GetAll().First(),
                Spread = 8,
                Result = "MINUS",
                FavoredTeam = stLouis
            });

            // Week 3
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(9) ?? _matchRepo.GetAll().First(),
                Spread = 5.5,
                Result = "PLUS",
                FavoredTeam = birmingham
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(10) ?? _matchRepo.GetAll().First(),
                Spread = 6,
                Result = "PLUS",
                FavoredTeam = memphis
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(11) ?? _matchRepo.GetAll().First(),
                Spread = 7,
                Result = "PLUS",
                FavoredTeam = michigan
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(12) ?? _matchRepo.GetAll().First(),
                Spread = 7,
                Result = "PLUS",
                FavoredTeam = stLouis
            });

            // Week 4
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(13) ?? _matchRepo.GetAll().First(),
                Spread = 7,
                Result = "MINUS",
                FavoredTeam = michigan
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(14) ?? _matchRepo.GetAll().First(),
                Spread = 1,
                Result = "PLUS",
                FavoredTeam = stLouis
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(15) ?? _matchRepo.GetAll().First(),
                Spread = 7.5,
                Result = "PLUS",
                FavoredTeam = birmingham
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(16) ?? _matchRepo.GetAll().First(),
                Spread = 7.5,
                Result = "PLUS",
                FavoredTeam = dc
            });

            // Week 5
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(17) ?? _matchRepo.GetAll().First(),
                Spread = 11,
                Result = "PLUS",
                FavoredTeam = birmingham
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(18) ?? _matchRepo.GetAll().First(),
                Spread = 1,
                Result = "MINUS",
                FavoredTeam = stLouis
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(19) ?? _matchRepo.GetAll().First(),
                Spread = 3.5,
                Result = "PLUS",
                FavoredTeam = arlington
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(20) ?? _matchRepo.GetAll().First(),
                Spread = 3.5,
                Result = "MINUS",
                FavoredTeam = sanAntonio
            });

            // Week 6
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(21) ?? _matchRepo.GetAll().First(),
                Spread = 3,
                Result = "MINUS",
                FavoredTeam = stLouis
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(22) ?? _matchRepo.GetAll().First(),
                Spread = 5.5,
                Result = "PLUS",
                FavoredTeam = houston
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(23) ?? _matchRepo.GetAll().First(),
                Spread = 2.5,
                Result = "MINUS",
                FavoredTeam = michigan
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(24) ?? _matchRepo.GetAll().First(),
                Spread = 8.5,
                Result = "MINUS",
                FavoredTeam = birmingham
            });

            // Week 7
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(25) ?? _matchRepo.GetAll().First(),
                Spread = 6.5,
                Result = "",
                FavoredTeam = dc
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(26) ?? _matchRepo.GetAll().First(),
                Spread = 1.5,
                Result = "",
                FavoredTeam = michigan
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(27) ?? _matchRepo.GetAll().First(),
                Spread = 6.5,
                Result = "",
                FavoredTeam = birmingham
            });
            rtnVal.Add(new()
            {
                Match = _matchRepo.GetById(28) ?? _matchRepo.GetAll().First(),
                Spread = 4.5,
                Result = "",
                FavoredTeam = stLouis
            });
        }

        return rtnVal;
    }
}