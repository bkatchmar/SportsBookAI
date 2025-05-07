using MongoDB.Driver;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo.Base;

namespace SportsBookAI.Core.Mongo.Repositories;

public class MongoMatchRepository : IRepository<IMatch>, IDisposable
{
    private MongoClient _mongoClient;
    private IMongoDatabase _database;
    private List<MongoMatch> _allMatches;
    private List<ITeam> _allTeams;

    public MongoMatchRepository(string DatabaseName, List<ITeam> AllTeams)
    {
        _allTeams = AllTeams;
        _mongoClient = new MongoClient(ConnectionDetails.ConnectionString);
        _database = _mongoClient.GetDatabase(DatabaseName);
        _allMatches = [];
    }

    public IList<IMatch> GetAll()
    {
        CheckGetAllMatches();

        List<IMatch> rtnVal = [];
        rtnVal.AddRange(_allMatches);
        return rtnVal;
    }
    public async Task<IList<IMatch>> GetAllAsync()
    {
        await CheckGetAllMatchesAsync();

        List<IMatch> rtnVal = [];
        rtnVal.AddRange(_allMatches);
        return rtnVal;
    }

    public IMatch? GetById(dynamic ObjectId)
    {
        try
        {
            CheckGetAllMatches();
            string id = ObjectId.ToString();
            return _allMatches.FirstOrDefault(t => t is MongoMatch mt && mt.Id == id);
        }
        catch
        {
            return null; // Return null if conversion fails
        }
    }
    public IMatch? GetByName(string Name) => null;

    public IList<IMatch> GetFromDaysBack(DateTime CurrentDate, int DaysBack)
    {
        CheckGetAllMatches();
        DateTime earliestDate = CurrentDate.AddDays(-DaysBack);

        // Filter matches that fall within the range [earliestDate, CurrentDate)
        return GetAll()
            .Where(m => m.MatchDateTimeLocal >= earliestDate && m.MatchDateTimeLocal < CurrentDate)
            .Cast<IMatch>()
            .ToList();
    }

    public void Dispose()
    {
        _mongoClient.Dispose();
    }

    private void CheckGetAllMatches()
    {
        if (!_allMatches.Any())
        {
            IMongoCollection<MongoMatch> matchCollection = _database.GetCollection<MongoMatch>("matches");
            _allMatches = matchCollection.Find(_ => true).SortBy(team => team.MatchDate).ToList();

            // Call in data enrichment as soon as we have the info from Mongo 
            _allMatches.ForEach(match => match.FillInData(_allTeams));
        }
    }

    private async Task CheckGetAllMatchesAsync()
    {
        if (!_allMatches.Any())
        {
            IMongoCollection<MongoMatch> matchCollection = _database.GetCollection<MongoMatch>("matches");
            _allMatches = await matchCollection.Find(_ => true).SortBy(team => team.MatchDate).ToListAsync();

            // Call in data enrichment as soon as we have the info from Mongo 
            _allMatches.ForEach(match => match.FillInData(_allTeams));
        }
    }
}
