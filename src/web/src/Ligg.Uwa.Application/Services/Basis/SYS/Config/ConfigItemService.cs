
using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class ConfigItemService
    {
        private readonly ConfigItemRepository _repository;
        private readonly CacheHandler _cacheHandler;
        //private string _cacheKey => Consts.CACHEKEY_CFG_ITM_LST;
        public ConfigItemService(ConfigItemRepository repository)
        {
            _repository = repository;
            _cacheHandler = new CacheHandler();
        }

        //*list
        public async Task<List<ManageConfigItemsDto>> GetPagedManageDtosAsync(ExpressionBuilderReqArgs param, Pagination pagination)
        {
            //var allList = _cacheHandler.GetAllCachedConfigItems();//*test can't get from cache because dynamic expression
            var list = await _repository.GetPagedEntitiesAsync(param, pagination);
            var dtos = list.MapToList<ManageConfigItemsDto>();
            var permissionHandler = new PermissionHandler();
            foreach (var dto in dtos)
            {
                dto.ConsumerNum = permissionHandler.GetPermissionHeadCount(PermissionType.GrantAsConsumerForConfigItem, dto.Id.ToString());
            }
            return dtos;
        }

        //*get
        public async Task<AddEditConfigItemDto> GetAddEditDtoAsync(string id)
        {
            var entity = await GetEntityByIdStringAsync(id);
            var dto = entity.MapTo<AddEditConfigItemDto>();
            return dto;
        }
        public async Task<ConfigItem> GetEntityByIdStringAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var allList = _cacheHandler.GetAllCachedConfigItems();
            var ett = allList.Find(x => x.Id.ToString() == id);
            return ett;
        }


        //*save
        public async Task<string> SaveAddEditDtoAsync(AddEditConfigItemDto dto)
        {
            var entity = dto.MapTo<ConfigItem>();
            bool isEdit = false;
            var oldEtt = new ConfigItem();
            if (entity.Id != new ConfigItem().Id)
            {
                isEdit = true;
                oldEtt = _cacheHandler.GetAllCachedConfigItems().Find(x => x.Id.ToString() == dto.Id);
            }

            var allList = _cacheHandler.GetAllCachedConfigItems();

            if (Convert.ToInt64(entity.MasterId) > (int)ConfigIndex.FuncConfig * 1000 + 90 * 1000 + 9999) //10*1000+90*1000+9999
            {
                var allCfgList = _cacheHandler.GetAllCachedConfigs();
                var type = allCfgList.Find(x => x.Id.ToString() == entity.MasterId).Type;
                var sameTypeMasterIds = allCfgList.Where(x => x.Type == type).Select(x => x.Id.ToString());
                if (sameTypeMasterIds.Count() > 0)
                {
                    var sameTypeCfgItems = allList.Where(x => sameTypeMasterIds.Contains(x.MasterId)).ToList();
                    if (sameTypeCfgItems.Count() > 0)
                    {
                        if (CommonHelper.FieldValueExists(sameTypeCfgItems, entity.Id, entity.Key, x => x.Key, null, true)) return "同类型下键已经存在";
                    }
                }
            }
            else
            {
                if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Key, x => x.Key, x => x.MasterId == entity.MasterId, true)) return "同编码下键已经存在";
            }
            entity.Attribute = entity.Attribute ?? string.Empty;
            entity.Attribute1 = entity.Attribute1 ?? string.Empty;
            entity.Value = entity.Value ?? string.Empty;
            entity.Authorization = entity.Authorization;
            entity.PermissionMark = entity.PermissionMark ?? string.Empty;
            entity.Description = entity.Description ?? string.Empty;
            if (isEdit) entity.IsDefault = oldEtt.IsDefault;
            var msg = await _repository.SaveEntityAsync(entity);
            if (msg == Consts.OK)
                _cacheHandler.RemoveConfigItemCache();
            return msg;
        }

        public async Task<string> UpdateEntityAsync(ConfigItem entity)
        {
            var msg = await _repository.SaveEntityAsync(entity);
            if (msg == Consts.OK)
                _cacheHandler.RemoveConfigItemCache();
            return msg;
        }

        public async Task<string> SetAsDefault(string id)
        {
            await _repository.SetAsDefault(id);
            _cacheHandler.RemoveConfigItemCache();
            return Consts.OK;
        }

        //*del
        public async Task<string> DeleteByIdsAsync(string ids)
        {
            //**need to check if has reference by others
            var idArr = ids.Split(',');
            await _repository.DeleteEntitiesByIdArrayAsync(idArr);
            _cacheHandler.RemoveConfigItemCache();
            return Consts.OK;
        }

        //*sequence
        public async Task<int> GetMaxSequenceNoAsync(string masterId)
        {
            var allList = _cacheHandler.GetAllCachedConfigItems();
            var maxOne = CommonHelper.GetMaxOne(allList, x => x.Sequence, x => x.MasterId == masterId);
            var step = GlobalContext.SystemSetting.GetMaxSequenceStep;
            var no = maxOne == null ? step : (((maxOne.Sequence ?? 0) + step));
            return no;
        }

        //*private

        public bool IsSameMaster(string[] idArr)
        {
            var etts = _cacheHandler.GetAllCachedConfigItems().FindAll(x => idArr.Contains(x.Id.ToString()));
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
