using LiteDB;

namespace Interfaces.Contracts.Database
{
    public interface ILiteDB
    {
        ILiteCollection<T> GetColumnByName<T>(string name);
    }
}