namespace SportsBookAI.Core.Interfaces;

public interface ISportsBookRepository
{
    IRepository<ITeam> TeamRepository { get; }
    IRepository<IMatch> MatchRepository { get; }
}