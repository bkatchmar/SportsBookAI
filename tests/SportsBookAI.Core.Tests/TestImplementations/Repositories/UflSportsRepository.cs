using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class UflSportsRepository : ISportsBookRepository
{
    public UflSportsRepository()
    {
        TeamRepository = new UflExampleTeamRepo();
        MatchRepository = new UflMatchRepository(TeamRepository);
        OverUnderRepository = new UflOverUnderRepository(MatchRepository);
    }

    public IRepository<ITeam> TeamRepository { get; private set; }
    public IRepository<IMatch> MatchRepository { get; private set; }
    public IRepository<IOverUnder> OverUnderRepository { get; private set; }
}