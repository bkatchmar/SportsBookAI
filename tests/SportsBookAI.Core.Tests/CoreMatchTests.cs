using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.Repositories;

namespace SportsBookAI.Core.Tests;

public class CoreMatchTests
{
    private readonly ISportsBookRepository superRepo;

    public CoreMatchTests()
    {
        // Initialize the mock repository once for all test methods
        superRepo = new MockSportsRepository();
    }

    [Fact]
    public void TestGetAll()
    {
         Assert.Equal(3, superRepo.MatchRepository.GetAll().Count);
    }

    [Fact]
    public void TestTwoMatchesFromMayFirst()
    {
         Assert.Equal(2, superRepo.MatchRepository.GetFromDaysBack(new DateTime(2025, 5, 1), 7).Count);
    }
}
