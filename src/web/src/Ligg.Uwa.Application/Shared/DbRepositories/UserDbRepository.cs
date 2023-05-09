using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Utilities.DbUtil;
using Ligg.Infrastructure.Utilities.LogUtil;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using System;
using System.Linq;

namespace Ligg.Uwa.Application.Shared
{
    public class UserDbRepository
    {
        private readonly string _creatorId;
        public UserDbRepository()
        {
            var crtOprt = CurrentOperator.Instance.GetCurrent();
            var initCreatorId = new User().Id.ToString();
            _creatorId = crtOprt == null ? initCreatorId : crtOprt.Id;
        }
        public OperatorInfo GetOperatorInfo(string id)
        {
            var sql = "select * from sys_operators where Id=" + id;
            var oprt = DbHelper.Get<Operator>(sql);//ok 
            if (oprt == null) return null;

            var oprtInfo = oprt.MapTo<OperatorInfo>();
            var masterId = "";
            var account = "";
            var name = "";
            var roleIdsStr = "";
            var authIdsStr = "";
            var thumbnailPostfix = "";
            var ownner = "";
            var creationTime = DateTime.MinValue;
            if (oprt.Type == (int)ActorType.User)
            {
                sql = @"select * from sys_users where Id=" + oprt.ActorId;
                var user = DbHelper.Get<User>(sql);//null
                account = user.Account;
                name = user.Name;

                if (user.MasterId != new Organization().Id.ToString())
                {
                    sql = @"select * from sys_organizations where Id=" + user.MasterId;
                    var org = DbHelper.Get<Organization>(sql);
                    if (org.CascadedNo.Split('.').Length == 3) masterId = org.Id.ToString();
                    else
                    {
                        var no = org.CascadedNo;
                        var index = no.IndexOf('.', 3);
                        var topParentNo = no.Substring(0, index) + '.';
                        sql = @"select * from sys_organizations where CascadedNo=" + "\"" + topParentNo + "\"";
                        var topParent = DbHelper.Get<Organization>(sql);//null
                        masterId = topParent.Id.ToString();
                    }
                }
                sql = @"select ContainerId from sys_usercontainers where Type=" + (int)UserGroupType.Role + " and UserId=" + "\"" + user.Id + "\"";
                var roles = DbHelper.FindMany<UserContainer>(sql);
                if (roles.Count() > 0)
                {
                    var roleIdList = roles.Select(x => x.ContainerId);
                    roleIdsStr = string.Join(" ", roleIdList);
                }
                sql = @"select ContainerId from sys_usercontainers where Type=" + (int)UserGroupType.AuthorizationGroup + " and UserId=" + "\"" + user.Id + "\"";
                var authGrps = DbHelper.FindMany<UserContainer>(sql);
                if (authGrps.Count() > 0)
                {
                    var authGrpIdList = authGrps.Select(x => x.ContainerId);
                    authIdsStr = string.Join(" ", authGrpIdList);
                }
                thumbnailPostfix = user.ThumbnailPostfix;
                creationTime = user.CreationTime;
                ownner = user.Id.ToString();
            }
            oprtInfo.Owner = ownner;
            oprtInfo.MasterId = masterId;
            oprtInfo.Account = account;
            oprtInfo.Name = name;

            oprtInfo.RoleIds = roleIdsStr;
            oprtInfo.AuthorizationGroupIds = authIdsStr;

            oprtInfo.IsMachine = oprt.Type == (int)ActorType.Machine;
            oprtInfo.IsRoot = account == "root";
            var roleIdArr = roleIdsStr.GetLarrayArray(true, true);
            if (roleIdArr != null)
                oprtInfo.IsAdministrator = roleIdArr.Contains(((int)BuiltinRole.Administrator).ToString());
            oprtInfo.IsSuper = (oprtInfo.IsAdministrator | oprtInfo.IsRoot);

            oprtInfo.ThumbnailPostfix = thumbnailPostfix;
            oprtInfo.CreationTime = creationTime;
            return oprtInfo;
        }

        public User GetUserByCredential(string credential)
        {
            var sql = @"select * from sys_users where Account= " + "\"" + credential + "\"" + " or Mobile= " + "\"" + credential + "\"" + " or Email= " + "\"" + credential + "\"";
            var user = DbHelper.Get<User>(sql);//null
            return user;
        }

        public Operator GetOperatorByActorId(string actorId)
        {
            var sql = @"select * from sys_operators where Type= " + (int)ActorType.User + " and ActorId= " + "\"" + actorId + "\"";
            var oprt = DbHelper.Get<Operator>(sql);//null
            return oprt;
        }

        public void UpdateOperator(Operator oprt)
        {
            oprt.LastModifierId = _creatorId;
            oprt.ModificationTime = DateTime.Now;
            try
            {
                DbHelper.Update<Operator>(oprt);//no good
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }


    }
}




