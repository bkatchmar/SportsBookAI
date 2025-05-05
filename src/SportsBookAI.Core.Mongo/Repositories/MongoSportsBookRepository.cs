using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Mongo.Repositories;

public class MongoSportsBookRepository : ISportsBookRepository, IDisposable
{
    private MongoTeamRepository _teamRepo;

    public MongoSportsBookRepository(string DatabaseName)
    {
        _teamRepo = new MongoTeamRepository(DatabaseName);
    }
    
    public IRepository<ITeam> TeamRepository => _teamRepo;

    public IRepository<IMatch> MatchRepository => throw new NotImplementedException();

    public void Dispose()
    {
        _teamRepo.Dispose();
    }
}
