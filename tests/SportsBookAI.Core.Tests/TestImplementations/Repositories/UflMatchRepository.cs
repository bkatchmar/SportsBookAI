using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class UflMatchRepository : IRepository<IMatch>
{
    private List<MockMatch> _allUflMatches;
    private IRepository<ITeam> _teamRepo;

    public UflMatchRepository(IRepository<ITeam> TeamRepo)
    {
        _teamRepo = TeamRepo;
        _allUflMatches = GetAllMatches();
    }

    public IList<IMatch> GetAll()
    {
        List<IMatch> rtnVal = [];
        rtnVal.AddRange(_allUflMatches);
        return rtnVal;
    }
    public Task<IList<IMatch>> GetAllAsync() => Task.FromResult(GetAll());

    public IMatch? GetById(dynamic ObjectId)
    {
        try
        {
            int id = Convert.ToInt32(ObjectId);
            return GetAll().FirstOrDefault(t => t is MockMatch mt && mt.ID == id);
        }
        catch
        {
            return null; // Return null if conversion fails
        }
    }

    /// <summary>Not used in mock implementation. Not relevant for teams</summary>
    public IMatch? GetByName(string Name) => throw new NotImplementedException("MockMatchRepository class does not use or need this method");

    public IList<IMatch> GetFromDaysBack(DateTime CurrentDate, int DaysBack)
    {
        DateTime earliestDate = CurrentDate.AddDays(-DaysBack);

        // Filter matches that fall within the range [earliestDate, CurrentDate)
        return GetAll()
            .Where(m => m.MatchDateTimeLocal >= earliestDate && m.MatchDateTimeLocal < CurrentDate)
            .Cast<IMatch>()
            .ToList();
    }

    public Task<IList<IMatch>> GetFromDaysBackAsync(DateTime CurrentDate, int DaysBack)
    {
        DateTime earliestDate = CurrentDate.AddDays(-DaysBack);

        // Filter matches that fall within the range [earliestDate, CurrentDate)
        IList<IMatch> lookup = GetAll()
            .Where(m => m.MatchDateTimeLocal >= earliestDate && m.MatchDateTimeLocal < CurrentDate)
            .Cast<IMatch>()
            .ToList();

        return Task.FromResult(lookup);
    }

    private List<MockMatch> GetAllMatches()
    {
        // Get team variables
        ITeam? arlington = _teamRepo.GetByName("Arlington Renegades");
        ITeam? birmingham = _teamRepo.GetByName("Birmingham Stallions");
        ITeam? dc = _teamRepo.GetByName("DC Defenders");
        ITeam? houston = _teamRepo.GetByName("Houston Roughnecks");
        ITeam? memphis = _teamRepo.GetByName("Memphis Showboats");
        ITeam? michigan = _teamRepo.GetByName("Michigan Panthers");
        ITeam? sanAntonio = _teamRepo.GetByName("San Antonio Brahmas");
        ITeam? stLouis = _teamRepo.GetByName("St. Louis Battlehawks");

        List<MockMatch> rtnVal = [];

        if (arlington != null && stLouis != null && birmingham != null && dc != null && houston != null && memphis != null && michigan != null && sanAntonio != null)
        {
            // Week 1
            rtnVal.Add(new()
            {
                ID = 1,
                HomeTeam = houston,
                AwayTeam = stLouis,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 1
            });
            rtnVal.Add(new()
            {
                ID = 2,
                HomeTeam = arlington,
                AwayTeam = sanAntonio,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 1
            });
            rtnVal.Add(new()
            {
                ID = 3,
                HomeTeam = memphis,
                AwayTeam = michigan,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 1
            });
            rtnVal.Add(new()
            {
                ID = 4,
                HomeTeam = dc,
                AwayTeam = birmingham,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 1
            });

            // Week 2
            rtnVal.Add(new()
            {
                ID = 5,
                HomeTeam = michigan,
                AwayTeam = birmingham,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 2
            });
            rtnVal.Add(new()
            {
                ID = 6,
                HomeTeam = dc,
                AwayTeam = memphis,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 2
            });
            rtnVal.Add(new()
            {
                ID = 7,
                HomeTeam = arlington,
                AwayTeam = houston,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 2
            });
            rtnVal.Add(new()
            {
                ID = 8,
                HomeTeam = stLouis,
                AwayTeam = sanAntonio,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 2
            });

            // Week 3
            rtnVal.Add(new()
            {
                ID = 9,
                HomeTeam = birmingham,
                AwayTeam = arlington,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 3
            });
            rtnVal.Add(new()
            {
                ID = 10,
                HomeTeam = memphis,
                AwayTeam = houston,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 3
            });
            rtnVal.Add(new()
            {
                ID = 11,
                HomeTeam = michigan,
                AwayTeam = sanAntonio,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 3
            });
            rtnVal.Add(new()
            {
                ID = 12,
                HomeTeam = stLouis,
                AwayTeam = dc,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 3
            });

            // Week 4
            rtnVal.Add(new()
            {
                ID = 13,
                HomeTeam = michigan,
                AwayTeam = memphis,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 4
            });
            rtnVal.Add(new()
            {
                ID = 14,
                HomeTeam = arlington,
                AwayTeam = stLouis,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 4
            });
            rtnVal.Add(new()
            {
                ID = 15,
                HomeTeam = houston,
                AwayTeam = birmingham,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 4
            });
            rtnVal.Add(new()
            {
                ID = 16,
                HomeTeam = dc,
                AwayTeam = sanAntonio,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 4
            });

            // Week 5
            rtnVal.Add(new()
            {
                ID = 17,
                HomeTeam = birmingham,
                AwayTeam = memphis,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 5
            });
            rtnVal.Add(new()
            {
                ID = 18,
                HomeTeam = stLouis,
                AwayTeam = michigan,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 5
            });
            rtnVal.Add(new()
            {
                ID = 19,
                HomeTeam = arlington,
                AwayTeam = dc,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 5
            });
            rtnVal.Add(new()
            {
                ID = 20,
                HomeTeam = sanAntonio,
                AwayTeam = houston,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 5
            });

            // Week 6
            rtnVal.Add(new()
            {
                ID = 21,
                HomeTeam = stLouis,
                AwayTeam = arlington,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 6
            });
            rtnVal.Add(new()
            {
                ID = 22,
                HomeTeam = houston,
                AwayTeam = memphis,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 6
            });
            rtnVal.Add(new()
            {
                ID = 23,
                HomeTeam = michigan,
                AwayTeam = dc,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 6
            });
            rtnVal.Add(new()
            {
                ID = 24,
                HomeTeam = birmingham,
                AwayTeam = sanAntonio,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                WeekNumber = 6
            });

            // Week 7
            rtnVal.Add(new()
            {
                ID = 25,
                HomeTeam = sanAntonio,
                AwayTeam = dc,
                MatchDateTimeUTC = DateTime.Parse("2025-05-09T20:00:00", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-05-09T20:00:00"),
                WeekNumber = 7
            });
            rtnVal.Add(new()
            {
                ID = 26,
                HomeTeam = arlington,
                AwayTeam = michigan,
                MatchDateTimeUTC = DateTime.Parse("2025-05-10T13:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-05-10T13:00:00"),
                WeekNumber = 7
            });
            rtnVal.Add(new()
            {
                ID = 27,
                HomeTeam = birmingham,
                AwayTeam = houston,
                MatchDateTimeUTC = DateTime.Parse("2025-05-11T12:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-05-11T12:00:00"),
                WeekNumber = 7
            });
            rtnVal.Add(new()
            {
                ID = 28,
                HomeTeam = memphis,
                AwayTeam = stLouis,
                MatchDateTimeUTC = DateTime.Parse("2025-05-11T15:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2025-05-11T15:00:00"),
                WeekNumber = 7
            });

            // I need a fake match for testing purposes, oh well, this is a test repo class after all...
            rtnVal.Add(new()
            {
                ID = 200,
                HomeTeam = stLouis,
                AwayTeam = memphis,
                MatchDateTimeUTC = DateTime.Parse("2026-05-11T15:00:00Z", null, System.Globalization.DateTimeStyles.AdjustToUniversal),
                MatchDateTimeLocal = DateTime.Parse("2026-05-11T15:00:00"),
                WeekNumber = 99
            });
        }

        return rtnVal;
    }
}