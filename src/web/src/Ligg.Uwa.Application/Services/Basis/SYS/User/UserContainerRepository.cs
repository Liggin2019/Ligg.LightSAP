using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class UserContainerRepository : CommonRepository<long, UserContainer, DbSetContext>
    {

        public UserContainerRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, UserContainer, DbSetContext> repository) : base(unitWork, repository)
        {
        }

        public async Task<string> UpdateUserContainersByTypeAndCtnIdAsync(int type, string ctnId, List<string> userIds)
        {
            if(userIds==null)
            {
                await UnitWork.ExecuteTransactionAsync(() =>
                {
                    UnitWork.RemoveMany<UserContainer>(x => x.ContainerId == ctnId);
                    UnitWork.Save();
                });
            }
            else if(userIds.Count==0)
            {
                await UnitWork.ExecuteTransactionAsync(() =>
                {
                    UnitWork.RemoveMany<UserContainer>(x => x.ContainerId == ctnId);
                    UnitWork.Save();
                });
            }
            else
            {
                var entities = new List<UserContainer>();
                foreach (var userId in userIds)
                {
                    var entity = new UserContainer();
                    entity.ContainerId = ctnId;
                    entity.Type = type;
                    entity.UserId = userId;
                    entities.Add(entity);
                }

                await UnitWork.ExecuteTransactionAsync(() =>
                {
                    UnitWork.RemoveMany<UserContainer>(x => x.ContainerId == ctnId);
                    UnitWork.CreateMany<long, UserContainer>(entities.ToArray());
                    UnitWork.Save();
                });
            }
            return Consts.OK;

        }

        public async Task<string> UpdateUserContainersByTypeAndUserIdAsync(int type, string userId, List<string> ctnIds)
        {
            var entities = new List<UserContainer>();
            foreach (var ctnId in ctnIds)
            {
                var entity = new UserContainer();
                entity.ContainerId = ctnId;
                entity.Type = type;
                entity.UserId = userId;
                entities.Add(entity);
            }

            await UnitWork.ExecuteTransactionAsync(() =>
            {
                UnitWork.RemoveMany<UserContainer>(x => x.UserId == userId & x.Type == type);
                if (entities.Count > 0)
                    UnitWork.CreateMany<long, UserContainer>(entities.ToArray());
                UnitWork.Save();
            });

            return Consts.OK;

        }




    }
}