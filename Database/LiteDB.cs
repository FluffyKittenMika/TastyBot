using Interfaces.Contracts.Database;
using System;
using LiteDB;
using Discord;
using System.IO;

namespace Database
{
    public class LiteDB : ILiteDB
    {
        //TODO make this configurable in config.json
        private readonly string connectionString = AppContext.BaseDirectory + "Databases/LiteDB.db";

        public LiteDatabase GetDatabase()
        {
            Directory.CreateDirectory(AppContext.BaseDirectory + "Databases");
            return new LiteDatabase(connectionString);
        }

        public ILiteCollection<T> GetColumnByName<T>(string name)
        {
            var liteDB = GetDatabase();
            return liteDB.GetCollection<T>(name);
        }
    }
}
