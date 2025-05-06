using Newtonsoft.Json;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class UflTeamRepository : IRepository<ITeam>
{
    private List<MockTeam> _allUflTeams;

    public UflTeamRepository() 
    {
        _allUflTeams = [];
        string teamsString = @"
        [
            {
                ""TeamName"": ""Arlington Renegades"",
                ""Conference"": ""XFL"",
                ""Division"": null
            },
            {
                ""TeamName"": ""Birmingham Stallions"",
                ""Conference"": ""USFL"",
                ""Division"": null
            },
            {
                ""TeamName"": ""DC Defenders"",
                ""Conference"": ""XFL"",
                ""Division"": null
            },
            {
                ""TeamName"": ""Houston Roughnecks"",
                ""Conference"": ""USFL"",
                ""Division"": null
            },
            {
                ""TeamName"": ""Memphis Showboats"",
                ""Conference"": ""USFL"",
                ""Division"": null
            },
            {
                ""TeamName"": ""Michigan Panthers"",
                ""Conference"": ""USFL"",
                ""Division"": null
            },
            {
                ""TeamName"": ""San Antonio Brahmas"",
                ""Conference"": ""XFL"",
                ""Division"": null
            },
            {
                ""TeamName"": ""St. Louis Battlehawks"",
                ""Conference"": ""XFL"",
                ""Division"": null
            }
        ]";
        List<MockTeam>? teams = JsonConvert.DeserializeObject<List<MockTeam>>(teamsString);

        if (teams != null && teams.Count > 0)
        {
            _allUflTeams.AddRange(teams);
        }
    }

    public IList<ITeam> GetAll()
    {
        List<ITeam> rtnVal = [];
        rtnVal.AddRange(_allUflTeams);
        return rtnVal;
    }
    public Task<IList<ITeam>> GetAllAsync() => Task.FromResult(GetAll());
    public ITeam? GetById(dynamic ObjectId) => null;
    public ITeam? GetByName(string Name) => _allUflTeams.FirstOrDefault(t => t.TeamName == Name);

    /// <summary>Not used in mock implementation. Not relevant for teams</summary>
    public IList<ITeam> GetFromDaysBack(DateTime CurrentDate, int DaysBack) => throw new NotImplementedException("MockTeamRepository class does not use or need this method");
}