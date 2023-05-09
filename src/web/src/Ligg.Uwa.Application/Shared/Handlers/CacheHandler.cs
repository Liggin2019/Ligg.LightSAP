using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Infrastructure.Utilities.DbUtil;
using Ligg.Uwa.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Ligg.Uwa.Application.Shared
{
    public class CacheHandler
    {
        private IUnitWork<DbSetContext> _unitwork;
        private readonly ICacheHandler _cacheHandler;
        public CacheHandler()
        {
            //_context = GlobalContext.ServiceProvider?.GetService<DbSetContext>();
            _unitwork = GlobalContext.ServiceProvider?.GetService<IUnitWork<DbSetContext>>();
            _cacheHandler = GlobalContext.ServiceProvider?.GetService<ICacheHandler>();
            var oprtor = CurrentOperator.Instance.GetCurrent();
            _unitwork.SetCurrentActorId(oprtor == null ? new Operator().Id.ToString() : oprtor.Id.ToString());
        }

        //public OperatorInfo GetCachedOperatorInfo(string Id)
        //{
        //    return _cacheHandler.GetCache<OperatorInfo>(Id);
        //}
        public void UpdateOperatorCache(string Id, OperatorInfo oprtorInfo)
        {
            _cacheHandler.SetCache(Id, oprtorInfo);
        }
        public void RemoveOperatorCache(string Id)
        {
            _cacheHandler.RemoveCache(Id);
        }

        //*tenants
        public List<Tenant> GetAllCachedTenants()
        {
            var list = GetAllCachedList<Tenant>(Consts.CACHEKEY_TNT_LST);
            if (list == null)
            {
                //list = _unitwork.GetMany<Tenant>(null).ToList();
                var sql = "select * from sys_tenants";
                list = DbHelper.FindMany<Tenant>(sql).ToList();
                _cacheHandler.SetCache(Consts.CACHEKEY_TNT_LST, list);
            }
            return list;
        }
        public void RemoveTenantCache()
        {
            RemoveCache(Consts.CACHEKEY_TNT_LST);
        }

        //*menuItems
        public List<MenuItem> GetAllCachedMenuItems()
        {
            var list = GetAllCachedList<MenuItem>(Consts.CACHEKEY_MNU_ITM_LST);
            if (list == null)
            {
                //list = _unitwork.GetMany<MenuItem>(null).ToList();
                var sql = "select * from sys_menuitems";
                list = DbHelper.FindMany<MenuItem>(sql).ToList();
                _cacheHandler.SetCache(Consts.CACHEKEY_MNU_ITM_LST, list);
            }
            return list;
        }
        public void RemoveMenuItemCache()
        {
            RemoveCache(Consts.CACHEKEY_MNU_ITM_LST);
        }

        //*configs
        public List<Config> GetAllCachedConfigs()
        {
            var list = GetAllCachedList<Config>(Consts.CACHEKEY_CFG_LST);
            if (list == null)
            {
                //list = _unitwork.GetMany<Config>(null).ToList();
                var sql = "select * from sys_configs";
                list = DbHelper.FindMany<Config>(sql).ToList();
                _cacheHandler.SetCache(Consts.CACHEKEY_CFG_LST, list);
            }
            return list;
        }
        public void RemoveConfigCache()
        {
            RemoveCache(Consts.CACHEKEY_CFG_LST);
        }

        //*configItems
        public List<ConfigItem> GetAllCachedConfigItems()
        {
            var list = GetAllCachedList<ConfigItem>(Consts.CACHEKEY_CFG_ITM_LST);
            if (list == null)
            {
                //list = _unitwork.GetMany<ConfigItem>(null).ToList();
                var sql = "select * from sys_configitems";
                list = DbHelper.FindMany<ConfigItem>(sql).ToList();
                foreach(var obj in list)
                {
                    obj.Attribute = obj.Attribute ?? string.Empty;
                    obj.Attribute1 = obj.Attribute1 ?? string.Empty;
                    obj.Attribute2 = obj.Attribute2 ?? string.Empty;
                    obj.Value = obj.Value ?? string.Empty;
                }
                _cacheHandler.SetCache(Consts.CACHEKEY_CFG_ITM_LST, list);
            }
            return list;
        }
        public void RemoveConfigItemCache()
        {
            RemoveCache(Consts.CACHEKEY_CFG_ITM_LST);
        }

        //*tags
        public List<Category> GetAllCachedTags()
        {
            var list = GetAllCachedList<Category>(Consts.CACHEKEY_TAG_LST);
            if (list == null)
            {
                //list = _unitwork.GetMany<Category>(x => x.Type == (int)CategoryType.Tag).ToList();
                var sql = "select * from scc_categories where Type="+ ((int)CategoryType.Tag);
                list = DbHelper.FindMany<Category>(sql).ToList();
                _cacheHandler.SetCache(Consts.CACHEKEY_TAG_LST, list);
            }
            return list;
        }
        public void RemoveTagCache()
        {
            RemoveCache(Consts.CACHEKEY_TAG_LST);
        }

        //*directories
        public List<Category> GetAllCachedDirectories()
        {
            var list = GetAllCachedList<Category>(Consts.CACHEKEY_DIR_LST);
            if (list == null)
            {
                //list = _unitwork.GetMany<Category>(x=>x.Type!=(int)CategoryType.Tag).ToList();
                var sql = "select * from scc_categories where Type!=" + ((int)CategoryType.Tag);
                list = DbHelper.FindMany<Category>(sql).ToList();
                _cacheHandler.SetCache(Consts.CACHEKEY_DIR_LST, list);
            }
            return list;
        }
        public void RemoveDirectoryCache()
        {
            RemoveCache(Consts.CACHEKEY_DIR_LST);
        }

        //*permissions
        public List<Permission> GetAllCachedPermissions()
        {
            var list = GetAllCachedList<Permission>(Consts.CACHEKEY_PMS_LST);
            if (list == null)
            {
                //list = _unitwork.GetMany<Permission>(null).ToList();
                var sql = "select * from sys_permissions";
                list = DbHelper.FindMany<Permission>(sql).ToList();
                _cacheHandler.SetCache(Consts.CACHEKEY_PMS_LST, list);
            }
            return list;
        }
        public void RemovePermissionCache()
        {
            RemoveCache(Consts.CACHEKEY_PMS_LST);
        }


        //*private
        private List<T> GetAllCachedList<T>(string cacheKey)
        {
            return _cacheHandler.GetCache<List<T>>(cacheKey);
        }

        private void RemoveCache(string cacheKey)
        {
            _cacheHandler.RemoveCache(cacheKey);
        }

    }
}