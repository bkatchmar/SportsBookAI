using Newtonsoft.Json;
using SportsBookAI.Core.Interfaces;
using SportsBookAI.Core.Tests.TestImplementations.CoreClasses;

namespace SportsBookAI.Core.Tests.TestImplementations.Repositories;

public class UflExampleTeamRepo : IRepository<ITeam>
{
    private readonly List<MockTeam> _uflTeams;

    public UflExampleTeamRepo()
    {
        string json = @"[
        {""TeamName"":""Arlington Renegades"",""Conference"":""XFL"",""Division"":null},
        {""TeamName"":""Birmingham Stallions"",""Conference"":""USFL"",""Division"":null},
        {""TeamName"":""DC Defenders"",""Conference"":""XFL"",""Division"":null},
        {""TeamName"":""Houston Roughnecks"",""Conference"":""USFL"",""Division"":null},
        {""TeamName"":""Memphis Showboats"",""Conference"":""USFL"",""Division"":null},
        {""TeamName"":""Michigan Panthers"",""Conference"":""USFL"",""Division"":null},
        {""TeamName"":""San Antonio Brahmas"",""Conference"":""XFL"",""Division"":null},
        {""TeamName"":""St. Louis Battlehawks"",""Conference"":""XFL"",""Division"":null}
        ]";
        List<MockTeam>? fromJson = JsonConvert.DeserializeObject<List<MockTeam>>(json);
        _uflTeams = [];
        if (fromJson != null)
        {
            _uflTeams.AddRange(fromJson);
        }
    }

    public IList<ITeam> GetAll()
    {
        List<ITeam> rtnVal = [];
        rtnVal.AddRange(_uflTeams);
        return rtnVal;
    }
    public Task<IList<ITeam>> GetAllAsync()
    {
        List<ITeam> rtnVal = [];
        rtnVal.AddRange(_uflTeams);
        return Task.FromResult((IList<ITeam>)rtnVal);
    }

    public ITeam? GetById(dynamic ObjectId) => null;
    public ITeam? GetByName(string Name) => _uflTeams.FirstOrDefault(t => t.TeamName == Name);

    /// <summary>Not used in mock implementation. Not relevant for teams</summary>
    public IList<ITeam> GetFromDaysBack(DateTime CurrentDate, int DaysBack) => throw new NotImplementedException("MockTeamRepository class does not use or need this method");
}
