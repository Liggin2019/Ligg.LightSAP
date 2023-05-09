using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Ligg.Uwa.Basis.SYS
{
    public class MenuItemService
    {
        private readonly MenuItemRepository _repository;
        private readonly CacheHandler _cacheHandler;
        public MenuItemService(MenuItemRepository repository)
        {
            _repository = repository;
            _cacheHandler = new CacheHandler();
        }

        //*manage
        public async Task<List<ManageMenuItemsDto>> GetManageDtosAsync(CommonReqArgs param)
        {
            var allList = _cacheHandler.GetAllCachedMenuItems();
            var list = new List<MenuItem>();
            var exp = GetListFilter(param);
            list = allList.AsQueryable().Where(exp).OrderBy(x => x.Sequence).ToList();
            var dtos = list.MapToList<ManageMenuItemsDto>();
            var topDtos = dtos.FindAll(x => x.ParentId == new MenuItem().Id.ToString());
            var cfgHandler = new ConfigHandler();
            var menuTypes = cfgHandler.GetConfigItems(((int)OrpConfigSubType.MenuDefinition).ToString());
            foreach (var topDto in topDtos)
            {
                topDto.MenuType = (int)MenuType.Undefined;
                var menuType = menuTypes.Find(x => x.Attribute == topDto.Id);
                if (menuType != null)
                {
                    topDto.TopParentId = topDto.Id;
                    var clientTypeName = menuType.Attribute1;
                    var clientTypeId = (int)EnumHelper.GetByName(clientTypeName, MenuType.Undefined);
                    topDto.MenuType = clientTypeId;
                }
                ChangeChildrenClientTyped(topDto.Id, topDto.Id, topDto.MenuType, dtos);
            }
            return dtos;
        }
        private void ChangeChildrenClientTyped(string parentId, string topParentId, int menuTypeId, List<ManageMenuItemsDto> dtos)
        {
            var children = dtos.FindAll(x => x.ParentId == parentId);
            foreach (var child in children)
            {
                child.TopParentId = topParentId;
                child.MenuType = menuTypeId;
                ChangeChildrenClientTyped(child.Id, topParentId, menuTypeId, dtos);
            }
        }

        //*list
        public async Task<List<TreeItem>> GetListDtoTreeByParentId(string parentId, int type, bool includeParent)//for menuZtree modal view
        {
            var dtos = await GetEnabledRecursiveListDtosByParenIdAsync(parentId, type, includeParent);
            var treeInfoList = new List<TreeItem>();
            foreach (var dto in dtos)
            {
                var menuZtree = new TreeItem();
                menuZtree.id = dto.Id.ToString();
                menuZtree.pId = dto.ParentId;
                menuZtree.name = dto.Name;
                treeInfoList.Add(menuZtree);
            }

            if (includeParent & parentId == new MenuItem().Id.ToString())
            {
                var topMenuZtree = new TreeItem();
                topMenuZtree.id = parentId;
                topMenuZtree.name = "根目录";
                treeInfoList.Add(topMenuZtree);
            }

            return treeInfoList;
        }

        //*get
        public async Task<AddEditMenuItemDto> GetAddEditDtoAsync(string id)
        {
            var obj = await GetEntityByIdStringAsync(id);
            var dto = obj.MapTo<AddEditMenuItemDto>();
            if (dto != null)
            {
                var parentId = dto.ParentId;
                if (parentId == new MenuItem().Id.ToString())
                {
                    dto.ParentName = "根目录";
                }
                else
                {
                    MenuItem parentMenu = await _repository.GetEntityByIdStringAsync(parentId);
                    if (parentMenu != null)
                    {
                        dto.ParentName = parentMenu.Name;
                    }
                }
            }
            return dto;
        }
        public async Task<MenuItem> GetEntityByIdStringAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var allList = _cacheHandler.GetAllCachedMenuItems();
            var ett = allList.Find(x => x.Id.ToString() == id);
            return ett;
        }
        public async Task<int> GetMaxSequenceNo(string parentId)
        {
            var allList = _cacheHandler.GetAllCachedMenuItems();
            var maxOne = CommonHelper.GetMaxOne(allList, x => x.Sequence, x => x.ParentId == parentId);
            var step = GlobalContext.SystemSetting.GetMaxSequenceStep;
            var no = maxOne == null ? step : (((maxOne.Sequence ?? 0) + step));
            return no;
        }

        //*private


        //*save
        public async Task<string> SaveAddEditDtoAsync(AddEditMenuItemDto dto)
        {
            var entity = dto.MapTo<MenuItem>();
            var allList = _cacheHandler.GetAllCachedMenuItems();
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Name, x => x.Name, x => x.ParentId == entity.ParentId, true)) return "名称已经存在！";
            if (!entity.Code.IsNullOrEmpty())
                if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Code, x => x.Code, x => x.ParentId == entity.ParentId, true)) return "编码已经存在！";
            var rootId = new MenuItem().Id.ToString();
            if (entity.Id == new MenuItem().Id)//add 
            {
                if (entity.ParentId != rootId)
                {
                    var parent = await GetEntityByIdStringAsync(entity.ParentId);
                    if (parent.Type != (int)MenuItemType.Directory) return "上级类别只能是目录！";
                }
                if (entity.Type == (int)MenuItemType.OuterLink) entity.IsBlankTarget = true;
            }
            else
            {
                var oldEntity = await GetEntityByIdStringAsync(entity.Id.ToString());
                if (oldEntity.Type == (int)MenuItemType.Directory)
                {
                    if (entity.Type != (int)MenuItemType.Directory) return "修改类别有误";
                }
                else
                {
                    if (entity.Type == (int)MenuItemType.Directory) return "修改类别有误！";
                }
            }

            if (entity.Type != (int)MenuItemType.Directory)
            {
                if (entity.PageId == null) return "页面Id不能为空！";
            }
            if (entity.Type == (int)MenuItemType.OuterLink)
            {
                entity.IsBlankTarget = true;
            }

            entity.Description = entity.Description ?? string.Empty;
            entity.Style = entity.Style ?? string.Empty;
            entity.Code = entity.Code ?? string.Empty;
            entity.Redirect = entity.Redirect ?? string.Empty;
            var msg = await _repository.SaveTreeEntityAsync(entity);
            if (msg != Consts.OK) return msg;
            _cacheHandler.RemoveMenuItemCache();

            return Consts.OK;
        }

        //*del
        public async Task<string> DeleteByIdStringAsync(string id)
        {
            var msg = await _repository.DeleteTreeEntitiesByIdAsync<MenuItem, long>(id, true);
            if (msg != Consts.OK) return msg;
            _cacheHandler.RemoveMenuItemCache();

            return Consts.OK;
        }

        //*common
        //for home/index
        public async Task<List<MenuItem>> GetEnabledRecursiveEntitiesByParenIdAsync(string parentId, bool includeParent)
        {
            var allList = _cacheHandler.GetAllCachedMenuItems();
            var children = CommonHelper.GetEnnabledRecursiveChildrenByParenId<MenuItem, long>(allList, parentId, includeParent);
            children = children.OrderBy(x => x.Sequence).ToList();
            return children;
        }

        //for sys/menu/manage
        public async Task<List<ListMenuItemsDto>> GetEnabledRecursiveListDtosByParenIdAsync(string parentId, int type, bool includeParent)
        {
            var allList = _cacheHandler.GetAllCachedMenuItems();
            var children = CommonHelper.GetEnnabledRecursiveChildrenByParenId<MenuItem, long>(allList, parentId, includeParent);
            children = children.OrderBy(x => x.Sequence).ToList();
            var dtos = children.MapToList<ListMenuItemsDto>();
            dtos = dtos.FindAll(x => x.Type == type);
            return dtos;

        }


        private static Expression<Func<MenuItem, bool>> GetListFilter(CommonReqArgs param)
        {
            var expression = DynamicExpressionEx.True<MenuItem>();
            if (param != null)
            {
                if (param.Status > -1)
                {
                    expression = expression.And(x => x.Status == (param.Status == 1 ? true : false));
                }

                if (!string.IsNullOrEmpty(param.Text) & !GlobalContext.SystemSetting.SupportMultiLanguages)
                {
                    expression = expression.And(x => x.Name.Contains(param.Text) | x.Description.Contains(param.Text));
                }
            }

            return expression;
        }



    }
}
