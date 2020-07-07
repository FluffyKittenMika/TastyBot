using Interfaces.Contracts.Database;
using System;
using LiteDB;

namespace Database
{
    public class LiteDB : ILiteDB
    {
        private readonly string connectionString = AppContext.BaseDirectory + "Databases/LiteDB.db";

        public LiteDatabase GetDatabase()
        {
            return new LiteDatabase(connectionString);
        }

        public ILiteCollection<T> GetColumnByName<T>(string name)
        {
            var liteDB = GetDatabase();
            return liteDB.GetCollection<T>(name);
        }
    }
}
