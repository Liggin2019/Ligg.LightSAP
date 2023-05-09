using ExpressionBuilder.Generics;
using Ligg.Infrastructure.Extensions;
using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class ConfigItemRepository : CommonRepository<long, ConfigItem, DbSetContext>
    {
        private readonly CacheHandler _cacheHandler;
        public ConfigItemRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, ConfigItem, DbSetContext> repository) : base(unitWork, repository)
        {
            _cacheHandler = new CacheHandler();
        }

        //*list
        public async Task<List<ConfigItem>> GetPagedEntitiesAsync(ExpressionBuilderReqArgs param, Pagination pagination)
        {
            if (param.OptionString.IsNullOrEmpty()) return new List<ConfigItem>();
            param.Formular = "OptionString:MasterId, Judgement:Status, DateTime:CreationTime, Mark:Key Attribute Attribute1 Attribute2 Value, Text: Name Description";
            var dynamicfilter = new Filter<ConfigItem>();
            ExpressionBuilderEx.GetAndExpression(param, dynamicfilter);
            var allList = _cacheHandler.GetAllCachedConfigItems();
            var list = allList.AsQueryable().Where(dynamicfilter).OrderBy(x => x.Sequence).ToList();
            return CommonHelper.GetPagedList(list, pagination); ;
            //return await FindPagedEntitiesAsync(dynamicfilter, pagination);
        }

        //*mod
        public async Task<string> SetAsDefault(string id)
        {
            var entity = await GetEntityByIdStringAsync(id);
            var otherEntities = await Repository.FindManyAsync(x => x.MasterId == entity.MasterId & x.Id != entity.Id);
            await UnitWork.ExecuteTransactionAsync(() =>
            {
                entity.IsDefault = true;
                UnitWork.Update<long, ConfigItem>(entity);
                foreach (var obj in otherEntities)
                {
                    obj.IsDefault = false;
                    UnitWork.Update<long, ConfigItem>(obj);
                }
                UnitWork.Save();
                _cacheHandler.RemoveConfigItemCache();
            });
            return Consts.OK;
        }



    }
}