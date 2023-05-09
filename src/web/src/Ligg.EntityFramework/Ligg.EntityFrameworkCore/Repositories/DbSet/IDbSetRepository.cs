
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ligg.EntityFrameworkCore
{
    public interface IDbSetRepository<TKey, T, TDbContext> where T : class where TDbContext : DbContext
    {
        //*get
        void SetCurrentActorId(string id);
        T Get(Expression<Func<T, bool>> exp = null);
        Task<T> GetAsync(Expression<Func<T, bool>> exp);
        IQueryable<T> GetMany(Expression<Func<T, bool>> exp = null);
        Task<List<T>> FindManyAsync(Expression<Func<T, bool>> exp = null);
        IQueryable<T> GetMany(int pageindex = 1, int pagesize = 10, string orderby = "", Expression<Func<T, bool>> exp = null);
        Task<List<T>> FindManyAsync(int pageindex, int pagesize, string orderby = "", Expression<Func<T, bool>> exp = null);


        //*add
        void Create(T entity);
        Task<int> CreateAsync(T entity);
        void CreateMany(T[] entities);
        Task<int> CreateManyAsync(T[] entities);

        //*mod
        void Update(T entity);
        Task<int> UpdateAsync(T entity);
        //局部修改
        void Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity);
        Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity);

        //*del
        void Remove(T entity);
        void RemoveMany(Expression<Func<T, bool>> exp);
        Task<int> RemoveAsync(T entity);
        Task<int> RemoveManyAsync(Expression<Func<T, bool>> exp);


        //*judge
        bool Any(Expression<Func<T, bool>> exp);
        Task<bool> AnyAsync(Expression<Func<T, bool>> exp);

        //*others
        int Count(Expression<Func<T, bool>> exp = null);
        Task<int> CountAsync(Expression<Func<T, bool>> exp = null);

        int ExecuteDbSqlRaw(string sql);
        Task<int> ExecuteDbSqlRawAsync(string sql);

        IQueryable<T> FromSetSqlRaw(string sql, params object[] parameters);
        //IQueryable<T> FromQuerySqlRaw(string sql, params object[] parameters);
        

    }
}