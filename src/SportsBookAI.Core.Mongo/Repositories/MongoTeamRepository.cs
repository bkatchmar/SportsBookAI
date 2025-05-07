using MongoDB.Driver;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo.Base;

namespace SportsBookAI.Core.Mongo.Repositories;

public class MongoTeamRepository : IRepository<ITeam>, IDisposable
{
    private MongoClient _mongoClient;
    private IMongoDatabase _database;
    private List<MongoTeam> _allTeams;

    public MongoTeamRepository(string DatabaseName)
    {
        _mongoClient = new MongoClient(ConnectionDetails.ConnectionString);
        _database = _mongoClient.GetDatabase(DatabaseName);
        _allTeams = [];
    }

    public IList<ITeam> GetAll()
    {
        CheckGetAllTeams();

        List<ITeam> rtnVal = [];
        rtnVal.AddRange(_allTeams);
        return rtnVal;
    }
    public async Task<IList<ITeam>> GetAllAsync()
    {
        await CheckGetAllTeamsAsync();

        List<ITeam> rtnVal = [];
        rtnVal.AddRange(_allTeams);
        return rtnVal;
    }
    public ITeam? GetById(dynamic ObjectId)
    {
        try
        {
            CheckGetAllTeams();
            string id = ObjectId.ToString();
            return _allTeams.FirstOrDefault(t => t is MongoTeam mt && mt.Id == id);
        }
        catch
        {
            return null; // Return null if conversion fails
        }
    }
    public ITeam? GetByName(string Name)
    {
        CheckGetAllTeams();
        return _allTeams.Find(t => t.TeamName == Name);
    }

    [Obsolete("This method is not applicable for TeamRepository and will always throw a NotImplementedException.", true)]
    public IList<ITeam> GetFromDaysBack(DateTime CurrentDate, int DaysBack)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _mongoClient.Dispose();
    }

    private void CheckGetAllTeams()
    {
        if (!_allTeams.Any())
        {
            IMongoCollection<MongoTeam> teamCollection = _database.GetCollection<MongoTeam>("teams");
            _allTeams = teamCollection.Find(_ => true).SortBy(team => team.TeamName).ToList();
        }
    }

    private async Task CheckGetAllTeamsAsync()
    {
        if (!_allTeams.Any())
        {
            IMongoCollection<MongoTeam> teamCollection = _database.GetCollection<MongoTeam>("teams");
            _allTeams = await teamCollection.Find(_ => true).SortBy(team => team.TeamName).ToListAsync();
        }
    }
}