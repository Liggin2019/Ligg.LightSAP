using FreeSql;
using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ligg.Infrastructure.Utilities.DbUtil
{
    public class FreeSqlHandler : IDbHandler
    {
        private static IFreeSql _instance = null;
        public FreeSqlHandler(DbOption dbOption )
        {
            CreateFreeSqlInstance(dbOption);
        }

        //单线程中，可以使用默认的事务，缺点：不支持异步
        public void ExecuteTransaction(Action action)
        {
            _instance.Transaction(IsolationLevel.Serializable, () =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

        }

        //*list
        //###所有FindMany，因Select不靠谱，慎用 version: 3.2.301###########

        public List<T> FindMany<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _instance.Select<T>(exp).ToList();
        }

        public List<T> FindMany<T>(string script) where T : class
        {
            return _instance.Select<T>().Where(script).ToList();
        }
        public List<T> FindMany<T, TMember>(int pageindex, int pagesize, bool Desc, Expression<Func<T, bool>> exp = null, Expression<Func<T, TMember>> exp1 = null) where T : class
        {
            if (pageindex < 1) pageindex = 1;
            var lst = new List<T>();
            if (Desc) lst = _instance.Select<T>(exp).OrderByDescending(exp1).ToList();
            else lst = _instance.Select<T>(exp).OrderBy(exp1).ToList();

            var lst1 = lst.Skip(pagesize * (pageindex - 1)).Take(pagesize);
            return lst1.ToList();
        }

        //*get
        //###所有Get，因Select不靠谱，慎用###########
        public T Get<T>(Expression<Func<T, bool>> exp) where T : class
        {
            var entities = _instance.Select<T>(exp); 
            return entities.First(); 
        }
        public T GetMaxOne<T, TMember>(Expression<Func<T, bool>> exp = null, Expression<Func<T, TMember>> exp1 = null) where T : class
        {
            var lst = _instance.Select<T>(exp).OrderByDescending(exp1).ToList();
            if (lst.Count > 0)
                return lst.First();
            return null;
        }
        public T GetMinOne<T, TMember>(Expression<Func<T, bool>> exp = null, Expression<Func<T, TMember>> exp1 = null) where T : class
        {
            var lst = _instance.Select<T>(exp).OrderBy(exp1).ToList();
            if (lst.Count > 0)
                return lst.First();
            return null;
        }

        //*create
        public int CreateMany<T>(T[] entities) where T : class
        {
            return _instance.Insert(entities).ExecuteAffrows();
        }
        public int Create<T>(T entity) where T : class
        {
            return _instance.Insert(entity).ExecuteAffrows();
        }

        //*update
        public int Update<T>(T entity) where T : class
        {
            return _instance.Update<T>().SetSource(entity).ExecuteAffrows();
        }
        public int Update<T>(T entity, Expression<Func<T, bool>> exp) where T : class
        {
            return _instance.Update<T>().SetDto(entity).Where(exp).ExecuteAffrows();
        }

        //*remove
        public int RemoveMany<T>(IEnumerable<T> entities) where T : class
        {
            return _instance.Delete<T>(entities).ExecuteAffrows();
        }
        public int RemoveMany<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _instance.Delete<T>().Where(exp).ExecuteAffrows();
        }
        public int Remove<T>(T entity) where T : class
        {
            return _instance.Delete<T>().ExecuteAffrows();
        }


        //*other
        public bool Any<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _instance.Select<T>().Any(exp);
        }
        public int Count<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return _instance.Select<T>(exp).ToList().Count;
        }


        //*sql script
        public DataTable ExecuteDataTable(string sql)
        {
            return _instance.Ado.ExecuteDataTable(sql);
        }

        public int ExecuteNonQuery(string sql)
        {
            return _instance.Ado.ExecuteNonQuery(sql);
        }

        //*private
        private static void CreateFreeSqlInstance(DbOption dbOption)
        {
            var freeSqlBuilder = new FreeSqlBuilder();

            var dataType = FreeSql.DataType.Sqlite; 
            if (dbOption.DatabaseType == DatabaseType.SqlServer)
            {
                dataType = FreeSql.DataType.SqlServer;
            }
            else if (dbOption.DatabaseType == DatabaseType.MySql)
            {
                dataType = FreeSql.DataType.MySql;
            }
            else if (dbOption.DatabaseType == DatabaseType.Sqlite)
            {
                dataType = FreeSql.DataType.Oracle;
            }

            _instance=freeSqlBuilder.UseConnectionString(dataType, dbOption.ConnectionString)
            //aop监听sql
            .UseMonitorCommand(cmd =>//执行前
            {
            }, (cmd, valueString) =>//执行后
            {
            })
            .UseAutoSyncStructure(false)//CodeFirst自动同步将实体同步到数据库结构（开发阶段必备），默认是true，正式环境请改为false
            .Build();//创建实例（官方建议使用单例模式）
        }
    }
}
