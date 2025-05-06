using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.Repositories;

namespace SportsBookAI.Core.Tests;

public class UflExampleTests
{
    private readonly ISportsBookRepository superRepo;

    public UflExampleTests()
    {
        // Initialize the mock repository once for all test methods
        superRepo = new UflSportsRepository();
    }

    [Fact]
    public void GetAllUflDataPoints()
    {
         Assert.Equal(8, superRepo.TeamRepository.GetAll().Count);
         Assert.Equal(24, superRepo.MatchRepository.GetAll().Count);
         Assert.Equal(24, superRepo.OverUnderRepository.GetAll().Count);
    }
}