
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ligg.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Ligg.EntityFrameworkCore
{
    public interface IUnitWork<TDbContext> where TDbContext : DbContext
    {
        void SetCurrentActorId(string id);

        DbContext GetDbContext();
        void ExecuteTransaction(Action action);
        Task ExecuteTransactionAsync(Action action);
        Task<int> SaveAsync();
        void Save();
        //*list
        IQueryable<T> GetMany<T>(Expression<Func<T, bool>> exp = null) where T : class;
        //Task<List<T>> FindManyAsync<T>(Expression<Func<T, bool>> exp = null) where T : class;
        IQueryable<T> GetMany<T>(int pageindex = 1, int pagesize = 10, string orderby = "", Expression<Func<T, bool>> exp = null) where T : class;
        //Task<List<T>> FindManyAsync<T>(int pageindex = 1, int pagesize = 10, string orderby = "", Expression<Func<T, bool>> exp = null) where T : class;

        //# get
        T Get<T>(Expression<Func<T, bool>> exp = null) where T : class;
        //Task<T> GetAsync<T>(Expression<Func<T, bool>> exp) where T : class;


        //#add
        void Create<TKey,T>(T entity) where T : Entity<TKey>;
        //Task CreateAsync<TKey, T>(T entity) where T : Entity<TKey>;
        void CreateMany<TKey, T>(T[] entities) where T : Entity<TKey>;
        //Task CreateManyAsync<TKey, T>(T[] entities) where T : Entity<TKey>;

        //#mod
        void Update<TKey, T>(T entity) where T : Entity<TKey>;
        void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class;

        //# del
        void Remove<T>(T entity) where T : class;
        //Task RemoveAsync<T>(T entity) where T : class; //necessary?
        void RemoveMany<T>(Expression<Func<T, bool>> exp) where T : class;
        //Task RemoveManyAsync<T>(Expression<Func<T, bool>> exp) where T : class;



        //#judge
        bool Any<T>(Expression<Func<T, bool>> exp) where T : class;
        //Task<bool> AnyAsync<T>(Expression<Func<T, bool>> exp) where T : class;

        //#others
        int Count<T>(Expression<Func<T, bool>> exp = null) where T : class;
        //Task<int> CountAsync<T>(Expression<Func<T, bool>> exp = null) where T : class;
        int ExecuteDbSqlRaw(string sql);
        Task<int> ExecuteDbSqlRawAsync(string sql);

        IQueryable<T> FromSetSqlRaw<T>(string sql, params object[] parameters) where T : class;
        //IQueryable<T> FromQuerySqlRaw<T>(string sql, params object[] parameters) where T : class;



    }
}
