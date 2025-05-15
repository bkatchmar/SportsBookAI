using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class MockTeamRepository : IRepository<ITeam>
{
    private readonly IList<ITeam> _allTeamsInRepo = [
        new MockTeam { ID = 1, TeamName = "New York Hawks", Conference = "Eastern", Division = null },
        new MockTeam { ID = 2, TeamName = "Boston Titans", Conference = "Eastern", Division = null },
        new MockTeam { ID = 3, TeamName = "Miami Stingrays", Conference = "Eastern", Division = null },
        new MockTeam { ID = 4, TeamName = "Los Angeles Vipers", Conference = "Western", Division = null },
        new MockTeam { ID = 5, TeamName = "Denver Blizzards", Conference = "Western", Division = null },
        new MockTeam { ID = 6, TeamName = "Seattle Thunder", Conference = "Western", Division = null }
    ];
    public IList<ITeam> GetAll() => _allTeamsInRepo;
    public Task<IList<ITeam>> GetAllAsync() => Task.FromResult(_allTeamsInRepo);
    public ITeam? GetById(dynamic ObjectId)
    {
        try
        {
            int id = Convert.ToInt32(ObjectId);
            return _allTeamsInRepo.FirstOrDefault(t => t is MockTeam mt && mt.ID == id);
        }
        catch
        {
            return null; // Return null if conversion fails
        }
    }
    public ITeam? GetByName(string Name) => _allTeamsInRepo.FirstOrDefault(t => t.TeamName == Name);

    /// <summary>Not used in mock implementation. Not relevant for teams</summary>
    public IList<ITeam> GetFromDaysBack(DateTime CurrentDate, int DaysBack) => throw new NotImplementedException("MockTeamRepository class does not use or need this method");

    public Task<IList<ITeam>> GetFromDaysBackAsync(DateTime CurrentDate, int DaysBack) => throw new NotImplementedException("MockTeamRepository class does not use or need this method");
}