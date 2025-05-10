using SportsBookAI.Core.Classes;
using SportsBookAI.Core.Classes.Patterns;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;
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
        Assert.True(allBasePredictionPatterns.Count > 0);

        // One more thing, take a game vs highest overs (DC Defenders) and match them against the highest unders (Stallions) and see if pattern ID 3 refuses to return a prediction
        ITeam? dcDefenders = superRepo.TeamRepository.GetByName("DC Defenders");
        ITeam? birminghamStallions = superRepo.TeamRepository.GetByName("Birmingham Stallions");
        Assert.NotNull(dcDefenders);
        Assert.NotNull(birminghamStallions);
        MockMatch defendersVsStallions = new()
        {
            ID = 200,
            HomeTeam = dcDefenders,
            AwayTeam = birminghamStallions,
            MatchDateTimeLocal = new DateTime(2025, 05, 12),
            MatchDateTimeUTC = new DateTime(2025, 05, 12)
        };

        IList<IPredictionPattern> specificPredictions = basePredicitonRepo.GetAllPredictions([defendersVsStallions]);
        Assert.Contains(specificPredictions, p => p.ID == 1);
        Assert.Contains(specificPredictions, p => p.ID == 2);
        Assert.DoesNotContain(specificPredictions, p => p.ID == 3);
        Assert.DoesNotContain(specificPredictions, p => p.ID == 4);
        Assert.DoesNotContain(specificPredictions, p => p.ID == 5);
    }

    [Fact]
    public void GetOverUnderPercentagesForUflSampleData()
    {
        IAggregator baseAggregatorForTestUflData = new BaseAggregator("UFL", superRepo);
        baseAggregatorForTestUflData.Aggregate();

        Assert.True(baseAggregatorForTestUflData.AllOverPercentage > 0); // Should be 0.5
        Assert.True(baseAggregatorForTestUflData.AllUnderPercentage > 0); // Should be 0.4583333.....
        Assert.True(baseAggregatorForTestUflData.AllOverPercentage > baseAggregatorForTestUflData.AllUnderPercentage);
    }

    [Fact]
    public void GetPointSpreadForUflSampleData()
    {
        IAggregator baseAggregatorForTestUflData = new BaseAggregator("UFL", superRepo);
        baseAggregatorForTestUflData.Aggregate();

        // Check the aggregation records for the 'Houston Roughnecks', Minus Record 0-1, Plus Record 3-2
        Assert.Equal(0, baseAggregatorForTestUflData.GetTeamMinusSideWins("Houston Roughnecks"));
        Assert.Equal(1, baseAggregatorForTestUflData.GetTeamMinusSideLosses("Houston Roughnecks"));
        Assert.Equal(3, baseAggregatorForTestUflData.GetTeamPlusSideWins("Houston Roughnecks"));
        Assert.Equal(2, baseAggregatorForTestUflData.GetTeamPlusSideLosses("Houston Roughnecks"));

        Assert.True(baseAggregatorForTestUflData.AllMinusSpreadsPercentage > 0);
        Assert.True(baseAggregatorForTestUflData.AllPlusSpreadsPercentage > 0);
        Assert.True(baseAggregatorForTestUflData.AllPlusSpreadsPercentage > baseAggregatorForTestUflData.AllMinusSpreadsPercentage);
        Assert.True(baseAggregatorForTestUflData.AllPlusSpreadsPercentage < 0.6);
    }

    [Fact]
    public void TrustId4OmitsPick()
    {
        IAggregator baseAggregatorForTestUflData = new BaseAggregator("UFL", superRepo);
        baseAggregatorForTestUflData.Aggregate();

        // One more thing, take a game vs highest overs (DC Defenders) and match them against the highest unders (Stallions) and see if pattern ID 3 refuses to return a prediction
        ITeam? houstonRoughnecks = superRepo.TeamRepository.GetByName("Houston Roughnecks");
        ITeam? birminghamStallions = superRepo.TeamRepository.GetByName("Birmingham Stallions");
        Assert.NotNull(houstonRoughnecks);
        Assert.NotNull(birminghamStallions);
        MockMatch roughnecksVsStallions = new()
        {
            ID = 200,
            HomeTeam = birminghamStallions,
            AwayTeam = birminghamStallions,
            MatchDateTimeLocal = new DateTime(2025, 05, 12),
            MatchDateTimeUTC = new DateTime(2025, 05, 12)
        };

        IPredictionPattern id4 = new PickMajorityOverUnderIfBothTeamsAreMiddleOfPack(baseAggregatorForTestUflData, roughnecksVsStallions);
        Assert.False(id4.PredictionMade);
    }

    [Fact]
    public void GetLookupIfPointSpreadsNeedPredictions()
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
            Assert.True(baseAggregatorForTestUflData.DoesThisMatchNeedPointSpreadPrediction(weekSevenMatch));
        }
    }

    [Fact]
    public void PretdictMinusSideIfFavoredTeamHasBetterRecordThanOpponentOnPlusSide()
    {
        Assert.Equal(1,1);
    }
}