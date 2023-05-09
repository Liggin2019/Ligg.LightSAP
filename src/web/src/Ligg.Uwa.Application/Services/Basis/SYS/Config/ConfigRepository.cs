using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class ConfigRepository : CommonRepository<long, Config, DbSetContext>
    {
        public ConfigRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, Config, DbSetContext> repository, ICacheHandler cacheManager) : base(unitWork, repository)
        {
            _cacheHandler = new CacheHandler();
        }
        private CacheHandler _cacheHandler;

        //*del
        public async Task<string> DeleteByIdsAsync(string ids)
        {
            var idArr = ids.Split<long>(',');
            //check subitems firstly
            await UnitWork.ExecuteTransactionAsync(() =>
            {
                UnitWork.RemoveMany<Config>(u => idArr.Contains(u.Id));
                UnitWork.Save();
                _cacheHandler.RemoveConfigCache();
                //_cacheHandler.RemoveCache(_cacheKey);
            });

            return Consts.OK;
        }

        //*common
        public List<Config> GetAllEnabledConfigs()
        {
            var allCfgList = new List<Config>();
            var allCustCfgList = _cacheHandler.GetAllCachedConfigs().FindAll(x=>x.Status); //allCustCfgList
            allCfgList.AddRange(allCustCfgList);
            var allSysCfgList = GetAllSysConfigs();
            allCfgList.AddRange(allSysCfgList);
            return allCfgList;
        }

        public List<Config> GetAllSysConfigs()
        {
            var types1 = EnumHelper.GetIds<DevConfigType>();
            var types2 = EnumHelper.GetIds<OrpConfigType>();
            var types10 = EnumHelper.GetIds<FuncConfigType>();
            var types = new List<int>();
            types.AddRange(types1);
            types.AddRange(types2);
            types.AddRange(types10);
            var subTypes1 = EnumHelper.EnumToKeyValueDescriptionList(typeof(DevConfigSubType));
            var subTypes2 = EnumHelper.EnumToKeyValueDescriptionList(typeof(OrpConfigSubType));
            var subTypes10 = EnumHelper.EnumToKeyValueDescriptionList(typeof(FuncConfigSubType));
            var subTypes = new List<KeyValueDescription>();
            subTypes.AddRange(subTypes1);
            subTypes.AddRange(subTypes2);
            subTypes.AddRange(subTypes10);
            var list = new List<Config>();
            foreach (var subType in subTypes)
            {
                var cfg = new Config();
                var id = Convert.ToInt32(subType.Key);
                cfg.Id = id;
                cfg.Code = subType.Value;
                cfg.Name = subType.Description;
                //var type = types.Where(x => id > 100 * x & id < 100 * x + 100).FirstOrDefault();
                var type = types.Where(x => id > x - 1 & id < x + 100).FirstOrDefault();
                cfg.Type = type;
                cfg.Sequence = id;
                cfg.Status = true;
                list.Add(cfg);
            }
            return list;
        }

        private string GetConfigTypeName(int typeId)
        {
            var name = EnumHelper.GetNameById<TntConfigType>(typeId);
            if (name.IsNullOrEmpty())
                name = EnumHelper.GetNameById<DevConfigType>(typeId);
            if (name.IsNullOrEmpty())
                name = EnumHelper.GetNameById<OrpConfigType>(typeId);
            if (name.IsNullOrEmpty())
                name = EnumHelper.GetNameById<CustConfigType>(typeId);
            if (name.IsNullOrEmpty())
                name = EnumHelper.GetNameById<FuncConfigType>(typeId);
            return name;
        }

    }
}