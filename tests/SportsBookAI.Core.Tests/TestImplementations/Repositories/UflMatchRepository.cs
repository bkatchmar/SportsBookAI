using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class UflMatchRepository : IRepository<IMatch>
{
    private List<MockMatch> _allMatches;
    private IRepository<ITeam> _teamRepo;

    public UflMatchRepository(IRepository<ITeam> TeamRepo)
    {
        _teamRepo = TeamRepo;
        var matches = new List<MockMatch>
        {
            new() {
                ID = 1,
                MatchDateTimeUTC = DateTime.Parse("2025-03-28T17:00:00Z"),
                MatchDateTimeLocal = DateTime.Parse("2025-03-28T13:00:00"),
                HomeTeam = new MockTeam { TeamName = "Houston Roughnecks" },
                AwayTeam = new MockTeam { TeamName = "St. Louis Battlehawks" }
            },
            new() {
                ID = 2,
                MatchDateTimeUTC = DateTime.Parse("2025-03-29T17:00:00Z"),
                MatchDateTimeLocal = DateTime.Parse("2025-03-29T13:00:00"),
                HomeTeam = new MockTeam { TeamName = "Arlington Renegades" },
                AwayTeam = new MockTeam { TeamName = "San Antonio Brahmas" }
            },
            new() {
                ID = 3,
                MatchDateTimeUTC = DateTime.Parse("2025-03-30T17:00:00Z"),
                MatchDateTimeLocal = DateTime.Parse("2025-03-30T13:00:00"),
                HomeTeam = new MockTeam { TeamName = "Memphis Showboats" },
                AwayTeam = new MockTeam { TeamName = "Michigan Panthers" }
            },
            new() {
                ID = 4,
                MatchDateTimeUTC = DateTime.Parse("2025-03-30T17:00:00Z"),
                MatchDateTimeLocal = DateTime.Parse("2025-03-30T13:00:00"),
                HomeTeam = new MockTeam { TeamName = "DC Defenders" },
                AwayTeam = new MockTeam { TeamName = "Birmingham Stallions" }
            }
        };
    }
}
