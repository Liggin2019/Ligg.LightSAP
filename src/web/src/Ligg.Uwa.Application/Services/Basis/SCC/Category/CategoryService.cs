using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Ligg.Uwa.Basis.SCC
{
    public class CategoryService
    {
        private readonly CategoryRepository _repository;
        private readonly ArticleRepository _articleRepositoryy;
        private readonly CacheHandler _cacheHandler;
        public CategoryService(CategoryRepository repository, ArticleRepository articleRepository)
        {
            _repository = repository;
            _articleRepositoryy = articleRepository;
            _cacheHandler = new CacheHandler();
        }

        //*manage
        public async Task<List<ManageCategoriesDto>> GetManageDtosAsync(CommonReqArgs param)
        {
            var allEtts = GetAllCachedEntitiesByType(param.Type ?? 0);

            var list = new List<Category>();
            var exp = GetListFilter(param);
            list = allEtts.AsQueryable().Where(exp).OrderBy(x => x.Sequence).ToList();
            var dtos = list.MapToList<ManageCategoriesDto>();
            return dtos;
        }

        //*list
        public async Task<List<TreeItem>> GetListDtoTreeByParentId(string parentId, bool includeParent, int type = -1)//for menuZtree modal view
        {
            var dtos = await GetEnabledRecursiveListDtosByParenIdAsync(parentId, includeParent, type);
            var treeInfoList = new List<TreeItem>();
            foreach (var dto in dtos)
            {
                var treeItem = new TreeItem();
                treeItem.id = dto.Id;
                treeItem.pId = dto.ParentId;
                treeItem.name = dto.Name;
                treeInfoList.Add(treeItem);
            }

            if (includeParent & parentId == new Category().Id.ToString())
            {
                var rootTreeItem = new TreeItem();
                rootTreeItem.id = parentId;
                rootTreeItem.name = "根目录";
                treeInfoList.Add(rootTreeItem);
            }

            return treeInfoList;
        }

        //*get
        public async Task<AddEditCategoryDto> GetAddEditDtoAsync(int type, string id)
        {
            var obj = await GetEntityByTypeAndIdStringAsync(type, id);
            var dto = obj.MapTo<AddEditCategoryDto>();
            if (dto != null)
            {
                var parentId = dto.ParentId;
                if (parentId == new Category().Id.ToString())
                {
                    dto.ParentName = "根目录";
                }
                else
                {
                    Category parent = await GetEntityByTypeAndIdStringAsync(type, parentId);
                    if (parent != null)
                    {
                        dto.ParentName = parent.Name;
                    }
                }
            }
            return dto;
        }

        public async Task<string> GetCascadedPathById(int type, string id)
        {
            if (id == new Category().Id.ToString()) return null;
            var ett = await GetEntityByTypeAndIdStringAsync(type, id);
            var allEtts = GetAllCachedEntitiesByType(ett.Type);
            return CommonHelper.GetCascadedPathById<Category, long>(allEtts, id);

        }

        public async Task<string> GetTopIdByTypeAndIdStringAsync(int type, string id)
        {
            if (id == new Category().Id.ToString()) return null;
            var allEtts = GetAllCachedEntitiesByType(type);
            var ett = allEtts.Find(x => x.Id.ToString() == id);
            var parent = CommonHelper.GetTopParent<Category, long>(allEtts, ett);
            if (parent == null) return null;

            return parent.Id.ToString();
        }

        public async Task<Category> GetEntityByTypeAndIdStringAsync(int type, string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var allEtts = GetAllCachedEntitiesByType(type);
            var ett = allEtts.Find(x => x.Id.ToString() == id);
            return ett;
        }
        public async Task<int> GetMaxSequenceNoAsync(string parentId, int type)
        {
            var allEtts = GetAllCachedEntitiesByType(type);
            var maxOne = CommonHelper.GetMaxOne(allEtts, x => x.Sequence, x => x.ParentId == parentId & type == x.Type);
            var step = GlobalContext.SystemSetting.GetMaxSequenceStep;
            var no = maxOne == null ? step : (((maxOne.Sequence ?? 0) + step));
            return no;
        }

        //*save
        public async Task<string> SaveAddEditDtoAsync(AddEditCategoryDto dto, string dir = null)
        {
            var entity = dto.MapTo<Category>();
            var allEtts = GetAllCachedEntitiesByType(dto.Type);
            if (CommonHelper.FieldValueExists(allEtts, entity.Id, entity.Name, x => x.Name, x => x.ParentId == entity.ParentId & x.Type == entity.Type, true)) return "同目录下名称不能重复！";
            var msg = await _repository.SaveTreeEntityAsync(entity);
            if (msg != Consts.OK) return msg;

            RemoveCacheByType(dto.Type);

            return Consts.OK;
        }

        //*del
        public async Task<string> DeleteByIdStringAsync(int type, string id)
        {
            //var ett=_repository
            var msg = await _repository.DeleteTreeEntitiesByIdAsync<Category, long>(id, true);
            if (msg != Consts.OK) return msg;
            RemoveCacheByType(type);
            return Consts.OK;
        }

        //*common

        //for scc/article/manage
        public async Task<List<ListCategoriesDto>> GetEnabledRecursiveListDtosByParenIdAsync(string parentId, bool includeParent, int type)
        {
            var allEtts = GetAllCachedEntitiesByType(type);
            //if (type != -1) allList = allList.Where(x => x.Type == type).ToList();
            var children = CommonHelper.GetEnnabledRecursiveChildrenByParenId<Category, long>(allEtts, parentId, includeParent);
            children = children.OrderBy(x => x.Sequence).ToList();
            var dtos = new List<ListCategoriesDto>();
            if (children == null) return dtos;
            dtos = children.MapToList<ListCategoriesDto>();

            return dtos;

        }

        public async Task<List<ListCategoriesDto>> GetEnabledTopListDtosByTypeAsync(int type)
        {
            var allEtts = GetAllCachedEntitiesByType(type);
            var dtos = new List<ListCategoriesDto>();
            var etts = allEtts.Where(x => x.ParentId == new Category().Id.ToString() & x.Status == true & x.Type == type);
            var etts1 = etts.OrderBy(x => x.Sequence);
            dtos = etts1.MapToList<ListCategoriesDto>();
            return dtos;
        }

        public async Task<bool> CheckChildExistence(int type, string id, string dir)
        {
            var ett = await GetEntityByTypeAndIdStringAsync(type, id);
            if (ett.Type == (int)CategoryType.ArticleFolder)
            {
                var obj = await _articleRepositoryy.GetEntityByExpressionAsync(x => x.MasterId == id);
                if (obj != null) return true;
            }
            return false;
        }

        private List<Category> GetAllCachedEntitiesByType(int type)
        {
            if (type == (int)CategoryType.Tag) return _cacheHandler.GetAllCachedTags();
            else
            {
                var allDirs = _cacheHandler.GetAllCachedDirectories();
                return allDirs.FindAll(x=>x.Type==type); 
            }
        }

        private void RemoveCacheByType(int type)
        {
            if (type == (int)CategoryType.Tag) _cacheHandler.RemoveTagCache();
            else _cacheHandler.RemoveDirectoryCache();
        }
        private static Expression<Func<Category, bool>> GetListFilter(CommonReqArgs param)
        {
            var expression = DynamicExpressionEx.True<Category>();
            if (param != null)
            {
                if (param.Type > -1)
                {
                    expression = expression.And(x => x.Type == param.Type);
                }

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
