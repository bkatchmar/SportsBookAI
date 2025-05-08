using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class UflSportsbookRepository : ISportsBookRepository
{
    public UflSportsbookRepository()
    {
        TeamRepository = new UflExampleTeamRepo();
        MatchRepository = new MockMatchRepository(TeamRepository);
    }

    public IRepository<ITeam> TeamRepository { get; private set; }
    public IRepository<IMatch> MatchRepository { get; private set; }
    public IRepository<IOverUnder> OverUnderRepository => null!;
    public IRepository<IPointSpread> PointSpreadRepository => null!;
}