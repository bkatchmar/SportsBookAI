using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Mongo.Base;

public class MongoTeam : ITeam
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("TeamName")]
    public string TeamName { get; set; } = string.Empty;
    [BsonElement("Conference")]
    public string? Conference { get; set; } = null!;

    public string? Division { get; set; } = null;

    public override string ToString() => TeamName;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (obj is MongoTeam otherTeam)
        {
            return Id == otherTeam.Id && TeamName == otherTeam.TeamName;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, TeamName);
    }
}
