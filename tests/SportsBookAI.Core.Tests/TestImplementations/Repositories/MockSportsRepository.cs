using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class MockSportsRepository : ISportsBookRepository
{
    public MockSportsRepository()
    {
        TeamRepository = new MockTeamRepository();
        MatchRepository = new MockMatchRepository(TeamRepository);
    }

    public IRepository<ITeam> TeamRepository { get; private set; }
    public IRepository<IMatch> MatchRepository { get; private set; }
    public IRepository<IOverUnder> OverUnderRepository { get; private set; } = null!;
}