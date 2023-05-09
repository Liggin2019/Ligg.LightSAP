using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly OperatorRepository _operatorRepository;
        private readonly OrganizationRepository _organizationRepository;
        private readonly UserContainerRepository _userContainerRepository;
        private readonly UserGroupRepository _userGroupRepository;
        private readonly EntryLogRepository _entryLogRepository;
        private readonly CacheHandler _cacheHandler;
        public UserService(UserRepository repository, OperatorRepository OperatorRepository, OrganizationRepository organizationRepository,
            UserGroupRepository userGroupRepository, UserContainerRepository userContainerRepository, EntryLogRepository entryLogRepository)
        {
            _repository = repository;
            _operatorRepository = OperatorRepository;
            _organizationRepository = organizationRepository;
            _userContainerRepository = userContainerRepository;
            _userGroupRepository = userGroupRepository;
            _entryLogRepository = entryLogRepository;
            _cacheHandler = new CacheHandler();
        }

        //*manage
        public async Task<List<ManageUsersDto>> GetPagedManageDtosAsync(ListUsersReqArgs param, Pagination pagination)
        {
            var dtos = new List<ManageUsersDto>();
            if (param?.MasterId != null)
            {
                if (param.RecursiveSearch == 1)
                    param.RecursiveChildMasterIds = _organizationRepository.GetRecursiveChildIdsById<Organization, long>(param.MasterId, true);

                var exp = ListUsersReqArgs.GetListFilter(param);
                var etts = await _repository.FindPagedEntitiesAsync(exp, pagination);
                dtos = etts.MapToList<ManageUsersDto>();
                var userIdArray = dtos.Select(x => x.Id).ToArray();
                foreach (var dto in dtos)
                {
                    dto.MasterCascadePath = _organizationRepository.GetCascadedPathById<Organization>(dto.MasterId);
                    var ett = etts.Where(x => x.Id.ToString() == dto.Id).FirstOrDefault();
                    dto.HasThumbnail = ett.ThumbnailPostfix.IsNullOrEmpty() ? 0 : 1;
                }
                return dtos;
            }
            else
            {
                return dtos;
            }
        }

        //*list
        public async Task<List<TreeItem>> GetOrganizationAndSubUserTreeByOrgId(string orgId, bool includeParentOrg)
        {
            var orgList = _organizationRepository.GetRecursiveChildrenById<Organization, long>(orgId, includeParentOrg);

            if (orgList.Count() == 0) return null;
            orgList = orgList.AsQueryable().OrderBy(x => x.Sequence).ToList();

            var orgIdArray = orgList.Select(x => x.Id.ToString()).ToList().ToArray();
            var userList = await _repository.FindEntitiesByExpressionAsync(x => orgIdArray.Contains(x.MasterId) & x.Status & x.Undeleted);

            var treeItemInfos = new List<TreeItem>();
            foreach (var org in orgList)
            {
                treeItemInfos.Add(new TreeItem
                {
                    id = org.Id.ToString(),
                    pId = org.ParentId,
                    name = org.Name
                });

                foreach (var user in userList.Where(x => x.MasterId == org.Id.ToString()))
                {
                    treeItemInfos.Add(new TreeItem
                    {
                        id = user.Id.ToString(),
                        pId = user.MasterId,
                        name = user.Name
                    });
                }
            }

            return treeItemInfos;
        }

        //*get
        public async Task<AddEditUserDto> GetAddEditDtoAsync(string id)
        {
            var obj = await _repository.GetEntityByIdStringAsync(id);
            var dto = obj.MapTo<AddEditUserDto>();
            dto.OrganizationPath = _organizationRepository.GetCascadedPathById<Organization>(obj.MasterId);

            //var roleIdsStr = await _userContainerRepository.GetContainerIdsStringByUserIdAsync(id, (int)UserGroupType.Role);
            //dto.RoleIds = roleIdsStr;
            return dto;
        }
        public async Task<GetSelfModel> GetShowModelAsync(string id)
        {
            var obj = await _repository.GetEntityByIdStringAsync(id);
            var mdl = obj.MapTo<GetSelfModel>();

            mdl.GenderName = "";
            if (obj.Gender == 1)
            {
                mdl.GenderName = "男";
            }
            else if (obj.Gender == 2)
            {
                mdl.GenderName = "女";
            }

            if (obj.MasterId != new Organization().Id.ToString())
                mdl.OrganizationPath = _organizationRepository.GetCascadedPathById<Organization>(obj.MasterId);
            var usrContainers = await _userContainerRepository.FindEntitiesByExpressionAsync(x => x.UserId == id);
            var userGrpIdArr = usrContainers.Select(x => x.ContainerId);
            var usrGroups = await _userGroupRepository.FindEntitiesByExpressionAsync(x => userGrpIdArr.Contains(x.Id.ToString()));


            var roles = usrGroups.FindAll(x => x.Type == (int)UserGroupType.Role);
            if (roles.Count > 0)
            {
                mdl.RoleNames = roles.Select(x => x.Name).ToArray().Unwrap(", ");
                var buildInRoles = EnumHelper.EnumToKeyValueDescriptionList(typeof(BuiltinRole));
                var roleIdArr = usrContainers.Where(x => x.Type == (int)UserGroupType.Role).Select(x => x.ContainerId);
                foreach (var roleId in roleIdArr)
                {
                    var buildInRole = buildInRoles.Find(x => x.Key == roleId);
                    if (buildInRole != null) mdl.RoleNames = mdl.RoleNames + ", " + buildInRole.Description;
                }

            }

            var authGrps = usrGroups.FindAll(x => x.Type == (int)UserGroupType.AuthorizationGroup);
            if (authGrps.Count > 0)
                mdl.AuthGroupNames = authGrps.Select(x => x.Name).ToArray().Unwrap(", ");

            var commGrps = usrGroups.FindAll(x => x.Type == (int)UserGroupType.CommunicationGroup);
            if (commGrps.Count > 0)
                mdl.CommGroupNames = commGrps.Select(x => x.Name).ToArray().Unwrap(", ");

            return mdl;
        }
        public async Task<User> GetEntityByIdStringAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var entity = await _repository.GetEntityByIdStringAsync(id);
            return entity;
        }


        public async Task<string> SaveAddEditDtoAsync(AddEditUserDto dto)
        {
            if (dto.MasterId == new Organization().Id.ToString()) return "根目录不能关联数据";

            dto.Email = dto.Email ?? string.Empty;
            dto.Mobile = dto.Mobile ?? string.Empty;
            dto.Qq = dto.Qq ?? string.Empty;
            dto.WeChat = dto.WeChat ?? string.Empty;
            dto.Description = dto.Description ?? string.Empty;
            return await _repository.SaveAddEditDtoAsync(dto);
        }
        public async Task<string> UpdateEntityAsync(User entity)
        {
            return await _repository.SaveEntityAsync(entity);
        }

        public async Task<string> SaveEntryLog(EntryLog entryLog)
        {
            await _entryLogRepository.CreateEntityAsync(entryLog);

            return Consts.OK;
        }


        //*update
        public async Task<string> ChangeSelfPassword(ChangePasswordDto dto)
        {
            return await _repository.ChangeSelfPassword(dto);
        }

        public async Task<string> UpdateBelongedUserGroups(RelatedIdsDto dto, int type)
        {
            var userId = dto.Id;
            var usrGrpIds = dto.RelatedIds;
            var usrGrpIdList = new List<string>();
            if (!usrGrpIds.IsNullOrEmpty())
            {
                var usrGrpIdArr = usrGrpIds.Split<long>(',');
                var usrGrpEtts = await _userGroupRepository.GetEntitiesByTypeAsync(type);
                var usrGrps = usrGrpEtts.FindAll(x => usrGrpIdArr.Contains(x.Id));
                if (usrGrps.Count > 0)
                    usrGrpIdList = usrGrpIdList = usrGrps.Select(x => x.Id.ToString()).ToList();
            }

            return await _userContainerRepository.UpdateUserContainersByTypeAndUserIdAsync(type, userId, usrGrpIdList);
        }
        public async Task<string> ResetUserPassword(ChangePasswordDto dto)
        {
            return await _repository.ResetUserPasswordAsync(dto);
        }

        public async Task<string> UpdateOperatorAsync(Operator oprt)
        {
            await _operatorRepository.UpdateEntityAsync(oprt);
            return Consts.OK;

        }

        //*del
        public async Task<string> DeleteByIdsAsync(string ids)
        {
            return await _repository.DeleteByIdsAsync(ids);
            _cacheHandler.RemovePermissionCache();
            return Consts.OK;
        }

        public async Task<List<User>> GetEntitiesByIdsAsync(string[] ids)
        {
            var etts = await _repository.FindEntitiesByIdArrayAsync(ids);
            return etts;
        }

        //*check
        public async Task<TResult<Operator>> Login(string credential, string password, int webClientType)
        {
            TResult<Operator> rst = new TResult<Operator>();

            if (credential.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                rst.Message = "登录凭证不能为空";
                return rst;
            }
            var userRepo = new UserDbRepository();
            //var user = await GetEntityByCredential(credential);
            var user = userRepo.GetUserByCredential(credential);

            if (user != null)
            {
                //var oprtor = await _operatorRepository.GetEntityByExpressionAsync(x => x.Type == (int)ActorType.User & x.ActorId == user.Id.ToString());
                var oprtor = userRepo.GetOperatorByActorId(user.Id.ToString());
                if(oprtor==null)
                {
                    rst.Message = "操作者信息为空";
                }
                else if (user.Status == true)
                {
                    if (user.Password == _repository.EncryptPassword(password, user.Salt))
                    {
                        oprtor.LoginCount++;
                        if (oprtor.FirstVisit == null)
                        {
                            oprtor.FirstVisit = DateTime.Now;
                        }
                        if (oprtor.PreviousVisit == null)
                        {
                            oprtor.PreviousVisit = DateTime.Now;
                        }
                        //if (oprtor.LastVisit != DateTime.MinValue)
                        if (oprtor.LastVisit !=null)
                        {
                            oprtor.PreviousVisit = oprtor.LastVisit;
                        }
                        oprtor.LastVisit = DateTime.Now;
                        
                        rst.Data = oprtor;
                        rst.Message = "登录成功";
                        rst.Flag = 1;

                    }
                    else
                    {
                        rst.Message = "密码错误，请重新输入";
                    }
                }
                else
                {
                    rst.Message = "账号被禁用，请联系管理员";
                }
            }
            else
            {
                rst.Message = "账号不存在或密码错误，请重新输入";
            }

            return rst;
        }

        public bool IsSameMaster(string[] idArr)
        {
            var etts = _repository.FindEntitiesByIdArrayAsync(idArr).Result;
            if (etts.Count == 0) return false;
            var masterId = etts.FirstOrDefault().MasterId;
            foreach (var ett in etts)
            {
                if (masterId != ett.MasterId)
                    return false;
            }
            return true;
        }


    }
}