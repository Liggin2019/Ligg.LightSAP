using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class UserGroupRepository : CommonRepository<long, UserGroup, DbSetContext>
    {
        public UserGroupRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, UserGroup, DbSetContext> repository) : base(unitWork, repository)
        {
        }

        //*list
        public async Task<List<UserGroup>> GetEntitiesByTypeAsync(int type, bool includeBuiltin = true)
        {
            var etts = new List<UserGroup>();
            if (type == (int)UserGroupType.Role & includeBuiltin)
            {
                var builtinObjs = EnumHelper.EnumToKeyValueDescriptionList(typeof(BuiltinRole));
                var ct = 0;
                foreach (var builtinObj in builtinObjs)
                {
                    var builinEtt = new UserGroup();
                    builinEtt.Type = type;
                    builinEtt.Id = System.Convert.ToInt32(builtinObj.Key);
                    builinEtt.Name = builtinObj.Description;
                    builinEtt.Description = builtinObj.Description;
                    builinEtt.Status = true;
                    builinEtt.Sequence = ct;
                    builinEtt.CreationTime = DateTime.Now;
                    builinEtt.ModificationTime = DateTime.Now;

                    builinEtt.Builtin = true;
                    etts.Add(builinEtt);
                    ct++;
                }
            }
            var ettsDb = await FindEntitiesByExpressionAsync(x => x.Type == type);
            etts.AddRange(ettsDb);
            return etts;
        }


        //*del
        public async Task<string> DeleteByIdsAsync1(string ids)
        {
            var idArr = ids.Split<long>(',');
            var isBuiltin=await Repository.AnyAsync(x=> idArr.Contains(x.Id));
            if(isBuiltin) return "内置角色不能删除";
            //remove relevance
            UnitWork.RemoveMany<Permission>(x => x.ActorType==(int)ActorType.Role | x.ActorType == (int)ActorType.AuthorizationGroup);
            UnitWork.RemoveMany<UserGroup>(u => idArr.Contains(u.Id));
            return Consts.OK;


        }

        public async Task<string> DeleteByIdsAsync(int type, string ids)
        {
            await UnitWork.ExecuteTransactionAsync(() =>
            {
                var idsArr = ids.Split(',');

                UnitWork.RemoveMany<UserContainer>(x => idsArr.Contains(x.ContainerId) & x.Type == type);
                UnitWork.RemoveMany<Permission>(x => idsArr.Contains(x.ActorId) & x.ActorType ==type);
                UnitWork.RemoveMany<UserGroup>(x => idsArr.Contains(x.Id.ToString()));
                UnitWork.Save();
            });
            return Consts.OK;
        }



    }
}