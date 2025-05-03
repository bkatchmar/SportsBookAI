using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.Repositories;

namespace SportsBookAI.Core.Tests;

public class CoreMatchTests
{
    private readonly IRepository<ITeam> teamRepo;
    private readonly IRepository<IMatch> matchRepo;

    public CoreMatchTests()
    {
        // Initialize the mock repository once for all test methods
        teamRepo = new MockTeamRepository();
        matchRepo = new MockMatchRepository(teamRepo);
    }

    [Fact]
    public void TestGetAll()
    {
         Assert.Equal(3, matchRepo.GetAll().Count);
    }

    [Fact]
    public void TestTwoMatchesFromMayFirst()
    {
         Assert.Equal(2, matchRepo.GetFromDaysBack(new DateTime(2025, 5, 1), 7).Count);
    }
}
