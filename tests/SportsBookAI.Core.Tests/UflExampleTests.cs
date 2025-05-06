using SportsBookAI.Core.Classes;
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

    [Fact]
    public void TestGetCoreClassForBaseAggregatiosn()
    {
        IAggregator baseAggregatorForTestUflData = new BaseAggregator("UFL", superRepo);
        baseAggregatorForTestUflData.Aggregate();

        Assert.Equal(4, baseAggregatorForTestUflData.OversByTeam["DC Defenders"]);
        Assert.Equal(2, baseAggregatorForTestUflData.OversByTeam["Birmingham Stallions"]);

        Assert.Equal(2, baseAggregatorForTestUflData.UndersByTeam["DC Defenders"]);
        Assert.Equal(2, baseAggregatorForTestUflData.UndersByTeam["Michigan Panthers"]);
        Assert.Equal(2, baseAggregatorForTestUflData.UndersByTeam["Memphis Showboats"]);
        Assert.Equal(4, baseAggregatorForTestUflData.UndersByTeam["Birmingham Stallions"]);

        Assert.Equal(3, baseAggregatorForTestUflData.TotalUniqueOvers.Count());
        Assert.Contains(4, baseAggregatorForTestUflData.TotalUniqueOvers);
        Assert.Contains(3, baseAggregatorForTestUflData.TotalUniqueOvers);
        Assert.Contains(2, baseAggregatorForTestUflData.TotalUniqueOvers);

        Assert.Equal(3, baseAggregatorForTestUflData.TotalUniqueUnders.Count());
        Assert.Contains(4, baseAggregatorForTestUflData.TotalUniqueUnders);
        Assert.Contains(3, baseAggregatorForTestUflData.TotalUniqueUnders);
        Assert.Contains(2, baseAggregatorForTestUflData.TotalUniqueUnders);
    }
}