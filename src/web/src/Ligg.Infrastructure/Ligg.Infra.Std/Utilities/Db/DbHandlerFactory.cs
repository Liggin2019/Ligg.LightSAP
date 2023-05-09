using Ligg.Infrastructure.Extensions;


using NLog;
using System;


namespace Ligg.Infrastructure.Utilities.DbUtil
{
    public class DbHandlerFactory
    {
        public IDbHandler Handler;
        public DbHandlerFactory(string DbType, string ConnectionString, string dbHandlerType)
        {
            var dbOption = new DbOption(DbType, ConnectionString);

            if (dbHandlerType.ToLower() == DbHandlerType.FreeSql.ToString().ToLower())
            {
                Handler = new FreeSqlHandler(dbOption);
            }
            else if (dbHandlerType.ToLower() == DbHandlerType.SugerSql.ToString().ToLower())
            {
                throw new NotImplementedException();
            }
            else if (dbHandlerType.ToLower() == DbHandlerType.LiggSql.ToString().ToLower())
            {
                throw new NotImplementedException();
            }
            else
            {
                Handler = new FreeSqlHandler(dbOption);
            }

        }



    }



}

