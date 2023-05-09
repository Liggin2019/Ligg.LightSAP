
using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Ligg.EntityFrameworkCore
{
    public class UnitWork<TDbContext> : IUnitWork<TDbContext> where TDbContext : DbContext
    {
        private TDbContext _context;
        public UnitWork(TDbContext context)
        {
            _context = context;
        }

        public DbContext GetDbContext()
        {
            return _context;
        }

        public string _currentActorId;
        public void SetCurrentActorId(string id)
        {
            _currentActorId = id;
        }

        public void ExecuteTransaction(Action action)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    action();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public async Task ExecuteTransactionAsync(Action action)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    action();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        //*list
        public IQueryable<T> GetMany<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp); 
        }

        public async Task<List<T>> FindManyAsync<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return await FilterAsync(exp);
        }

        public IQueryable<T> GetMany<T>(int pageindex, int pagesize, string orderby = "", Expression<Func<T, bool>> exp = null) where T : class
        {
            if (pageindex < 1) pageindex = 1;
            if (string.IsNullOrEmpty(orderby))
                orderby = "Id descending";

            return Filter(exp).OrderBy(orderby).Skip(pagesize * (pageindex - 1)).Take(pagesize);
        }

        public async Task<List<T>> FindManyAsync<T>(int pageindex, int pagesize, string orderby = "", Expression<Func<T, bool>> exp = null) where T : class
        {
            if (pageindex < 1) pageindex = 1;
            if (string.IsNullOrEmpty(orderby))
                orderby = "Id descending";

            var qry = Filter(exp).OrderBy(orderby).Skip(pagesize * (pageindex - 1)).Take(pagesize);
            return await qry.ToListAsync();
        }

        //*get
        public T Get<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(exp);
        }
        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(exp);
        }

        //*add
        public void Create<TKey, T>(T entity) where T : Entity<TKey>
        {
            UpdateFieldsOnCreate<TKey, T>(entity);
            _context.Set<T>().Add(entity);
        }
        public async Task CreateAsync<TKey, T>(T entity) where T : Entity<TKey>
        {
            UpdateFieldsOnCreate<TKey, T>(entity);

            await _context.Set<T>().AddAsync(entity);
        }

        public void CreateMany<TKey, T>(T[] entities) where T : Entity<TKey>
        {
            foreach (var entity in entities)
            {
                UpdateFieldsOnCreate<TKey, T>(entity);
            }
            _context.Set<T>().AddRange(entities);
        }

        public async Task CreateManyAsync<TKey, T>(T[] entities) where T : Entity<TKey>
        {
            foreach (var entity in entities)
            {
                UpdateFieldsOnCreate<TKey, T>(entity);
            }
            await _context.Set<T>().AddRangeAsync(entities);
        }

        //*mod
        public void Update<TKey, T>(T entity) where T : Entity<TKey>
        {
            UpdateFieldsOnUpate<TKey, T>(entity);

            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;

            //if data has no change
            if (!_context.ChangeTracker.HasChanges())
            {
                entry.State = EntityState.Unchanged;
            }

        }

        public void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class
        {
            _context.Set<T>().Where(where).Update(entity);
        }

        //*del
        public void Remove<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual void RemoveMany<T>(Expression<Func<T, bool>> exp) where T : class
        {
            _context.Set<T>().Where(exp).Delete();
        }
        public virtual async Task RemoveManyAsync<T>(Expression<Func<T, bool>> exp) where T : class
        {
            await _context.Set<T>().Where(exp).DeleteAsync();
        }



        //*judge
        public bool Any<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().Any(exp);
        }

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return await _context.Set<T>().AnyAsync(exp);
        }

        //*others
        public int Count<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp).Count();
        }

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return await Filter(exp).CountAsync();
        }

        public int ExecuteDbSqlRaw(string sql)
        {
            return _context.Database.ExecuteSqlRaw(sql);
        }
        public async Task<int> ExecuteDbSqlRawAsync(string sql)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql);
        }

        public IQueryable<T> FromSetSqlRaw<T>(string sql, params object[] parameters) where T : class
        {
            return _context.Set<T>().FromSqlRaw(sql, parameters);
        }


        public void Save()
        {
            try
            {
                var entities = _context.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added|| e.State == EntityState.Modified)
                    .Select(e => e.Entity);

                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext, validateAllProperties: true);
                }

                _context.SaveChanges();
            }
            catch (ValidationException exc)
            {
                Console.WriteLine($"{nameof(Save)} validation exception: {exc?.Message}");
                throw exc.InnerException as Exception ?? exc;
            }
            catch (Exception ex) //DbUpdateException 
            {
                throw ex.InnerException as Exception ?? ex;
            }

        }
        public async Task<int> SaveAsync()
        {
            try
            {
                var entities = _context.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added|| e.State == EntityState.Modified)
                    .Select(e => e.Entity);

                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext, validateAllProperties: true);
                }

                return await _context.SaveChangesAsync();
            }
            catch (ValidationException exc)
            {
                Console.WriteLine($"{nameof(Save)} validation exception: {exc?.Message}");
                throw exc.InnerException as Exception ?? exc;
            }
            catch (Exception ex) //DbUpdateException 
            {
                throw ex.InnerException as Exception ?? ex;
            }
        }

        //*private
        private IQueryable<T> Filter<T>(Expression<Func<T, bool>> exp) where T : class
        {
            var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
        }

        private async Task<List<T>> FilterAsync<T>(Expression<Func<T, bool>> exp) where T : class
        {
            var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return await dbSet.ToListAsync();
        }

        void UpdateFieldsOnCreate<TKey,U>(U entity) where U : Entity<TKey>
        {
            if (entity.InitiateKey())
            {
                entity.CreateKeyVal();
            }

            var id = _currentActorId.ToString();
            entity.CreatorId = id.ToString();
            entity.CreationTime = DateTime.Now;
            entity.LastModifierId = id.ToString();
            entity.ModificationTime = DateTime.Now;
        }

        void UpdateFieldsOnUpate<TKey, U>(U entity) where U : Entity<TKey>
        {
            var id = _currentActorId.ToString();
            entity.LastModifierId = id;
            entity.ModificationTime = DateTime.Now;
            var old = Get<U>(x => x.Id.ToString() == entity.Id.ToString());
            entity.CreatorId = old.CreatorId;
            entity.CreationTime = old.CreationTime;
        }

    }
}
