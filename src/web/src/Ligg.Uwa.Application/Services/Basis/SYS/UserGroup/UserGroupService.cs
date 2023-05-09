using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class UserGroupService
    {
        private readonly UserGroupRepository _repository;
        private readonly UserContainerRepository _userContainerRepository;
        private readonly UserRepository _userRepository;
        private readonly CacheHandler _cacheHandler;
        public UserGroupService(UserGroupRepository repository, UserContainerRepository userContainerRepository, UserRepository userRepository)
        {
            _repository = repository;
            _userContainerRepository = userContainerRepository;
            _userRepository = userRepository;
            _cacheHandler = new CacheHandler();
        }

        //*list
        public async Task<List<ManageUserGroupsDto>> GetPagedManageDtosAsync(CommonReqArgs param, Pagination pagination)
        {
            var exp = GetListFilter(param);
            var etts = await _repository.GetEntitiesByTypeAsync(param.Type ?? -1, true);
            var etts1 = etts.AsQueryable().Where(exp).OrderBy(x => x.Sequence).ToList();
            var etts2 = CommonHelper.GetPagedList<UserGroup>(etts1, pagination);
            var dtos = etts2.MapToList<ManageUserGroupsDto>();

            var ids = etts2.Select(x => x.Id.ToString()).ToArray();
            var userCtns = new List<UserContainer>();
            if (ids != null)
                userCtns = await _userContainerRepository.FindEntitiesByExpressionAsync(x => ids.Contains(x.ContainerId));
            if (userCtns.Count > 0)
            {
                foreach (var dto in dtos)
                {
                    var userCtns1 = userCtns.FindAll(x => x.ContainerId == dto.Id);
                    dto.UserNum = userCtns1.Count;
                }
            }
            return dtos;
        }

        //*test
        public async Task<List<GroupDict>> GetListDtoGroupDictsByType(int type, bool includeBuiltin = true)
        {
            var lst = await GetListDtosByTypeAsync(type, includeBuiltin);
            var dicts = new List<GroupDict>();

            var roleTypes = EnumHelper.GetIds<RoleType>();
            foreach (var tp in roleTypes)
            {
                var grpDict = new GroupDict();
                grpDict.Key = "请选择" + tp;
                grpDict.Value = new List<DictItem>();
                foreach (var dto in lst.Where(x => x.Builtin == tp))
                {
                    var subDictItem = new DictItem(dto.Id.ToString(), dto.Name);
                    grpDict.Value.Add(subDictItem);
                }
                dicts.Add(grpDict);
            }
            return dicts;
        }

        public async Task<List<DictItem>> GetListDtoDictItemsByType(int type, bool includeBuiltin = true)
        {
            var lst = await GetListDtosByTypeAsync(type, includeBuiltin);
            var dict = new List<DictItem>();
            foreach (var dto in lst)
            {
                dict.Add(new DictItem(dto.Id.ToString(), dto.Name));
            }
            return dict;
        }

        public async Task<string> GetIdsStringByTypeAndUserIdAsync(int type, string userId)
        {
            var list = await _userContainerRepository.FindEntitiesByExpressionAsync(x => x.UserId == userId & x.Type == type);
            if (list.Count() == 0) return string.Empty;
            return string.Join(",", list.Select(p => p.ContainerId));
        }

        public async Task<string> GetUserIdsStringByTypeAndIdAsync(int type, string id)
        {
            var list = await _userContainerRepository.FindEntitiesByExpressionAsync(x => x.ContainerId == id & x.Type == type);
            if (list.Count() == 0) return string.Empty;
            return string.Join(",", list.Select(p => p.UserId));
        }

        //*get
        public async Task<AddEditUserGroupDto> GetAddEditDtoAsync(string id)
        {
            var entity = await _repository.GetEntityByIdStringAsync(id);
            var dto = entity.MapTo<AddEditUserGroupDto>();
            return dto;

        }
        public async Task<UserGroup> GetEntityByIdStringAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var entity = await _repository.GetEntityByIdStringAsync(id);
            return entity;
        }
        public async Task<int> GetMaxSequenceNoAsync(int type)
        {
            var maxOne = await _repository.GetMaxOneAsync(x => x.Sequence, x => x.Type == type);
            var step = GlobalContext.SystemSetting.GetMaxSequenceStep;
            var no = maxOne == null ? step : (((maxOne.Sequence ?? 0) + step));
            return no;
        }

        //*save
        public async Task<string> SaveAddEditDtoAsync(AddEditUserGroupDto dto)
        {
            var entity = dto.MapTo<UserGroup>();

            dto.Description = dto.Description ?? string.Empty;
            return await _repository.SaveEntityAsync(entity);
        }

        //*update
        public async Task<string> UpdateContainedUsers(RelatedIdsDto dto, int type)
        {
            var grpId = dto.Id;
            var userIds = dto.RelatedIds;
            if (userIds.IsNullOrEmpty())
            {
                return await _userContainerRepository.UpdateUserContainersByTypeAndCtnIdAsync(type, grpId, null);
            }

            var userIdArr = userIds.Split<long>(',');
            var users = await _userRepository.FindEntitiesByExpressionAsync(x => userIdArr.Contains(x.Id));
            if (users.Count == 0) return Consts.OK;
            var userIdList = users.Select(x => x.Id.ToString()).ToList();

            return await _userContainerRepository.UpdateUserContainersByTypeAndCtnIdAsync(type, grpId, userIdList);
        }

        //*del
        public async Task<string> DeleteByIdsAsync(int type, string ids)
        {
            await _repository.DeleteByIdsAsync(type, ids);
            _cacheHandler.RemovePermissionCache();
            return Consts.OK;
        }



        //*private
        private async Task<List<ListUserGroupsDto>> GetListDtosByTypeAsync(int type, bool includeBuiltin = true)
        {
            var dtos = new List<ListUserGroupsDto>();
            if (type == (int)UserGroupType.Role & includeBuiltin)
            {
                var builtinobjs = EnumHelper.EnumToKeyValueDescriptionList(typeof(BuiltinRole));
                var ct = 0;
                foreach (var builtinObj in builtinobjs)
                {
                    var builtinDto = new ListUserGroupsDto();
                    builtinDto.Id = builtinObj.Key;
                    builtinDto.Name = builtinObj.Description;
                    builtinDto.Builtin = 1;
                    builtinDto.Description = builtinObj.Description;
                    dtos.Add(builtinDto);
                    //builtinDto.Status = 1;
                    //builtinDto.Sequence = ct;
                    //builtinDto.ModificationTime = DateTime.Now;
                    ct++;
                }
            }
            var list = await _repository.FindEntitiesByExpressionAsync(x => x.Type == type & x.Status);
            var dtosDb = list.MapToList<ListUserGroupsDto>();
            dtos.AddRange(dtosDb);
            return dtos.OrderBy(x=>x.Sequence).ToList();
        }

        private static Expression<Func<UserGroup, bool>> GetListFilter(CommonReqArgs param)
        {
            var expression = DynamicExpressionEx.True<UserGroup>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.Text))
                {
                    expression = expression.And(x => x.Name.Contains(param.Text));
                }

                if (param.Type > -1)
                {
                    expression = expression.And(x => x.Type == param.Type);
                }

                if (param.Status > -1)
                {
                    expression = expression.And(x => x.Status == (param.Status == 1 ? true : false));
                }

            }

            return expression;
        }

    }
}
