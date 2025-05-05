namespace SportsBookAI.Core.Interfaces;

public interface IRepository<T>
{
    IList<T> GetAll();
    Task<IList<ITeam>> GetAllAsync();
    T? GetById(dynamic ObjectId);
    T? GetByName(string Name);
    IList<T> GetFromDaysBack(DateTime CurrentDate, int DaysBack);
}