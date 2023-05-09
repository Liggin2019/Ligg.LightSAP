
using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Ligg.Uwa.Application
{
    public class CommonRepository<TKey, T, TDbContext> where T : Entity<TKey> where TDbContext : DbContext
    {
        protected IUnitWork<TDbContext> UnitWork;
        protected IDbSetRepository<TKey, T, TDbContext> Repository;

        public CommonRepository(IUnitWork<TDbContext> unitWork, IDbSetRepository<TKey, T, TDbContext> repository)
        {
            UnitWork = unitWork;
            Repository = repository;
            var oprtor = CurrentOperator.Instance.GetCurrent();
            var id = oprtor == null ? new Operator().Id.ToString() : oprtor.Id.ToString();
            Repository.SetCurrentActorId(id);
            UnitWork.SetCurrentActorId(id);
        }

        //*list
        public async Task<List<T>> FindEntitiesByIdArrayAsync(string[] ids)
        {
            return await Repository.FindManyAsync(u => ids.Contains(u.Id.ToString()));
        }

        public async Task<List<T>> FindEntitiesByExpressionAsync(Expression<Func<T, bool>> exp)
        {
            return await Repository.FindManyAsync(exp);
        }

        public async Task<List<T>> FindPagedEntitiesAsync(Expression<Func<T, bool>> exp, Pagination pagination)
        {
            var orderby = pagination.SortName + (pagination.SortType.Trim().ToLower() == "desc" ? " descending" : "");
            pagination.Total = Repository.Count(exp);

            var data = await Repository.FindManyAsync(pagination.PageIndex, pagination.PageSize, orderby, exp);
            return data;
        }

        //*get
        public T GetEntityById(TKey id)
        {
            //return await Repository.FindManyAsync(u => u.Id == id)//error: Operator '==' cannot be applied to operands of type 'TKey' and 'TKey' 
            return Repository.Get(u => u.Id.ToString() == id.ToString());
        }
        public async Task<T> GetEntityByIdAsync(TKey id)
        {
            return await Repository.GetAsync(u => u.Id.ToString() == id.ToString());
        }
        public async Task<T> GetEntityByIdStringAsync(string id)
        {
            return await Repository.GetAsync(u => u.Id.ToString() == id);
        }

        public async Task<T> GetEntityByExpressionAsync(Expression<Func<T, bool>> exp)
        {
            return await Repository.GetAsync(exp);
        }

        public async Task<T> GetMaxOneAsync<TField>(Expression<Func<T, TField>> sortExp, Expression<Func<T, bool>> scopeExp = null)
        {
            var lst = Repository.GetMany(scopeExp);
            var lst1 = lst.OrderByDescending(sortExp).ToList();
            return lst1.Count > 0 ? lst1.First() : null;
        }

        //*save
        public async Task<string> SaveEntityAsync(T entity)
        {
            if (IsDefaultKeyValue(entity.Id))//add
            {
                await CreateEntityAsync(entity);
            }
            else
            {
                var oldEtt = await GetEntityByIdAsync(entity.Id);
                if (oldEtt == null) return "Entity doesn't exist";
                await UpdateEntityAsync(entity);
            }
            return Consts.OK;
        }

        public async Task<int> CreateEntityAsync(T entity)
        {
            return await Repository.CreateAsync(entity);
        }

        public async Task<int> CreateEntitiesAsync(T[] entities)
        {
            return await Repository.CreateManyAsync(entities);
        }

        public async Task<int> UpdateEntityAsync(T entity)
        {
            return await Repository.UpdateAsync(entity);
        }


        //*del
        public async Task DeleteEntitiesByIdArrayAsync(string[] ids)
        {
            await Repository.RemoveManyAsync(u => ids.Contains(u.Id.ToString()));
            //await Repository.RemoveManyAsync(u => ids.ToString().Contains(u.Id.ToString()));
        }
        //public async Task DeleteEntitiesByIdStringsAsync(string[] ids)
        //{
        //    await Repository.RemoveManyAsync(u => ids.Contains(u.Id.ToString()));
        //}

        public async Task DeleteEntityByIdAsync(TKey id)
        {
            var entity = GetEntityById(id);
            if (entity != null)
                Repository.RemoveMany(u => u.Id.ToString() == id.ToString());
        }
        public async Task DeleteEntityByIdStringAsync(string id)
        {
            var entity = await GetEntityByIdStringAsync(id);
            if (entity != null)
                Repository.RemoveMany(u => u.Id.ToString() == id);
        }


        //*judge
        public bool IsDefaultKeyValue(TKey id)
        {
            return id.ToString() == default(TKey).ToString();
        }

        public bool FieldValueExists<TField>(TKey id, string fieldValue, Expression<Func<T, TField>> selectExp, Expression<Func<T, bool>> scopeExp = null, bool ignoreCase = false)
        {
            var valArr = IsDefaultKeyValue(id) ?
                Repository.GetMany(scopeExp).Select(selectExp).ToArray().Distinct().Where(x => x != null) :
                Repository.GetMany(scopeExp).Where(x => x.Id.ToString() != id.ToString()).Select(selectExp).ToArray().Distinct().Where(x => x != null);
            foreach (var val in valArr)
            {
                if (ignoreCase)
                {
                    if (val.ToString().ToLower() == fieldValue.ToLower()) return true;
                }
                else
                {
                    if (val.ToString() == fieldValue) return true;
                }
            }

            return false;
        }

        public bool FieldValueExistsByIdString<TField, TKey>(string id, string fieldValue, Expression<Func<T, TField>> selectExp, Expression<Func<T, bool>> scopeExp = null, bool ignoreCase = false)
        {
            var valArr = id == default(TKey).ToString() ?
                            Repository.GetMany(scopeExp).Select(selectExp).ToArray().Distinct().Where(x => x != null) :
                            Repository.GetMany(scopeExp).Where(x => x.Id.ToString() != id.ToString()).Select(selectExp).ToArray().Distinct().Where(x => x != null);
            foreach (var val in valArr)
            {
                if (ignoreCase)
                {
                    if (val.ToString().ToLower() == fieldValue.ToLower()) return true;
                }
                else
                {
                    if (val.ToString() == fieldValue) return true;
                }
            }

            return false;
        }

        //*common
        public TKey GetDefaultKeyValue(TKey id)
        {
            return default(TKey);
        }

    }


}