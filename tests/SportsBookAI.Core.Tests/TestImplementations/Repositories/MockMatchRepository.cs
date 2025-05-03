using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class MockMatchRepository(IRepository<ITeam> TeamRepo) : IRepository<IMatch>
{
    public IList<IMatch> GetAll()
    {
        ITeam? newYork = TeamRepo.GetById(1);
        ITeam? boston = TeamRepo.GetById(2);
        ITeam? miami = TeamRepo.GetById(3);
        ITeam? losAngeles = TeamRepo.GetById(4);
        ITeam? denver = TeamRepo.GetById(5);
        ITeam? seattle = TeamRepo.GetById(6);

        if (newYork != null && boston != null && miami != null && losAngeles != null && denver != null && seattle != null)
        {
            return
            [
                new MockMatch
                {
                    ID = 1,
                    MatchDateTimeLocal = new DateTime(2025, 4, 29, 13, 0, 0), // Local time: April 29, 2025 at 13:00
                    MatchDateTimeUTC = new DateTime(2025, 4, 29, 17, 0, 0, DateTimeKind.Utc), // UTC time: April 29, 2025 at 17:00
                    HomeTeam = newYork,
                    AwayTeam = miami
                },
                new MockMatch
                {
                    ID = 2,
                    MatchDateTimeLocal = new DateTime(2025, 4, 27, 17, 0, 0), // Local time: April 27, 2025 at 17:00
                    MatchDateTimeUTC = new DateTime(2025, 4, 27, 21, 0, 0, DateTimeKind.Utc), // UTC time: April 27, 2025 at 21:00
                    HomeTeam = losAngeles,
                    AwayTeam = boston
                },
                new MockMatch
                {
                    ID = 3,
                    MatchDateTimeLocal = new DateTime(2025, 5, 2, 14, 0, 0), // Local time: May 2, 2025 at 14:00
                    MatchDateTimeUTC = new DateTime(2025, 5, 2, 18, 0, 0, DateTimeKind.Utc), // UTC time: May 2, 2025 at 18:00
                    HomeTeam = denver,
                    AwayTeam = seattle
                }
            ];
        }

        return [];
    }

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
}