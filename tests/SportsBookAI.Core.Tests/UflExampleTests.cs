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
        Assert.Equal(28, superRepo.MatchRepository.GetAll().Count);
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

    [Fact]
    public void GeneratePreidctionsOverWeekSevenMatches()
    {
        IAggregator baseAggregatorForTestUflData = new BaseAggregator("UFL", superRepo);
        baseAggregatorForTestUflData.Aggregate();

        BasePatternRepo basePredicitonRepo = new(baseAggregatorForTestUflData);

        // Grab all 4 matches for "Week Seven" 
        IList<IMatch> weekSevenMatches = superRepo.MatchRepository.GetFromDaysBack(new DateTime(2025, 05, 12), 3);
        Assert.Equal(4, weekSevenMatches.Count);

        // TEST: These should all be "true" as in this example, we don't have "Over/Under" predictions for these matches yet
        foreach (IMatch weekSevenMatch in weekSevenMatches)
        {
            Assert.True(baseAggregatorForTestUflData.DoesThisMatchNeedOverUnderPrediction(weekSevenMatch));
        }

        // Get all Over Under Marks
        IList<IOverUnder> marks = superRepo.OverUnderRepository.GetAll();
        IEnumerable<IMatch> playedMatches = marks.Select(m => m.Match);

        // Since we have matches from the over under marks, test to make sure that yeah, you don't need to make an Over Under prediction
        Assert.Empty(playedMatches.Where(m => baseAggregatorForTestUflData.DoesThisMatchNeedOverUnderPrediction(m)));

        // Time to make some predictions
        IList<IPredictionPattern> allBasePredictionPatterns = basePredicitonRepo.GetAllPredictions(weekSevenMatches);
        Assert.Equal(4, allBasePredictionPatterns.Count);
        Assert.Equal("DC Defenders/San Antonio Brahmas Over", allBasePredictionPatterns.ElementAt(0).PredictionText);
        Assert.Equal("DC Defenders/San Antonio Brahmas Over", allBasePredictionPatterns.ElementAt(1).PredictionText);
        Assert.Equal("Houston Roughnecks/Birmingham Stallions Under", allBasePredictionPatterns.ElementAt(2).PredictionText);
        Assert.Equal("Houston Roughnecks/Birmingham Stallions Under", allBasePredictionPatterns.ElementAt(4).PredictionText);
    }
}