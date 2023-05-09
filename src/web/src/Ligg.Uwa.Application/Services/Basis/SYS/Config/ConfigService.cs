using Ligg.Infrastructure.Utilities.DataParserUtil;
using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class ConfigService
    {
        private readonly ConfigRepository _repository;
        private readonly ConfigItemRepository _configItemRepository;
        private readonly UserRepository _userRepository;
        private readonly CacheHandler _cacheHandler;
        public ConfigService(ConfigRepository repository, ConfigItemRepository configItemRepository, UserRepository userRepository)
        {
            _repository = repository;
            _configItemRepository = configItemRepository;
            _userRepository = userRepository;
            _cacheHandler = new CacheHandler();
        }

        //*list
        public async Task<List<ManageConfigsDto>> GetPagedManageSysConfigsAsync(int index, CommonReqArgs param, Pagination pagination)
        {
            int[] types = null;
            List<KeyValueDescription> subTypes = new List<KeyValueDescription>();
            if (index == (int)ConfigIndex.TntConfig)
            {
                types = EnumHelper.GetIds<TntConfigType>();
                subTypes = EnumHelper.EnumToKeyValueDescriptionList(typeof(TntConfigSubType));
            }
            else if (index == (int)ConfigIndex.DevConfig)
            {
                types = EnumHelper.GetIds<DevConfigType>();
                subTypes = EnumHelper.EnumToKeyValueDescriptionList(typeof(DevConfigSubType));
            }
            else if (index == (int)ConfigIndex.OrpConfig)
            {
                types = EnumHelper.GetIds<OrpConfigType>();
                subTypes = EnumHelper.EnumToKeyValueDescriptionList(typeof(OrpConfigSubType));
            }
            else if (index == (int)ConfigIndex.IndConfig)
            {
                types = EnumHelper.GetIds<IndConfigType>();
                subTypes = EnumHelper.EnumToKeyValueDescriptionList(typeof(OrpConfigSubType));
            }
            else if (index == (int)ConfigIndex.FuncConfig)
            {
                types = EnumHelper.GetIds<FuncConfigType>();
                subTypes = EnumHelper.EnumToKeyValueDescriptionList(typeof(FuncConfigSubType));
            }
            else return null;

            //var cfgItem= new ConfigHandler().GetFirstConfigItem(((int)OrpConfigSubType.ConfigOwner).ToString(), index.ToString());
            //var ownnerAccount = "Undefined";
            //if(cfgItem!=null)
            //{
            //    var ownnerId = cfgItem.Value;
            //    var user = await _userRepository.GetEntityByIdStringAsync(ownnerId);
            //    if (user != null) ownnerAccount = user.Account;
            //}

            //var cacheHandler = new CacheHandler();
            //var Pmses = cacheHandler.GetAllCachedPermissions().FindAll(x=>x.MasterId== index.ToString()&(x.Type==(int)PermissionType.GrantAsManagerForConfig| x.Type == (int)PermissionType.GrantAsProducerForSysConfig));
            //var ManagerPmses = Pmses.FindAll(x=>x.Type == (int)PermissionType.GrantAsManagerForConfig);
            //var OperatorPmses = Pmses.FindAll(x=>x.Type == (int)PermissionType.GrantAsProducerForSysConfig);
            //if (cfgItem != null)
            //{
            //    var ownnerId = cfgItem.Value;
            //    var user = await _userRepository.GetEntityByIdStringAsync(ownnerId);
            //    if (user != null) ownnerAccount = user.Account;
            //}

            var list = new List<ManageConfigsDto>();
            foreach (var subType in subTypes)
            {
                var manageConfigsDto = new ManageConfigsDto();
                var id = Convert.ToInt32(subType.Key);
                manageConfigsDto.Id = id.ToString();
                manageConfigsDto.Code = subType.Value;
                manageConfigsDto.Name = subType.Description;
                var type = types.Where(x => id > x - 1 & id < x + 100).FirstOrDefault();
                manageConfigsDto.Type = type;
                manageConfigsDto.Sequence = id;
                manageConfigsDto.Status = 1;
                list.Add(manageConfigsDto);
            }
            var dtos = list.MapToList<ManageConfigsDto>();
            var expression = DynamicExpressionEx.True<ManageConfigsDto>();
            if (param != null)
            {

                if (param.Type > -1)
                {
                    expression = expression.And(x => x.Type == param.Type);
                }

                if (!string.IsNullOrEmpty(param.Text) & GlobalContext.SystemSetting.SupportMultiLanguages)
                {
                    expression = expression.And(x => x.Name.Contains(param.Text) | x.Description.Contains(param.Text));
                }

            }
            var dtos1 = dtos.AsQueryable().Where(expression).OrderBy(x => x.Sequence);
            var dtos2 = CommonHelper.GetPagedList(dtos1.ToList(), pagination);

            var cacheHandler = new CacheHandler();
            var permissions = cacheHandler.GetAllCachedPermissions().FindAll(x => x.Type == (int)PermissionType.GrantAsManagerForConfig | x.Type == (int)PermissionType.GrantAsProducerForSysConfig);
            var managerPermissions = permissions.FindAll(x => x.Type == (int)PermissionType.GrantAsManagerForConfig & x.MasterId != (int)ConfigIndex.CustConfig + "");
            var producerPermissions = permissions.FindAll(x => x.Type == (int)PermissionType.GrantAsProducerForSysConfig);
            var configOwnercfgItem = new ConfigHandler().GetFirstConfigItem(((int)OrpConfigSubType.ConfigOwner).ToString(), ConfigIndex.CustConfig.ToString());
            var ownnerAccount = "Undefined";
            if (configOwnercfgItem != null)
            {
                var ownnerId = configOwnercfgItem.Attribute1;
                var user = await _userRepository.GetEntityByIdStringAsync(ownnerId);
                if (user != null) ownnerAccount = user.Account;
            }
            foreach (var dto in dtos2)
            {
                dto.OwnerAccount = ownnerAccount;
                var managerPermissions1 = managerPermissions.FindAll(x=>x.MasterId== index+"");
                dto.ManagerNum = managerPermissions1.Count;

                var ProducerPermissions1 = producerPermissions.FindAll(x => x.MasterId == dto.Type+"");
                dto.ProducerNum = ProducerPermissions1.Count;
            }

            return dtos2;
        }

        public async Task<List<ManageConfigsDto>> GetPagedManageCustConfigsAsync(CommonReqArgs param, Pagination pagination)
        {
            var exp = GetListFilter(param);
            var types = EnumHelper.GetIds<CustConfigType>();
            var list = _cacheHandler.GetAllCachedConfigs().Where(x => types.Contains(x.Type)).ToList();
            var list1 = list.AsQueryable().Where(exp).OrderBy(x => x.Sequence).ToList();
            list1 = CommonHelper.GetPagedList(list1, pagination);
            var dtos = list1.MapToList<ManageConfigsDto>();

            var cacheHandler = new CacheHandler();
            var permissions = cacheHandler.GetAllCachedPermissions().FindAll(x => x.Type == (int)PermissionType.GrantAsManagerForConfig | x.Type == (int)PermissionType.GrantAsProducerForCustConfig);
            var managerPermissions = permissions.FindAll(x => x.Type == (int)PermissionType.GrantAsManagerForConfig& x.MasterId == (int)ConfigIndex.CustConfig + "");
            var producerPermissions = permissions.FindAll(x => x.Type == (int)PermissionType.GrantAsProducerForCustConfig);
            var configOwnerCfgItem = new ConfigHandler().GetFirstConfigItem(((int)OrpConfigSubType.ConfigOwner).ToString(), ConfigIndex.CustConfig.ToString());
            var ownnerAccount = "Undefined";
            if (configOwnerCfgItem != null)
            {
                var ownnerId = configOwnerCfgItem.Attribute1;
                var user = await _userRepository.GetEntityByIdStringAsync(ownnerId);
                if (user != null) ownnerAccount = user.Account;
            }
            foreach (var dto in dtos)
            {
                dto.OwnerAccount = ownnerAccount;
                dto.ManagerNum = managerPermissions.Count;
                var producerPermissions1 = producerPermissions.FindAll(x=>x.MasterId==dto.Id);
                dto.ProducerNum = producerPermissions1.Count;
            }
            return dtos;
        }

        //*get
        public async Task<AddEditConfigDto> GetAddEditDtoAsync(string id)
        {
            var entity = _cacheHandler.GetAllCachedConfigs().Where(x => x.Id.ToString() == id).FirstOrDefault();
            var dto = entity.MapTo<AddEditConfigDto>();

            return dto;
        }

        public async Task<Config> GetEntityByIdStringAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var allList = _cacheHandler.GetAllCachedConfigs();
            var ett = allList.Find(x => x.Id.ToString() == id);
            return ett;
        }

        public async Task<int> GetMaxSequenceNoAsync(int type)
        {
            var allList = _cacheHandler.GetAllCachedConfigs();
            var maxOne = CommonHelper.GetMaxOne(allList, x => x.Sequence, x => x.Type == type);
            var step = GlobalContext.SystemSetting.GetMaxSequenceStep;
            var no = maxOne == null ? step : (((maxOne.Sequence ?? 0) + step));
            return no;
        }

        //*save
        public async Task<string> SaveAddEditDtoAsync(AddEditConfigDto dto)
        {
            var entity = dto.MapTo<Config>();
            var allList = _cacheHandler.GetAllCachedConfigs();
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Code, X => X.Code, null, true)) return "编码已经存在";
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Name, X => X.Name, null, true)) return "名称已经存在";

            entity.Description = entity.Description ?? string.Empty;
            var msg=await _repository.SaveEntityAsync(entity);
            if (msg == Consts.OK)
                _cacheHandler.RemoveConfigCache();
            return msg;
        }

        //*del
        public async Task<string> DeleteByIdsAsync(string ids)
        {
            var idArr = ids.Split(',');
            var itemList = await _configItemRepository.GetEntityByExpressionAsync(x => idArr.Contains(x.MasterId));
            if (itemList != null) return "子项存在，请先删除子项！";
            await _repository.DeleteEntitiesByIdArrayAsync(idArr);
            _cacheHandler.RemoveConfigCache();
            return Consts.OK;

        }

        private static Expression<Func<Config, bool>> GetListFilter(CommonReqArgs param)
        {
            var expression = DynamicExpressionEx.True<Config>();
            if (param != null)
            {
                if (param.Status > -1)
                {
                    expression = expression.And(x => x.Status == (param.Status == 1 ? true : false));
                }

                if (param.Type > -1)
                {
                    expression = expression.And(x => x.Type == param.Type);
                }

                if (!string.IsNullOrEmpty(param.Text) & !GlobalContext.SystemSetting.SupportMultiLanguages)
                {
                    expression = expression.And(x => x.Name.Contains(param.Text) | x.Description.Contains(param.Text));
                }

            }

            return expression;
        }

        //*init front-end
        public List<ConfigDetail> GetPortalConfigDetailsToFrontEnd(string portalId)
        {
            var configDetails = new List<ConfigDetail>();
            var commonTxtCfgDetails = ConvertRelatedSystematicConfigsToConfigDetails(((int)DevConfigSubType.CommonText).ToString());
            configDetails.AddRange(commonTxtCfgDetails);
            return configDetails;
        }
        public List<ConfigDetail> GetPageConfigDetailsToFrontEnd(string pageId)
        {
            var configDetails = new List<ConfigDetail>();
            if (pageId.IsNullOrEmpty()) return configDetails;

            var allCfgItems = _cacheHandler.GetAllCachedConfigItems().FindAll(x => x.Status);
            var page = allCfgItems.Find(x => x.MasterId.ToLower() == ((int)DevConfigSubType.Action).ToString() & x.Attribute.ToLower()==ActionType.Page.ToString().ToLower()&x.Id.ToString()==pageId);
            if (page == null) return configDetails;
            var dict = page.Value.ConvertLdictToDictionary(true, true, true);
            var enumCodeOrIds = dict.GetLdictValue("Enums");
            if (!enumCodeOrIds.IsNullOrEmpty())
            {
                var enumConfgiDetails = ConvertRelatedSystematicConfigsToConfigDetails(enumCodeOrIds);
                configDetails.AddRange(enumConfgiDetails);
            }
            //var dict = page.Value.ConvertLdictToDictionary(true, true, true);
            var fieldkeyOrIds = dict.GetLdictValue("Fields");
            if (!fieldkeyOrIds.IsNullOrEmpty())
            {
                //*doing
                var keyOrIdArr = fieldkeyOrIds.GetLarrayArray(true, true);
                if (keyOrIdArr != null)
                {
                    var cfgItems = allCfgItems.FindAll(x => x.Id.ToString() == ((int)DevConfigSubType.Vocabulary).ToString() & keyOrIdArr.Contains(x.Id.ToString()) | keyOrIdArr.Contains(x.Key));
                    var cfgDetails = cfgItems.MapToList<ConfigDetail>();
                    configDetails.AddRange(cfgDetails);
                }
            }

            var permissionHandler = new PermissionHandler();
            var removeButtons = permissionHandler.GetUnavailableMvcAdminPageButtons(page);
            configDetails.AddRange(removeButtons);

            return configDetails;
        }

        private List<ConfigDetail> ConvertRelatedSystematicConfigsToConfigDetails(string masterIdsOrCodes)
        {
            var configDetails = new List<ConfigDetail>();
            var allCfgs = _repository.GetAllSysConfigs();
            var allCfgItems = _cacheHandler.GetAllCachedConfigItems().FindAll(x => x.Status);

            if (!masterIdsOrCodes.IsNullOrEmpty())
            {
                var masterIdOrCodesArr = masterIdsOrCodes.GetLarrayArray(true, true);
                if (masterIdOrCodesArr != null)
                {
                    foreach (var relatedMaterIdOrCode in masterIdOrCodesArr)
                    {
                        var relatedCfg = allCfgs.Find(x => x.Id.ToString() == relatedMaterIdOrCode | x.Code.ToLower() == relatedMaterIdOrCode.ToLower());
                        if (relatedCfg != null)
                        {
                            var relatedConfigItems = allCfgItems.FindAll(x => x.MasterId == relatedCfg.Id.ToString());
                            var relatedConfigDetails = relatedConfigItems.MapToList<ConfigDetail>();

                            foreach (var relatedConfigDetail in relatedConfigDetails)
                            {
                                //relatedConfigDetail.Code = relatedCfg.Code;
                                relatedConfigDetail.Type = relatedCfg.Type;
                            }
                            configDetails.AddRange(relatedConfigDetails);
                        }

                    }
                }
            }
            return configDetails;
        }



    }
}
