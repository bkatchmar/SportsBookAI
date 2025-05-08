namespace SportsBookAI.Core.Interfaces;

public interface ISportsBookRepository
{
    IRepository<ITeam> TeamRepository { get; }
    IRepository<IMatch> MatchRepository { get; }
    IRepository<IOverUnder> OverUnderRepository { get; }
    IRepository<IPointSpread> PointSpreadRepository { get; }
}