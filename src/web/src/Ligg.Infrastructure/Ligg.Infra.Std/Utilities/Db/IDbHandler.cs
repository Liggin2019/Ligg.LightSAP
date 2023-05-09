using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;


namespace Ligg.Infrastructure.Utilities.DbUtil
{
    public  interface IDbHandler
    {
        void ExecuteTransaction(Action action);

        //*list
        List<T> FindMany<T>(Expression<Func<T, bool>> exp) where T : class;
        List<T> FindMany<T>(string script) where T : class;

       List<T> FindMany<T, TMember>(int pageindex, int pagesize, bool Desc, Expression<Func<T, bool>> exp = null, Expression<Func<T, TMember>> exp1 = null) where T : class;

        //*get
        T Get<T>(Expression<Func<T, bool>> exp) where T : class;
        T GetMaxOne<T, TMember>(Expression<Func<T, bool>> exp = null, Expression<Func<T, TMember>> exp1 = null) where T : class;
        T GetMinOne<T, TMember>(Expression<Func<T, bool>> exp = null, Expression<Func<T, TMember>> exp1 = null) where T : class;


        //*create
        int CreateMany<T>(T[] entities) where T : class;
        int Create<T>(T entity) where T : class;

        //*update
        int Update<T>(T entity) where T : class;
        int Update<T>(T entity, Expression<Func<T, bool>> exp) where T : class;

        int Remove<T>(T entity) where T : class;

        //*remove
        int RemoveMany<T>(IEnumerable<T> entities) where T : class;
        int RemoveMany<T>(Expression<Func<T, bool>> exp) where T : class;

        //*other
        bool Any<T>(Expression<Func<T, bool>> exp) where T : class;
        int Count<T>(Expression<Func<T, bool>> exp = null) where T : class;

        //*sql
        DataTable ExecuteDataTable(string sql);
        int ExecuteNonQuery(string sql);


    }
}

