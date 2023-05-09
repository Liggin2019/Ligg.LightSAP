
using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class PermissionService
    {
        private readonly PermissionRepository _repository;
        private readonly ConfigItemRepository _configItemRepository;
        private readonly OperatorRepository _operatorRepository;
        private readonly UserGroupRepository _userGroupRepository;
        private readonly UserRepository _userRepository;
        private readonly CacheHandler _cacheHandler;
        public PermissionService(PermissionRepository repository, ConfigItemRepository configItemRepository, OperatorRepository operatorRepository, UserGroupRepository userGroupRepository, UserRepository userRepository)
        {
            _repository = repository;
            _configItemRepository = configItemRepository;
            _operatorRepository = operatorRepository;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
            _cacheHandler = new CacheHandler();
        }

        //*list
        public async Task<List<ManagePermissionsDto>> GetPagedManageDtosAsync(ListPermissionsReqArgs param, Pagination pagination)
        {
            var dtos = new List<ManagePermissionsDto>();
            var allEtts = _cacheHandler.GetAllCachedPermissions();
            var exp = ListPermissionsReqArgs.GetListFilter(param);
            var filteredEtts = allEtts.AsQueryable().Where(exp);

            var filteredDtos = filteredEtts.MapToList<ManagePermissionsDto>();
            dtos = CommonHelper.GetPagedList(filteredDtos.ToList(), pagination);

            if (dtos.Count() > 0)
            {
                var usrAuthGrpIds = allEtts.Where(x => x.ActorType == (int)ActorType.AuthorizationGroup).Select(x => x.ActorId);
                var usrAuthGrps = await _userGroupRepository.FindEntitiesByExpressionAsync(x => usrAuthGrpIds.Contains(x.Id.ToString()));
                var roleIds = allEtts.Where(x => x.ActorType == (int)ActorType.Role).Select(x => x.ActorId);
                var allRoles = await _userGroupRepository.GetEntitiesByTypeAsync((int)ActorType.Role, true);
                var roles = allRoles.Where(x => roleIds.Contains(x.Id.ToString())).ToList();
                var userIds = allEtts.Where(x => x.ActorType == (int)ActorType.User).Select(x => x.ActorId);
                var users = await _userRepository.FindEntitiesByExpressionAsync(x => userIds.Contains(x.Id.ToString()));

                var creatorIds = allEtts.Select(x => x.CreatorId);
                var oprtors = await _operatorRepository.FindEntitiesByExpressionAsync(x => creatorIds.Contains(x.Id.ToString()));
                var creatorUserIds = oprtors.Where(x=>x.Type==(int)ActorType.User).Select(x=>x.ActorId.ToString());
                var creatorUsers = await _userRepository.FindEntitiesByExpressionAsync(x => creatorUserIds.Contains(x.Id.ToString()));
                foreach (var dto in dtos)
                {
                    var ett = allEtts.Find(x => x.Id.ToString() == dto.Id);
                    if (dto.ActorType == (int)ActorType.Role)
                    {
                        var usrGrp = roles.Find(x => x.Id.ToString() == ett.ActorId);

                        dto.ActorName = usrGrp != null ? usrGrp.Name : "Undefined";
                    }
                    else if (dto.ActorType == (int)ActorType.AuthorizationGroup)
                    {
                        var usrGrp = usrAuthGrps.Find(x => x.Id.ToString() == ett.ActorId);
                        dto.ActorName = usrGrp != null ? usrGrp.Name : "xxx";
                    }
                    else if (dto.ActorType == (int)ActorType.User)
                    {
                        var usr = users.Find(x => x.Id.ToString() == ett.ActorId);
                        dto.ActorName = usr == null ? "Undefined" : usr.Name;
                        dto.ActorAccount = usr == null ? "Undefined" : usr.Account;
                    }

                    var userOprtor = oprtors.Find(x => x.Type==(int)ActorType.User&x.Id.ToString() == ett.CreatorId);
                    var creatorUser = creatorUsers.Find(x => x.Id.ToString() == userOprtor.ActorId);
                    var creatorAccount = creatorUser==null?"Undefined": creatorUser.Account;
                    if (creatorAccount.Contains("-D"))
                    {
                        var index = creatorAccount.IndexOf("-D");
                        creatorAccount = creatorAccount.Substring(0, index + 2);
                    };
                    dto.CreatorAccount = creatorAccount;
                }
            }

            return dtos;
        }

        //*get

        public async Task<string> GetActorIdsByTypeMasterIdAndActorType(int type, string masterId, int actorType)
        {
            var list = await _repository.FindEntitiesByExpressionAsync(x => x.Type == type & x.MasterId == masterId & x.ActorType == actorType);
            if (list.Count() == 0) return string.Empty;
            return string.Join(",", list.Select(x => x.ActorId));

        }
        public async Task<Permission> GetEntityByIdStringAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var entity = _cacheHandler.GetAllCachedPermissions().Where(x => x.Id.ToString() == id).FirstOrDefault();
            return entity;

        }

        //*save
        public async Task<string> AddAsync(int type, string masterId, int actorType, RelatedIdsDto dto)
        {

            string[] actorIdArr = null;
            if (actorType == ((int)ActorType.User))
            {
                var actorIds = dto.RelatedIds;
                if (actorIds.IsNullOrEmpty()) return Consts.OK;
                actorIdArr = actorIds.Split(',');
                var users = await _userRepository.FindEntitiesByExpressionAsync(x => actorIdArr.Contains(x.Id.ToString()));
                if (users.Count == 0) return Consts.OK;
                var userIdList = users.Select(x => x.Id.ToString()).ToArray();
                actorIdArr = userIdList;
            }
            else//usergrp
            {
                var actorIds = dto.RelatedIds;
                if (actorIds.IsNullOrEmpty()) return Consts.OK;
                actorIdArr = actorIds.Split(',');
            }
            return await _repository.AddAsync(type, masterId, actorType, actorIdArr);
        }

        //*del
        public async Task<string> DeleteByIdsAsync(string ids)
        {
            var idArr = ids.Split(',');
            await _repository.DeleteEntitiesByIdArrayAsync(idArr);
            _cacheHandler.RemovePermissionCache();
            return Consts.OK;

        }


        //*private
        private static Expression<Func<ManagePermissionsDto, bool>> GetListFilter(ListPermissionsReqArgs param)
        {
            var expression = DynamicExpressionEx.True<ManagePermissionsDto>();
            if (param != null)
            {
                if (param.Type > -1)
                {
                    expression = expression.And(x => x.Type == param.Type);
                }
            }

            return expression;
        }

    }
}
