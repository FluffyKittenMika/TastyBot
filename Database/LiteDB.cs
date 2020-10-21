using Interfaces.Contracts.Database;
using LiteDB;
using System;
using System.IO;
using System.Configuration;
using System.Data.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Databases
{
    public class LiteDB : ILiteDB
    {
        private readonly string connectionString = $"Filename={AppContext.BaseDirectory}Databases/LiteDB.db;Password=QDgDAKIAEReA9EgYG102TJ1eQO0";

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

//Some failed attempts

/*
 public class LiteDB : ILiteDB
    {

        //private readonly string connection = $"Filename={AppContext.BaseDirectory}Databases/LiteDB.db;Password=QDgDAKIAEReA9EgYG102TJ1eQO0;connection=shared";
        public LiteDatabase liteDBConnection;
        private ConnectionString connectionString = new ConnectionString()
        {
            Password = "QDgDAKIAEReA9EgYG102TJ1eQO0",
            Filename = @$"{AppContext.BaseDirectory}Databases/LiteDB.db",
            Connection = ConnectionType.Shared,
        };


        public LiteDatabase GetDatabase()
        {
            Directory.CreateDirectory(AppContext.BaseDirectory + "Databases");
            return new LiteDatabase(connectionString);
        }

        public ILiteCollection<T> GetColumnByName<T>(string name)
        {
            while (liteDBConnection != null)
            {
                
                Task.Delay(30);
            }
            liteDBConnection = GetDatabase();
            return liteDBConnection.GetCollection<T>(name);
            
        }

        public void DisposeTheDbConnection()
        {
            if (liteDBConnection != null)
            {
                liteDBConnection.Dispose();
                liteDBConnection = null;

            }
        }
    }
 */

/*
 public class LiteDB : ILiteDB
    {

        //private readonly string connection = $"Filename={AppContext.BaseDirectory}Databases/LiteDB.db;Password=QDgDAKIAEReA9EgYG102TJ1eQO0;connection=shared";
        public LiteDatabase liteDBConnection;
        private ConnectionString connectionString = new ConnectionString()
        {
            Password = "QDgDAKIAEReA9EgYG102TJ1eQO0",
            Filename = @$"{AppContext.BaseDirectory}Databases/LiteDB.db",
            Connection = ConnectionType.Shared,
        };

        public LiteDB()
        {

        }

        public LiteDatabase GetDatabase()
        {
            Directory.CreateDirectory(AppContext.BaseDirectory + "Databases");
            return new LiteDatabase(connectionString);
        }

        public ILiteCollection<T> GetColumnByName<T>(string name)
        {
            
            try
            {
                liteDBConnection = GetDatabase();
                return liteDBConnection.GetCollection<T>(name);
            }
            finally
            {
                if (liteDBConnection != null)
                {
                    ((IDisposable)liteDBConnection).Dispose();
                }
            }
        }
    }
 */

/*
 public class LiteDB : ILiteDB
    {

        //private readonly string connection = $"Filename={AppContext.BaseDirectory}Databases/LiteDB.db;Password=QDgDAKIAEReA9EgYG102TJ1eQO0;connection=shared";
        private ConnectionString connectionString = new ConnectionString()
        {
            Password = "QDgDAKIAEReA9EgYG102TJ1eQO0",
            Filename = @$"{AppContext.BaseDirectory}Databases/LiteDB.db",
            Connection = ConnectionType.Shared,
        };

        public LiteDB()
        {
            DbAccessQueue = new Queue();
            liteDb = null;
        }

        public LiteDatabase GetDatabase()
        {
            Directory.CreateDirectory(AppContext.BaseDirectory + "Databases");
            return new LiteDatabase(connectionString);
        }

        public ILiteCollection<T> GetColumnByName<T>(string name)
        {
            var db = GetDatabase();
            GC.KeepAlive(db);
            try
            {
                return db.GetCollection<T>(name);
            }
            finally
            {
                if (db != null)
                {
                    ((IDisposable)db).Dispose();
                }
            }
        }
    }
 */

/*
 public class LiteDB : ILiteDB
    {
        LiteDB liteDb;
        Queue DbAccessQueue;
        //private readonly string connection = $"Filename={AppContext.BaseDirectory}Databases/LiteDB.db;Password=QDgDAKIAEReA9EgYG102TJ1eQO0;connection=shared";
        private ConnectionString connectionString = new ConnectionString()
        {
            Password = "QDgDAKIAEReA9EgYG102TJ1eQO0",
            Filename = @$"{AppContext.BaseDirectory}Databases/LiteDB.db",
            Connection = ConnectionType.Shared,
        };

        public LiteDB()
        {
            DbAccessQueue = new Queue();
            liteDb = null;
        }

        public LiteDatabase GetDatabase()
        {
            Directory.CreateDirectory(AppContext.BaseDirectory + "Databases");
            return new LiteDatabase(connectionString);
        }

        public ILiteCollection<T> GetColumnByName<T>(string name)
        {
            var db = GetDatabase();
            try
            {

                return db.GetCollection<T>(name);
            }
            finally
            {
                Task.Delay(1000);
                if (db != null)
                {
                    ((IDisposable)db).Dispose();
                }
            }
        }
    }
 */

/*
public class LiteDB : ILiteDB
{
    LiteDatabase liteDb;
    Queue DbAccessQueue;
    //private readonly string connection = $"Filename={AppContext.BaseDirectory}Databases/LiteDB.db;Password=QDgDAKIAEReA9EgYG102TJ1eQO0;connection=shared";
    private ConnectionString connectionString = new ConnectionString()
    {
        Password = "QDgDAKIAEReA9EgYG102TJ1eQO0",
        Filename = @$"{AppContext.BaseDirectory}Databases/LiteDB.db",
        Connection = ConnectionType.Shared,
    };

    public LiteDB()
    {
        DbAccessQueue = new Queue();
    }

    public LiteDatabase GetDatabase()
    {
        Directory.CreateDirectory(AppContext.BaseDirectory + "Databases");
        return new LiteDatabase(connectionString);
    }

    public ILiteCollection<T> GetColumnByName<T>(string name)
    {
        DbAccessQueue.Enqueue(name);
        while (true)
        {
            if (liteDb == null)
            {
                using var liteDb = GetDatabase();
                return liteDb.GetCollection<T>(DbAccessQueue.Dequeue().ToString());
            }

        }

    }
}
*/

/*
public class LiteDB : ILiteDB
{
    //private readonly string connection = $"Filename={AppContext.BaseDirectory}Databases/LiteDB.db;Password=QDgDAKIAEReA9EgYG102TJ1eQO0;connection=shared";
    private ConnectionString connectionString = new ConnectionString()
    {
        Password = "QDgDAKIAEReA9EgYG102TJ1eQO0",
        Filename = @$"{AppContext.BaseDirectory}Databases/LiteDB.db",
        Connection = ConnectionType.Direct,
    };

    public LiteDB()
    {
    }

    public LiteDatabase GetDatabase()
    {
        Directory.CreateDirectory(AppContext.BaseDirectory + "Databases");
        return new LiteDatabase(connectionString);
    }

    public ILiteCollection<T> GetColumnByName<T>(string name)
    {
        var db = GetDatabase();
        try
        {

            return db.GetCollection<T>(name);
        }
        finally
        {
            if (db != null)
            {
                ((IDisposable)db).Dispose();
            }
        }
    }
}
*/

/*
 public class LiteDB : ILiteDB
    {
        private Queue DbAccessQueue;
        //private readonly string connection = $"Filename={AppContext.BaseDirectory}Databases/LiteDB.db;Password=QDgDAKIAEReA9EgYG102TJ1eQO0;connection=shared";
        private ConnectionString connectionString = new ConnectionString()
        {
            Password = "QDgDAKIAEReA9EgYG102TJ1eQO0",
            Filename = @$"{AppContext.BaseDirectory}Databases/LiteDB.db",
            Connection = ConnectionType.Direct,
        };

        public LiteDB()
        {
            DbAccessQueue = new Queue();
        }

        public LiteDatabase GetDatabase()
        {
            Directory.CreateDirectory(AppContext.BaseDirectory + "Databases");
            return new LiteDatabase(connectionString);
        }

        public ILiteCollection<T> GetColumnByName<T>(string name)
        {
            DbAccessQueue.Enqueue(name);
            while (true)
            {
                if (DbAccessQueue == null)
                {
                    using var db = GetDatabase();
                    return db.GetCollection<T>(DbAccessQueue.Dequeue().ToString());
                }
                
            }
            
        }
    }
 */

/*
  Backup code. :D
public class LiteDB : ILiteDB
    {
        //private readonly string connection = $"Filename={AppContext.BaseDirectory}Databases/LiteDB.db;Password=QDgDAKIAEReA9EgYG102TJ1eQO0;connection=shared";
        private ConnectionString connectionString = new ConnectionString()
        {
            Password = "QDgDAKIAEReA9EgYG102TJ1eQO0",
            Filename = @$"{AppContext.BaseDirectory}Databases/LiteDB.db",
            Connection = ConnectionType.Shared,
        };

        public LiteDatabase GetDatabase()
        {
            Directory.CreateDirectory(AppContext.BaseDirectory + "Databases");
            return new LiteDatabase(connectionString);
            
        }

        public ILiteCollection<T> GetColumnByName<T>(string name)
        {
            using var db = GetDatabase();
            return db.GetCollection<T>(name);
        }
    }
*/
