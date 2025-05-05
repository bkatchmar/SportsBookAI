using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Mongo.Repositories;

public class MongoSportsBookRepository : ISportsBookRepository, IDisposable
{
    private MongoTeamRepository _teamRepo;
    private MongoMatchRepository _matchRepo;

    public MongoSportsBookRepository(string DatabaseName)
    {
        _teamRepo = new(DatabaseName);
        _matchRepo = new(DatabaseName, _teamRepo.GetAll().ToList());
    }

    public IRepository<ITeam> TeamRepository => _teamRepo;

    public IRepository<IMatch> MatchRepository => _matchRepo;

    public void Dispose()
    {
        _teamRepo.Dispose();
        _matchRepo.Dispose();
    }
}
