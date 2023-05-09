using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ligg.Uwa.Application.Shared
{
    public static class CommonHelper
    {

        //*list
        public static List<T> GetEnnabledRecursiveChildrenByParenId<T, TKey>(List<T> allList, string parentId, bool includeParent) where T : Tree<TKey>
        {
            var rst = new List<T>();
            if (allList.Count() == 0) return rst;
            var parent = new Tree<TKey>();
            var children = allList;
            if (parentId != parent.Id.ToString())
            {
                parent = allList.Find(x => x.Id.ToString() == parentId);
                if (parent == null) return rst;

                if (includeParent)
                    children = allList.FindAll(x => x.CascadedNo.Contains(parent.CascadedNo));
                else
                    children = allList.FindAll(x => x.CascadedNo.Contains(parent.CascadedNo) & x.CascadedNo != parent.CascadedNo);

            }
            var disabledItems = children.Where(x => !x.Status).ToList();
            if (disabledItems.Count() > 0)
            {
                var nos = disabledItems.Select(x => x.CascadedNo).ToArray();
                foreach (var no in nos)
                {
                    var disabledChildren = children.FindAll(x => x.CascadedNo.Contains(no));
                    foreach (var disabledChild in disabledChildren)
                    {
                        if (children.Any(x => x.Id.ToString() == disabledChild.Id.ToString()))
                            children.Remove(disabledChild);
                    }
                }
            }

            if (children.Count() == 0) return rst;
            return children;
        }


        public static List<T> GetPagedList<T>(List<T> list, Pagination pagination)
        {
            pagination.Total = list.Count;
            var pageIndex = pagination.PageIndex;
            var pageSize = pagination.PageSize;

            var desc = pagination.SortType.Trim().ToLower() == "desc";
            var list1 = new List<T>().AsEnumerable();
            if (desc)
                list1 = from p in list orderby GetPropertyValue(p, pagination.SortName) descending  select p;
            else
                list1 = from p in list orderby GetPropertyValue(p, pagination.SortName) select p;

            var list2 = list1.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return list2.ToList();
        }

        //*get
        public static T GetTopParent<T, TKey>(List<T> list, T entity) where T : Tree<TKey>
        {
            if (entity == null) return null;
            if (entity.CascadedNo.Split('.').Length == 3) return entity;
            var no = entity.CascadedNo;
            var index = no.IndexOf('.', 3);
            var topParentNo = no.Substring(0, index) + '.';
            var topParent = list.Find(u => u.CascadedNo == topParentNo);
            return topParent;
        }
        public static string GetCascadedPathById<T, TKey>(List<T> list, string id) where T : Tree<TKey>
        {
            var entity = list.Find(x => x.Id.ToString() == id);
            return GetCascadedPath<T, TKey>(list,entity);
        }



        public static T GetMaxOne<T, TField>(List<T> list, Expression<Func<T, TField>> sortExp, Expression<Func<T, bool>> scopeExp = null)
        {
            var list1 = scopeExp == null ? list.AsQueryable() : list.AsQueryable().Where(scopeExp);
            var list2 = list1.OrderByDescending(sortExp);
            return list2.Count() > 0 ? list2.First() : default(T);
        }

        public static object GetDefaultKeyValue(Object obj)
        {
            var type = obj.GetType();
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        //*judge
        public static bool FieldValueExists<T, TField, TKey>(List<T> list, TKey id, string fieldValue, Expression<Func<T, TField>> selectExp, Expression<Func<T, bool>> scopeExp = null, bool ignoreCase = false) where T : Entity<TKey>
        {
            var list1 = scopeExp == null ? list.AsQueryable() : list.AsQueryable().Where(scopeExp);
            var valArr = id.ToString() == default(TKey).ToString() ?
                list1.Select(selectExp).ToArray().Distinct().Where(x => x != null) :
                list1.Where(x => x.Id.ToString() != id.ToString()).Select(selectExp).ToArray().Distinct().Where(x => x != null);
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

        //*private
        private static string GetCascadedPath<T,TKey>(List<T> list,T entity) where T : Tree<TKey>
        {
            if (entity.ParentId.ToString() == default(TKey).ToString())
            {
                return entity.Name;
            }
            var cascadedNo = entity.CascadedNo;
            var noList = new List<IntIdValue>();

            var arr = entity.CascadedNo.Split('.');
            var parentNo = "";
            for (int i = 1; i < arr.Length; i++)
            {
                if (i == 1)
                {
                    parentNo = '.' + arr[i] + '.';
                }
                else
                {
                    parentNo = parentNo + arr[i] + '.';
                }
                var idNo = new IntIdValue(i, parentNo);
                noList.Add(idNo);
            }

            var arr1 = noList.Select(x => x.Value??"").ToArray();
            var parents = list.Where(x => arr1.Contains(x.CascadedNo)).ToList();
            var cascadedName = "";
            for (int i = 2; i < noList.Count; i++)
            {
                var no = noList.Where(x => x.Id == i).First().Value;
                var parent = parents.Where(x => x.CascadedNo == no).First();
                if (i == 2)
                {
                    cascadedName = parent.Name;
                }
                else
                {
                    cascadedName = cascadedName + ">" + parent.Name;
                }
            }

            return cascadedName;
        }


    }

}
