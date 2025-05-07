using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SportsBookAI.Core.Interfaces;

namespace SportsBookAI.Core.Mongo.Base;

public class MongoMatch : IMatch
{
    public MongoMatch(IList<ITeam> AllTeams)
    {
        FillInData(AllTeams);
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("HomeTeam")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string HomeTeamId { get; set; } = null!;

    [BsonIgnore]
    public ITeam HomeTeam { get; set; } = null!;

    [BsonElement("AwayTeam")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AwayTeamId { get; set; } = null!;

    [BsonIgnore]
    public ITeam AwayTeam { get; set; } = null!;

    [BsonElement("DateTime")]
    public DateTime MatchDate { get; set; }

    [BsonIgnore]
    public DateTime MatchDateTimeUTC
    {
        get => MatchDate;
        set
        {
            MatchDate = value;

            TimeZoneInfo newYorkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            MatchDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(MatchDate, newYorkTimeZone);
        }
    }

    [BsonIgnore]
    public DateTime MatchDateTimeLocal { get; set; }

    [BsonElement("WeekNumber")]
    public int? WeekNumber { get; set; }

    public void FillInData(IList<ITeam> AllTeams)
    {
        TimeZoneInfo newYorkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        List<MongoTeam> mongoTeams = AllTeams.OfType<MongoTeam>().ToList();

        HomeTeam = mongoTeams.First(t => t.Id == HomeTeamId);
        AwayTeam = mongoTeams.First(t => t.Id == AwayTeamId);
        MatchDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(MatchDate, newYorkTimeZone);
    }

    public override string ToString()
    {
        return $"Match Date: {MatchDateTimeLocal:dddd, MMMM dd, yyyy HH:mm:ss tt}, Week {WeekNumber}, {HomeTeam?.ToString()} vs {AwayTeam?.ToString()}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (obj is MongoMatch otherMatch)
        {
            return Id == otherMatch.Id && HomeTeamId == otherMatch.HomeTeamId && AwayTeamId == otherMatch.AwayTeamId;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, HomeTeamId, AwayTeamId);
    }
}
