namespace SportsBookAI.Core.Interfaces;

public interface IRepository<T>
{
    IList<T> GetAll();
    T? GetById(dynamic ObjectId);
    T? GetByName(string Name);
    IList<T> GetFromDaysBack(DateTime CurrentDate, int DaysBack);
}