using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using System.Threading.Tasks;


namespace Ligg.Uwa.Basis.SYS
{
    public class TenantRepository : CommonRepository<long, Tenant, DbSetContext>
    {
        private readonly CacheHandler _cacheHandler;
        public TenantRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, Tenant, DbSetContext> repository) : base(unitWork, repository)
        {
            _cacheHandler = new CacheHandler();
        }

        public async Task<string> SetAsDefault(string id)
        {
            var entity = await GetEntityByIdStringAsync(id);
            var otherEntities = await Repository.FindManyAsync(x=>x.Id != entity.Id);
            await UnitWork.ExecuteTransactionAsync(() =>
            {
                entity.IsDefault = true;
                UnitWork.Update<long, Tenant>(entity);
                foreach (var obj in otherEntities)
                {
                    obj.IsDefault = false;
                    UnitWork.Update<long, Tenant>(obj);
                }
                UnitWork.Save();
                _cacheHandler.RemoveTenantCache();
            });
            return Consts.OK;
        }

    }
}