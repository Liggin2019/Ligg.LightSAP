using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Infrastructure.DataModels;
using Ligg.Uwa.Application.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ligg.Infrastructure.Utilities.LogUtil;

namespace Ligg.Uwa.Application
{
    public class TreeRepository<TKey, T, TDbContext> : CommonRepository<TKey, T, TDbContext> where T : Tree<TKey> where TDbContext : DbContext
    {
        protected IUnitWork<TDbContext> UnitWork;
        protected IDbSetRepository<TKey, T, TDbContext> Repository;

        public TreeRepository(IUnitWork<TDbContext> unitWork, IDbSetRepository<TKey, T, TDbContext> repository) : base(unitWork, repository)
        {
            UnitWork = unitWork;
            Repository = repository;
        }


        //*list
        public List<T> GetRecursiveChildrenById<U,TKey>(string id, bool includeParent) where U : Tree<TKey>
        {
            var list = new List<T>();
            if (id== default(TKey).ToString())  
            {
                list = Repository.GetMany(null).ToList();
            }
            else
            {
                var entity = UnitWork.Get<U>(x => x.Id.ToString() == id);
                if (includeParent)
                {
                    list = Repository.GetMany(x => x.CascadedNo.Contains(entity.CascadedNo)).ToList();
                }
                else
                {
                    list = Repository.GetMany(x => x.CascadedNo.Contains(entity.CascadedNo) & x.CascadedNo != entity.CascadedNo).ToList();
                }
            }
            return list;
        }

        public List<string> GetRecursiveChildIdsById<U, TKey>(string id, bool includeParent) where U : Tree<TKey>
        {
            var entity = UnitWork.Get<U>(x => x.Id.ToString() == id.ToString());
            var children = GetRecursiveChildrenById<U, TKey>(id, includeParent);
            var idList = new List<string>();
            //idList.Add(entity.Id);
            foreach (var child in children)
            {
                idList.Add(child.Id.ToString());
            }
            return idList;
        }

        //*get
        public U GetTopParent<U>(U entity) where U : Tree<TKey>
        {
            if (entity == null) return null;
            if (entity.CascadedNo.Split('.').Length == 3) return entity;
            var no = entity.CascadedNo;
            var index = no.IndexOf('.', 3);
            var topParentNo= no.Substring(0, index) + '.';
            var topParent = UnitWork.Get<U>(u => u.CascadedNo == topParentNo);
            return topParent;
        }
        public string GetCascadedPathById<U>(string id) where U : Tree<TKey>
        {
            var entity = UnitWork.Get<U>(x => x.Id.ToString() == id);
            return GetCascadedPath<U>(entity);
        }

        //*save
        public async Task<string> SaveTreeEntityAsync<U>(U entity) where U : Tree<TKey>
        {
            try
            {
                var msg = CheckBeforeSave(entity);
                if (msg != Consts.OK) return msg;

                await UnitWork.ExecuteTransactionAsync(() =>
                {
                    if (IsDefaultKeyValue(entity.Id))//add
                    {
                        UnitWork.Create<TKey, U>(entity);
                        UpdateCascades(entity);
                    }
                    else//mod
                    {
                        UpdateSelfAndChildrenCascades(entity);
                        UnitWork.Update<TKey, U>(entity);

                    }

                    UnitWork.Save();
                });
            }
            catch (Exception ex)
            {
                Infrastructure.Utilities.LogUtil.LogHelper.Error(ex);
                return "SaveTreeEntityAsync failed";
            }

            return Consts.OK;
        }


        //*del
        public async Task<string> DeleteTreeEntitiesByIdsAsync<U, TKey>(string[] idStrArr, bool checkChildExistence=false) where U : Tree<TKey>
        {
            try
            {
                if(checkChildExistence)
                {
                    foreach (var idStr in idStrArr)
                    {
                        var hasChild = UnitWork.GetMany<U>(u => u.ParentId.ToString()== idStr).Any();
                        if(hasChild) return "child object exists";
                    }
                }

                var deletedCascadeNos = UnitWork.GetMany<U>(u => idStrArr.Contains(u.Id.ToString())).Select(u => u.CascadedNo).ToArray();
                var deletedIds = new List<string>();
                foreach (var cascadeNo in deletedCascadeNos)
                {
                    deletedIds.AddRange(UnitWork.GetMany<U>(u => u.CascadedNo.Contains(cascadeNo)).Select(u => u.Id.ToString()).ToArray());
                }

                await UnitWork.ExecuteTransactionAsync(() =>
                {
                    UnitWork.RemoveMany<U>(u => deletedIds.Contains(u.Id.ToString()));

                    UnitWork.Save();
                });
            }
            catch (Exception ex)
            {
                Infrastructure.Utilities.LogUtil.LogHelper.Error(ex);
                return "DeleteTreeEntitiesByIdsAsync failed";
            }

            return Consts.OK;
        }
        public async Task<string> DeleteTreeEntitiesByIdAsync<U,TKey>(string id, bool checkChildExistence = false) where U : Tree<TKey>
        {
            var ett = await GetEntityByIdStringAsync(id);
            if(ett==null) return "Entity does not exist";

            try
            {
                var idStr = id;
                if (checkChildExistence)
                {
                    var hasChild = UnitWork.GetMany<U>(u => u.ParentId== idStr).Any();
                    if (hasChild) return "child object exists";
                }

                var deletedCascadeNos = UnitWork.GetMany<U>(u => idStr==u.Id.ToString()).Select(u => u.CascadedNo).ToArray();
                var deletedIds = new List<string>();
                foreach (var cascadeNo in deletedCascadeNos)
                {
                    deletedIds.AddRange(UnitWork.GetMany<U>(u => u.CascadedNo.Contains(cascadeNo)).Select(u => u.Id.ToString()).ToArray());
                }

                await UnitWork.ExecuteTransactionAsync(() =>
                {
                    UnitWork.RemoveMany<U>(u => deletedIds.Contains(u.Id.ToString()));

                    UnitWork.Save();
                });
            }
            catch (Exception ex)
            {
                Infrastructure.Utilities.LogUtil.LogHelper.Error(ex);
                return "DeleteTreeEntitiesByIdAsync failed";
            }

            return Consts.OK;
        }

        //*private

        //##get
        private string GetCascadedPath<U>(U entity) where U : Tree<TKey>
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
                var idNo= new IntIdValue(i, parentNo);
                noList.Add(idNo);
            }

            var arr1 = noList.Select(x => x.Value).ToList();
            var parents = UnitWork.GetMany<U>(u => arr1.Contains(u.CascadedNo)).ToList();
            var cascadedName = "";
            for (int i = 2; i < noList.Count; i++)
            {
                var no = noList.Where(x => x.Id==i).First().Value;
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

        //##update
        private void UpdateSelfAndChildrenCascades<U>(U entity) where U : Tree<TKey>
        {
            var oldObj = Repository.Get(x => x.Id.ToString() == entity.Id.ToString());
            var newParentId = entity.ParentId;
            bool parentChanged = false;
            if (oldObj.ParentId.ToString() != newParentId.ToString()) parentChanged = true;

            if (parentChanged)
            {
                UpdateCascades(entity);
                UpdateChildrenCascades(entity);
            }
            else
            {
                entity.CascadedNo = oldObj.CascadedNo;
                //entity.CascadedId = oldObj.CascadedId;
                if (oldObj.Name != entity.Name)
                {
                    //var tmp = (oldObj.CascadedName).Replace(oldObj.Name, entity.Name);
                    //entity.CascadedName = tmp;
                    UpdateChildrenCascadedNames(entity);
                }

                //else
                ///entity.CascadedName = oldObj.CascadedName;
            }
        }

        private void UpdateCascades<U>(U entity) where U : Tree<TKey>
        {
            string cascadedNo;
            int currentCascadeNo = 1;
            var sameLevels = UnitWork.GetMany<U>(u => u.ParentId == entity.ParentId && u.Id.ToString() != entity.Id.ToString());
            foreach (var obj in sameLevels)
            {
                int objCascadeNo = int.Parse(obj.CascadedNo.TrimEnd('.').Split('.').Last());
                if (currentCascadeNo <= objCascadeNo) currentCascadeNo = objCascadeNo + 1;
            }

            var parent = UnitWork.Get<U>(u => u.Id.ToString() == entity.ParentId);

            if (entity.ParentId.ToString() != default(TKey).ToString())
            {
                if (parent != null)
                {
                    cascadedNo = parent.CascadedNo + currentCascadeNo + ".";
                    //entity.CascadedId = parent.CascadedId + ">" + entity.Id.ToString();
                    //entity.CascadedName = parent.CascadedName + ">" + entity.Name;
                }
                else
                {
                    throw new Exception("Can't find parent node");
                }
            }
            else
            {
                //top nodes,first top node CascadeNo=.0.1.;
                cascadedNo = ".0." + currentCascadeNo + ".";
                //entity.CascadedId = entity.Id.ToString();
                //entity.CascadedName = entity.Name;
            }

            entity.CascadedNo = cascadedNo;

        }


        private void UpdateChildrenCascades<U>(U entity) where U : Tree<TKey>
        {
            var oldObj = Repository.Get(u => u.Id.ToString() == entity.Id.ToString());
            //get old CascadeNo
            var oldCascadeNo = oldObj.CascadedNo;
            //get children by CascadeNo
            var childs = Repository.GetMany(x => x.CascadedNo.Contains(oldCascadeNo) && x.Id.ToString() != entity.Id.ToString())
                .OrderBy(x => x.CascadedNo).ToList();

            //update children Cascades
            foreach (var child in childs)
            {
                child.CascadedNo = child.CascadedNo.Replace(oldCascadeNo, entity.CascadedNo);
                //child.CascadedId = entity.CascadedId + ">" + child.Id;
                //child.CascadedName = entity.CascadedName + ">" + child.Name;
                UnitWork.Update<TKey, T>(child);
            }
        }

        private void UpdateChildrenCascadedNames<U>(U entity) where U : Tree<TKey>
        {
            var oldObj = Repository.Get(u => u.Id.ToString() == entity.Id.ToString());
            //get old CascadeNo
            var oldCascadeNo = oldObj.CascadedNo;
            //get children by CascadeNo
            var childs = Repository.GetMany(x => x.CascadedNo.Contains(oldCascadeNo) && x.Id.ToString() != entity.Id.ToString())
                .OrderBy(x => x.CascadedNo).ToList();

            //update children CascadedName
            foreach (var child in childs)
            {
                //child.CascadedName = (child.CascadedName).Replace(oldObj.Name, entity.Name);
                UnitWork.Update<TKey, T>(child);
            }
        }

        //##check
        private string CheckBeforeSave<U>(U entity) where U : Tree<TKey>
        {
            var newParentId = entity.ParentId;
            if (newParentId != default(TKey).ToString())
            {
                var newParent = Repository.Get(x => x.Id.ToString() == entity.ParentId.ToString());
                if (newParent.CascadedNo.LastIndexOf('.') == 12) return "Tree level can't exceed 10";
            }

            if (!IsDefaultKeyValue(entity.Id)) //update
            {
                if (entity.Id.ToString() == entity.ParentId.ToString()) return "Can't choose self as parent";

                var oldEntity = Repository.Get(x => x.Id.ToString() == entity.Id.ToString());
                if (oldEntity == null) return "Entity doesn't exist";
                if (oldEntity.ParentId.ToString() == default(TKey).ToString())
                {
                    if (newParentId.ToString() != default(TKey).ToString())
                        return "Top Level tree node can not be demoted";
                }
            }
            return Consts.OK;
        }




    }
}
