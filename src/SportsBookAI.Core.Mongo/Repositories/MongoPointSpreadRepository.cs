using MongoDB.Driver;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo.Base;

namespace SportsBookAI.Core.Mongo.Repositories;

public class MongoPointSpreadRepository : IRepository<IPointSpread>, IDisposable
{
    private MongoClient _mongoClient;
    private IMongoDatabase _database;
    private List<MongoPointSpread> _allPointSpreads;
    private List<IMatch> _allMatches;

    public MongoPointSpreadRepository(string DatabaseName, List<IMatch> AllMatches)
    {
        _allMatches = AllMatches;
        _mongoClient = new MongoClient(ConnectionDetails.ConnectionString);
        _database = _mongoClient.GetDatabase(DatabaseName);
        _allPointSpreads = [];
    }

    public IList<IPointSpread> GetAll()
    {
        CheckGetAllRecords();

        List<IPointSpread> rtnVal = [];
        rtnVal.AddRange(_allPointSpreads);
        return rtnVal;
    }
    public async Task<IList<IPointSpread>> GetAllAsync()
    {
        await CheckGetAllRecordsAsync();

        List<IPointSpread> rtnVal = [];
        rtnVal.AddRange(_allPointSpreads);
        return rtnVal;
    }

    public IPointSpread? GetById(dynamic ObjectId)
    {
        try
        {
            CheckGetAllRecords();
            string id = ObjectId.ToString();
            return _allPointSpreads.FirstOrDefault(t => t is MongoPointSpread mt && mt.Id == id);
        }
        catch
        {
            return null; // Return null if conversion fails
        }
    }

    public IPointSpread? GetByName(string Name) => null;

    public IList<IPointSpread> GetFromDaysBack(DateTime CurrentDate, int DaysBack)
    {
        CheckGetAllRecords();
        DateTime earliestDate = CurrentDate.AddDays(-DaysBack);

        // Filter matches that fall within the range [earliestDate, CurrentDate)
        return GetAll()
            .Where(m => m.Match.MatchDateTimeLocal >= earliestDate && m.Match.MatchDateTimeLocal < CurrentDate)
            .Cast<IPointSpread>()
            .ToList();
    }

    private void CheckGetAllRecords()
    {
        if (!_allPointSpreads.Any())
        {
            IMongoCollection<MongoPointSpread> collection = _database.GetCollection<MongoPointSpread>("pointspread");
            _allPointSpreads = collection.Find(_ => true).ToList();

            // Call in data enrichment as soon as we have the info from Mongo 
            _allPointSpreads.ForEach(match => match.FillInData(_allMatches));
            _allPointSpreads = _allPointSpreads.OrderBy(d => d.Match.MatchDateTimeUTC).ToList();
        }
    }

    private async Task CheckGetAllRecordsAsync()
    {
        if (!_allPointSpreads.Any())
        {
            IMongoCollection<MongoPointSpread> collection = _database.GetCollection<MongoPointSpread>("pointspread");
            _allPointSpreads = await collection.Find(_ => true).ToListAsync();

            // Call in data enrichment as soon as we have the info from Mongo 
            _allPointSpreads.ForEach(match => match.FillInData(_allMatches));
            _allPointSpreads = _allPointSpreads.OrderBy(d => d.Match.MatchDateTimeUTC).ToList();
        }
    }

    public void Dispose()
    {
        _mongoClient.Dispose();
    }
}
