
using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Application.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Ligg.Uwa.Basis.SYS
{
    public class PermissionRepository : CommonRepository<long, Permission, DbSetContext>
    {
        private readonly CacheHandler _cacheHandler;
        public PermissionRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, Permission, DbSetContext> repository, ICacheHandler cacheManager) : base(unitWork, repository)
        {
            _cacheHandler = new CacheHandler();
        }

        public async Task<string> AddAsync(int type, string masterId, int actorType, string[] actorIdArr)
        {
            var allEtts = _cacheHandler.GetAllCachedPermissions();
            var addEtts = new List<Permission>();
            foreach (var actorId in actorIdArr)
            {
                var entity = new Permission();
                entity.Type = type;
                entity.MasterId = masterId;
                entity.ActorType = actorType;
                entity.ActorId = actorId;
                addEtts.Add(entity);
            }

            await UnitWork.ExecuteTransactionAsync(() =>
            {
                UnitWork.RemoveMany<Permission>(x => x.Type == type&x.MasterId == masterId & x.ActorType == actorType);
                UnitWork.CreateMany<long, Permission>(addEtts.ToArray());
                UnitWork.Save();
            });
            _cacheHandler.RemovePermissionCache();
            return Consts.OK;

        }




    }
}