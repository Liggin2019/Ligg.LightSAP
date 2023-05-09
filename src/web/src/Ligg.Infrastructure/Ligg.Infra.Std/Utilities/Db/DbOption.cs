
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Ligg.Infrastructure.Utilities.DbUtil
{
    public class DbOption
    {

        public DbOption(string databaseType, string connectionString)
        {
            DatabaseType = GetDatabaseType(databaseType);
            ConnectionString = connectionString;
        }

        public DatabaseType DatabaseType { get; private set; }
        public string ConnectionString { get; private set; }

        private DatabaseType GetDatabaseType(string dbType = "")
        {
            if (dbType.ToLower() == DatabaseType.SqlServer.ToString().ToLower()) return DatabaseType.SqlServer;
            if (dbType.ToLower() == DatabaseType.MySql.ToString().ToLower()) return DatabaseType.MySql;
            if (dbType.ToLower() == DatabaseType.Oracle.ToString().ToLower()) return DatabaseType.Oracle;
            return DatabaseType.Sqlite;
        }


    }

    public enum DatabaseType
    {
        Sqlite = 1,
        MySql = 2,
        SqlServer = 3,
        Oracle = 4,
    }

    public enum DbHandlerType
    {
        FreeSql = 1,
        SugerSql = 2,
        LiggSql = 3,
    }


}
