using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using System.ComponentModel;

using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore.Helpers; 

namespace Ligg.EntityFrameworkCore
{
    public class DbSetRepository<TKey, T, TDbContext> : IDbSetRepository<TKey, T, TDbContext> where T : Entity<TKey> where TDbContext : DbContext
    {
        private TDbContext _context;
        public string _currentActorId;
        public DbSetRepository(TDbContext context)
        {
            _context = context;
        }

        //#get
        public void SetCurrentActorId(string id)
        {
            _currentActorId = id;
        }
        public T Get(Expression<Func<T, bool>> exp)
        {

            return _context.Set<T>().AsNoTracking().FirstOrDefault(exp);
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> exp)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(exp);

        }
        public IQueryable<T> GetMany(Expression<Func<T, bool>> exp = null)
        {
            return Filter(exp);
        }
        public async Task<List<T>> FindManyAsync(Expression<Func<T, bool>> exp = null)
        {
            return await FilterAsync(exp);
        }
        public IQueryable<T> GetMany(int pageindex, int pagesize, string orderby = "", Expression<Func<T, bool>> exp = null)
        {
            if (pageindex < 1) pageindex = 1;
            if (string.IsNullOrEmpty(orderby)) orderby = "Id descending";
            return Filter(exp).OrderBy(orderby).Skip(pagesize * (pageindex - 1)).Take(pagesize);
        }
        public async Task<List<T>> FindManyAsync(int pageindex, int pagesize, string orderby = "", Expression<Func<T, bool>> exp = null)
        {
            if (pageindex < 1) pageindex = 1;
            if (string.IsNullOrEmpty(orderby)) orderby = "Id descending";
            var query = Filter(exp).OrderBy(orderby).Skip(pagesize * (pageindex - 1)).Take(pagesize);
            return await query.ToListAsync();
        }

        //#add
        public void Create(T entity)
        {
            UpdateFieldsOnCreate(entity);

            _context.Set<T>().Add(entity);
            Save();
            _context.Entry(entity).State = EntityState.Detached;
        }
        public async Task<int> CreateAsync(T entity)
        {
            UpdateFieldsOnCreate(entity);

            _context.Set<T>().Add(entity);
            return await SaveAsync();
            //_context.Entry(entity).State = EntityState.Detached;
        }

        public void CreateMany(T[] entities)
        {
            foreach (var entity in entities)
            {
                UpdateFieldsOnCreate(entity);
            }

            _context.Set<T>().AddRange(entities);
            Save();
        }
        public async Task<int> CreateManyAsync(T[] entities)
        {
            foreach (var entity in entities)
            {
                UpdateFieldsOnCreate(entity);
            }

            await _context.Set<T>().AddRangeAsync(entities);
            return await SaveAsync();
        }

        //#mod
        public void Update(T entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;

            if (!_context.ChangeTracker.HasChanges())
            {
                return;
            }

            Save();
        }
        public async Task<int> UpdateAsync(T entity)
        {
            UpdateFieldsOnUpate(entity);

            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;

            if (!_context.ChangeTracker.HasChanges())
            {
                return 0;
            }

            return await SaveAsync();
        }
        public void Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity)
        {
            var entities = Filter(where);
            if (entities.Count() == 0) return;
            foreach (var obj in entities)
            {
                UpdateFieldsOnUpate(obj);
            }

            _context.Set<T>().Where(where).Update(entity);
            Save();
        }

        public async Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity)
        {
            var entities = Filter(where);
            if (entities.Count() != 0) return 0;

            foreach (var obj in entities)
            {
                UpdateFieldsOnUpate(obj);
            }


            //await _context.Set<T>().Where(where).UpdateAsync(entity);
            _context.Set<T>().Where(where).Update(entity);

            if (!_context.ChangeTracker.HasChanges())
            {
                return 0;
            }

            return await SaveAsync();
        }

        //#del
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            Save();
        }
        public async Task<int> RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await SaveAsync();
        }
        public void RemoveMany(Expression<Func<T, bool>> exp)
        {
            _context.Set<T>().Where(exp).Delete();
            Save();
        }

        public async Task<int> RemoveManyAsync(Expression<Func<T, bool>> exp)
        {
            _context.Set<T>().Where(exp).Delete();
            return await SaveAsync();
            //await _context.Set<T>().Where(exp).DeleteAsync();
            //Save();
        }



        //#judge
        public bool Any(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().Any(exp);
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> exp)
        {
            return await _context.Set<T>().AnyAsync(exp);
        }

        //#other
        public int Count(Expression<Func<T, bool>> exp = null)
        {
            return Filter(exp).Count();
        }
        public async Task<int> CountAsync(Expression<Func<T, bool>> exp = null)
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

        public IQueryable<T> FromSetSqlRaw(string sql, params object[] parameters)
        {
            return _context.Set<T>().FromSqlRaw(sql, parameters);
        }
        //public IQueryable<T> FromQuerySqlRaw(string sql, params object[] parameters)
        //{
        //    return _context.Query<T>().FromSqlRaw(sql, parameters); ////for ef 3.0.1 is ok, for 5.0.15 is not ok
        //}


        //#private
        private IQueryable<T> Filter(Expression<Func<T, bool>> exp)
        {
            var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
        }


        private async Task<List<T>> FilterAsync(Expression<Func<T, bool>> exp)
        {
            var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return await dbSet.ToListAsync();
        }

        private void Save()
        {
            try
            {
                var entities = _context.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added
                                || e.State == EntityState.Modified)
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

        private async Task<int> SaveAsync()
        {
            try
            {
                var entities = _context.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added
                                || e.State == EntityState.Modified)
                    .Select(e => e.Entity);

                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext, validateAllProperties: true);
                }

                return await _context.SaveChangesAsync();
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"{nameof(Save)} validation exception: {ex?.Message}");
                throw ex.InnerException as Exception ?? ex;
            }
            catch (Exception ex) //DbUpdateException 
            {
                throw ex.InnerException as Exception ?? ex;
            }
        }

        void UpdateFieldsOnCreate(T entity)
        {
            if (entity.InitiateKey())
            {
                entity.CreateKeyVal();
            }

            var id = _currentActorId;
            entity.CreatorId = id;
            entity.CreationTime = DateTime.Now;
            entity.LastModifierId = id;
            entity.ModificationTime = DateTime.Now;
        }

        void UpdateFieldsOnUpate(T entity)
        {
            var id = _currentActorId;
            entity.LastModifierId = id;
            entity.ModificationTime = DateTime.Now;
            var old = Get(x => x.Id.ToString() == entity.Id.ToString());
            entity.CreatorId = old.CreatorId;
            entity.CreationTime = old.CreationTime;
        }



    }

}