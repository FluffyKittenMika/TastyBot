using LiteDB;

namespace Interfaces.Contracts.Database
{
    public interface ILiteDB
    {
        LiteDatabase GetDatabase();
        ILiteCollection<T> GetColumnByName<T>(string name);
    }
}