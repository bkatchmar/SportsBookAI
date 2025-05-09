using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Mongo.Base;

public class MongoPointSpread : IPointSpread
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("MatchId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string MatchId { get; set; } = null!;

    [BsonElement("Spread")]
    [BsonRepresentation(BsonType.Double)]
    public double Spread { get; set; }

    [BsonElement("Result")]
    [BsonRepresentation(BsonType.String)]
    public string Result { get; set; } = null!;

    [BsonIgnore]
    public IMatch Match { get; set; } = null!;

    [BsonElement("FavoredTeamId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string FavoredTeamId { get; set; } = null!;

    [BsonIgnore]
    public ITeam FavoredTeam { get; set; } = null!;

    public void FillInData(IList<IMatch> AllMatches)
    {
        // Fill in match data
        List<MongoMatch> mongoMacthes = AllMatches.OfType<MongoMatch>().ToList();
        Match = mongoMacthes.First(t => t.Id == MatchId);

        // Fill in team data, specifically, the favored team
        HashSet<MongoTeam> allTeams = AllMatches.SelectMany(m => new[] { m.HomeTeam, m.AwayTeam }).OfType<MongoTeam>().ToHashSet();
        FavoredTeam = allTeams.First(t => t.Id == FavoredTeamId);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (obj is MongoPointSpread otherSpread)
        {
            return MatchId == otherSpread.MatchId;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(MatchId);
    }
}