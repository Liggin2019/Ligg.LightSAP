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
    public class OrganizationService
    {
        private readonly OrganizationRepository _repository;
        private readonly UserRepository _userRepository;
        public OrganizationService(OrganizationRepository repository, UserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        //*manage
        public async Task<List<ManageOrganizationsDto>> GetManageDtosAsync(CommonReqArgs param)
        {
            var exp = GetListFilter(param);
            var lst = await _repository.FindEntitiesByExpressionAsync(exp);
            var dtos = lst.MapToList<ManageOrganizationsDto>();
            //dtos.OrderBy(x => x.Sequence).ToList();//no use
            dtos =dtos.OrderBy(x => x.Sequence).ToList();

            var headIds = lst.Select(x=>x.Owner).Distinct().Where(x=>x!=null);
            if (headIds.Count() == 0)
                return dtos;
            var heads = await _userRepository.FindEntitiesByExpressionAsync(x => headIds.Contains(x.Id.ToString()));
            foreach (var org in lst)
            {
                var dto = dtos.Find(x=>x.Id== org.Id.ToString());
                var head = heads.FirstOrDefault(x => x.Id.ToString() == org.Owner);
                dto.OwnerName = head == null ? "" :
                (dto.OwnerName = head.Name ?? "");
            }
           
            return dtos;
        }

        //*list
        public async Task<List<TreeItem>> GetListDtoTreeByParenId(string parentId, bool includeParent)
        {
            var list = await GetEnabledRecursiveListDtosByParenIdAsync(parentId, includeParent);
            var treeInfoList = new List<TreeItem>();
            foreach (var menu in list)
            {
                treeInfoList.Add(new TreeItem
                {
                    id = menu.Id.ToString(),
                    pId = menu.ParentId,
                    name = menu.Name
                });
            }
            return treeInfoList;
        }

        //*get
        public async Task<AddEditOrganizationDto> GetAddEditDtoAsync(string id)
        {
            var entity = await _repository.GetEntityByIdStringAsync(id);
            var dto = entity.MapTo<AddEditOrganizationDto>();
            if (dto != null)
            {
                var parentId = dto.ParentId;
                if (parentId==new Organization().ToString())
                {
                    dto.ParentName = "根目录";
                }
                else
                {
                    var parent = await _repository.GetEntityByIdStringAsync(parentId);
                    if (parent != null)
                    {
                        dto.ParentName = parent.Name;
                    }
                }
            }
            return dto;
        }

        public async Task<Organization> GetEntityByIdStringAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var entity = await _repository.GetEntityByIdStringAsync(id);
            return entity;
        }
        public async Task<int> GetMaxSequenceNoAsync(string parentId)
        {
            var maxOne = await _repository.GetMaxOneAsync(x => x.Sequence, x => x.ParentId == parentId);
            var step = GlobalContext.SystemSetting.GetMaxSequenceStep;
            var no = maxOne == null ? step : (((maxOne.Sequence ?? 0) + step));
            return no;
        }

        //*save
        public async Task<string> SaveAddEditDtoAsync(AddEditOrganizationDto dto)
        {
            var entity = dto.MapTo<Organization>();
            if (_repository.FieldValueExists(entity.Id, entity.Name, x => x.Name, x=>x.ParentId== entity.ParentId, true)) return "名称已经存在！";
            if(entity.Owner != null)
            {
                var head = await _userRepository.GetEntityByIdStringAsync(entity.Owner);
                if (head == null) return "负责人不存在";
            }

            entity.Description = entity.Description ?? string.Empty;
            var msg = await _repository.SaveTreeEntityAsync(entity);
            if (msg != Consts.OK) return msg;
            return msg;
        }

        //*del
        public async Task<string> DeleteByIdAsync(string id)
        {
            var org = await _repository.GetEntityByIdStringAsync(id);
            var toDelOrgIds = new List<string>();
            toDelOrgIds.AddRange(_repository.GetRecursiveChildIdsById<Organization,long>(org.Id.ToString(), true));

            var users = await _userRepository.FindEntitiesByExpressionAsync(x => toDelOrgIds.Contains(x.MasterId));
            if (users.Count() > 0) return "组织或下阶子组织存在用户,不能删除";

            var msg = await _repository.DeleteTreeEntitiesByIdAsync<Organization,long>(id, true);
            return msg;
        }


        //*private
        private async Task<List<ListOrganizationsDto>> GetEnabledRecursiveListDtosByParenIdAsync(string parentId, bool includeParent)
        {
            var allList = _repository.GetRecursiveChildrenById<Organization,long>(parentId, includeParent);
            var children = CommonHelper.GetEnnabledRecursiveChildrenByParenId<Organization,long>(allList, parentId, includeParent);

            var dtos = children.MapToList<ListOrganizationsDto>();
            return dtos;

        }

        private static Expression<Func<Organization, bool>> GetListFilter(CommonReqArgs param)
        {
            var expression = DynamicExpressionEx.True<Organization>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.Text) & !GlobalContext.SystemSetting.SupportMultiLanguages)
                {
                    expression = expression.And(x => x.Name.Contains(param.Text) | x.Description.Contains(param.Text));
                }

                if (param.Status > -1)
                {
                    expression = expression.And(x => x.Status == (param.Status == 1));
                }
                if (param.LongType >-1)
                {
                    expression = expression.And(x => x.Type== param.LongType.ToString());
                }
            }

            return expression;
        }

    }
}
