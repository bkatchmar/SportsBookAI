using MongoDB.Driver;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Mongo.Base;

namespace SportsBookAI.Core.Mongo.Repositories;

public class MongoOverUnderRepository : IRepository<IOverUnder>, IDisposable
{
    private MongoClient _mongoClient;
    private IMongoDatabase _database;
    private List<MongoOverUnder> _allOverUnderMarks;
    private List<IMatch> _allMatches;

    public MongoOverUnderRepository(string DatabaseName, List<IMatch> AllMatches)
    {
        _allMatches = AllMatches;
        _mongoClient = new MongoClient(ConnectionDetails.ConnectionString);
        _database = _mongoClient.GetDatabase(DatabaseName);
        _allOverUnderMarks = [];
    }

    public IList<IOverUnder> GetAll()
    {
        CheckGetAllRecords();
        
        List<IOverUnder> rtnVal = [];
        rtnVal.AddRange(_allOverUnderMarks);
        return rtnVal;
    }
    public async Task<IList<IOverUnder>> GetAllAsync()
    {
        await CheckGetAllRecordsAsync();

        List<IOverUnder> rtnVal = [];
        rtnVal.AddRange(_allOverUnderMarks);
        return rtnVal;
    }

    public IOverUnder? GetById(dynamic ObjectId) 
    {
        try
        {
            CheckGetAllRecords();
            string id = ObjectId.ToString();
            return _allOverUnderMarks.FirstOrDefault(t => t is MongoOverUnder mt && mt.Id == id);
        }
        catch
        {
            return null; // Return null if conversion fails
        }
    }

    public IOverUnder? GetByName(string Name) => null;
    
    public IList<IOverUnder> GetFromDaysBack(DateTime CurrentDate, int DaysBack)
    {
        CheckGetAllRecords();
        DateTime earliestDate = CurrentDate.AddDays(-DaysBack);
        
        // Filter matches that fall within the range [earliestDate, CurrentDate)
        return GetAll()
            .Where(m => m.Match.MatchDateTimeLocal >= earliestDate && m.Match.MatchDateTimeLocal < CurrentDate)
            .Cast<IOverUnder>()
            .ToList();
    }

    private void CheckGetAllRecords()
    {
        if (!_allOverUnderMarks.Any())
        {
            IMongoCollection<MongoOverUnder> collection = _database.GetCollection<MongoOverUnder>("overunder");
            _allOverUnderMarks = collection.Find(_ => true).ToList();
            
            // Call in data enrichment as soon as we have the info from Mongo 
            _allOverUnderMarks.ForEach(match => match.FillInData(_allMatches));
            _allOverUnderMarks = _allOverUnderMarks.OrderBy(d => d.Match.MatchDateTimeUTC).ToList();
        }
    }

    private async Task CheckGetAllRecordsAsync()
    {
        if (!_allOverUnderMarks.Any())
        {
            IMongoCollection<MongoOverUnder> collection = _database.GetCollection<MongoOverUnder>("overunder");
            _allOverUnderMarks = await collection.Find(_ => true).ToListAsync();

            // Call in data enrichment as soon as we have the info from Mongo 
            _allOverUnderMarks.ForEach(match => match.FillInData(_allMatches));
            _allOverUnderMarks = _allOverUnderMarks.OrderBy(d => d.Match.MatchDateTimeUTC).ToList();
        }
    }

    public void Dispose()
    {
        _mongoClient.Dispose();
    }
}
