using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Mongo.Repositories;

public class MongoSportsBookRepository : ISportsBookRepository, IDisposable
{
    private MongoTeamRepository _teamRepo;
    private MongoMatchRepository _matchRepo;
    private MongoOverUnderRepository _overUnderRepo;

    public MongoSportsBookRepository(string DatabaseName)
    {
        _teamRepo = new(DatabaseName);
        _matchRepo = new(DatabaseName, _teamRepo.GetAll().ToList());
        _overUnderRepo = new(DatabaseName, _matchRepo.GetAll().ToList());
    }

    public IRepository<ITeam> TeamRepository => _teamRepo;

    public IRepository<IMatch> MatchRepository => _matchRepo;

    public IRepository<IOverUnder> OverUnderRepository => _overUnderRepo;

    public void Dispose()
    {
        _teamRepo.Dispose();
        _matchRepo.Dispose();
        _overUnderRepo.Dispose();
    }
}
