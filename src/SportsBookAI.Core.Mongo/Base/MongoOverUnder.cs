using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Mongo.Base;

public class MongoOverUnder : IOverUnder
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("MatchId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string MatchId { get; set; } = null!;

    [BsonElement("Mark")]
    [BsonRepresentation(BsonType.Double)]
    public double Mark { get; set; }

    [BsonElement("Hit")]
    [BsonRepresentation(BsonType.String)]
    public string Hit { get; set; } = null!;

    [BsonIgnore]
    public IMatch Match { get; set; } = null!;

    public void FillInData(IList<IMatch> AllMatches)
    {
        List<MongoMatch> mongoTeams = AllMatches.OfType<MongoMatch>().ToList();
        Match = mongoTeams.First(t => t.Id == MatchId);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (obj is MongoOverUnder overMarking)
        {
            return MatchId == overMarking.MatchId && Id == overMarking.Id;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, MatchId);
    }
}