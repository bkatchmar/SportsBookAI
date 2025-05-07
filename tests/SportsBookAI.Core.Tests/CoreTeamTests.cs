using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.Repositories;

namespace SportsBookAI.Core.Tests;

public class CoreTeamTests
{
    private readonly IRepository<ITeam> teamRepo;

    public CoreTeamTests()
    {
        // Initialize the mock repository once for all test methods
        teamRepo = new MockTeamRepository();
    }

    [Fact]
    public void TestGetAll()
    {
        Assert.Equal(6, teamRepo.GetAll().Count);
    }

    [Fact]
    public void TestGetByNumericId()
    {
        ITeam? newYork = teamRepo.GetById(1);
        ITeam? miami = teamRepo.GetById(3);

        Assert.NotNull(newYork);
        Assert.NotNull(miami);

        Assert.Equal("New York Hawks", newYork.TeamName);
        Assert.Equal("Miami Stingrays", miami.TeamName);
    }
}