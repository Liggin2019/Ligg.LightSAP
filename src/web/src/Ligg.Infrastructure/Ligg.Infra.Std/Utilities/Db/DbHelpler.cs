
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.Extensions;
using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ligg.Infrastructure.Utilities.DbUtil
{
    public class DbHelper
    {
        public static DbHandlerFactory DbHandler;

        public static void ExecuteTransaction(Action action)
        {
            DbHandler.Handler.ExecuteTransaction(action);

        }

        //*list
        public static List<T> FindMany<T>(string script)
        {
            var dt = DbHandler.Handler.ExecuteDataTable(script);
            var lst = dt.ConvertToList<T>().ToList();
            return lst;
        }


        //*get
        public static T Get<T>(string script)
        {
            var lst = FindMany<T>(script);

            return lst.FirstOrDefault();
        }
        public static T FindMaxOne<T, TMember>(string script, Expression<Func<T, TMember>> exp1 = null) where T : class
        {
            var lst = FindMany<T>(script).AsQueryable();
            var lst1 = lst.OrderByDescending(exp1).ToList();
            return lst1.FirstOrDefault();
        }
        public static T FindMinOne<T, TMember>(string script, Expression<Func<T, TMember>> exp1 = null) where T : class
        {
            var lst = FindMany<T>(script).AsQueryable();
            var lst1 = lst.OrderBy(exp1).ToList();
            return lst1.FirstOrDefault();
        }

        //*create
        public static int CreateMany<T>(T[] entities) where T : class
        {
            return DbHandler.Handler.CreateMany(entities);
        }
        public static int Create<T>(T entity) where T : class
        {
            return DbHandler.Handler.Create(entity);
        }

        //*update
        public static int Update<T>(T entity) where T : class
        {
            return DbHandler.Handler.Update(entity);
        }
        public static int Update<T>(T entity, Expression<Func<T, bool>> exp) where T : class
        {
            return DbHandler.Handler.Update(entity, exp);
        }

        //*remove
        public static int RemoveMany<T>(IEnumerable<T> entities) where T : class
        {
            return DbHandler.Handler.RemoveMany(entities);
        }
        public static int RemoveMany<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return DbHandler.Handler.RemoveMany(exp);
        }
        public static int Remove<T>(T entity) where T : class
        {
            return DbHandler.Handler.Remove(entity);
        }


        //*other
        public static bool Any<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return DbHandler.Handler.Any(exp);
        }
        public static int Count<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return DbHandler.Handler.Count(exp);
        }


        //*sql script
        public static DataTable ExecuteDataTable(string sql)
        {
            return DbHandler.Handler.ExecuteDataTable(sql);
        }

        public static int ExecuteNonQuery(string sql)
        {
            return DbHandler.Handler.ExecuteNonQuery(sql);
        }


    }
}

