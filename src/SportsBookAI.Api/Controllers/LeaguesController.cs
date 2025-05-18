using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SportsBookAI.Core.Mongo;

namespace SportsBookAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly string[] _leagues;
    private readonly string? _mongoDbConnectionString;

    public LeaguesController(IOptions<LeaguesWithDataSetting> options, IConfiguration configuration)
    {
        _leagues = options.Value.LeaguesWithData;
        _mongoDbConnectionString = configuration.GetConnectionString("MongoDb");
        if (!string.IsNullOrEmpty(_mongoDbConnectionString))
        {
            ConnectionDetails.ConnectionString = _mongoDbConnectionString;
        }
    }

    [HttpGet]
    public IActionResult GetLeagues() => Ok(_leagues);
}
