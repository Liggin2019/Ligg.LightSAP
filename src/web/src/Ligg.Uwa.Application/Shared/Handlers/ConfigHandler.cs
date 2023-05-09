using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Basis.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using Ligg.Infrastructure.Utilities.DataParserUtil;


namespace Ligg.Uwa.Application.Shared
{
    public class ConfigHandler
    {
        private readonly CacheHandler _cacheHandler;
        //private readonly PermissionHandler _permissionHandler;
        private readonly List<ConfigItem> _configItems;
        private readonly List<Config> _custConfigs;
        public ConfigHandler()
        {
            _cacheHandler = new CacheHandler();
            _custConfigs = _cacheHandler.GetAllCachedConfigs().FindAll(x => x.Status);
            _configItems = _cacheHandler.GetAllCachedConfigItems().FindAll(x => x.Status);
        }

        //*public
        public List<Config> GetCustConfigs()
        {
            return _custConfigs.OrderBy(x=>x.Sequence).ToList();
        }
        public ConfigDefinition GetConfigDefinition(string subTypeId)
        {
            var cfgFeature = GetConfigFeature(subTypeId);
            if (cfgFeature == null) throw new ArgumentException("cfgFeature is null, please check param of Config.SubType");
            var configDefinition = SaveConfigFeatureToConfigDefinition(cfgFeature);

            var isOwner = false;
            var ownerCfgItem = GetFirstConfigItem((int)OrpConfigSubType.ConfigOwner + "", configDefinition.IndexName);
            if (ownerCfgItem != null)
            {
                var owner = ownerCfgItem.Attribute1;
                var currentOprt = CurrentOperator.Instance.GetCurrent();
                if (currentOprt != null)
                {
                    if (currentOprt.IsRoot) isOwner = true;
                    else
                    {
                        if (currentOprt.ActorId == owner) isOwner = true;
                    }
                }
                else isOwner = false;
            }
            var isManager = false;
            var isProducer = false;
            var permissionHandler = new PermissionHandler();
            isManager = permissionHandler.IsManger(PermissionType.GrantAsManagerForConfig, configDefinition.Index.ToString());
            if (configDefinition.Index == (int)ConfigIndex.CustConfig)
                isProducer = permissionHandler.IsProducer(PermissionType.GrantAsProducerForCustConfig, configDefinition.SubType.ToString());
            else
                isProducer = permissionHandler.IsProducer(PermissionType.GrantAsProducerForSysConfig, configDefinition.Type.ToString());

            configDefinition.IsManager = isManager;
            configDefinition.IsProducer = isProducer;
            configDefinition.IsOwner = isOwner;

            var cfgDefinitionCfgItem = GetConfigItem((int)DevConfigSubType.SysConfigDefinition + "", cfgFeature.IndexName + "_" + cfgFeature.SubTypeName);
            if (cfgFeature.Index == (int)ConfigIndex.CustConfig)
                cfgDefinitionCfgItem = GetConfigItem((int)DevConfigSubType.CustConfigDefinition + "", cfgFeature.IndexName + "_" + cfgFeature.TypeName);
            if (cfgDefinitionCfgItem == null) return configDefinition;

            configDefinition.Attributes = new TextHandler().GetAtributes(cfgDefinitionCfgItem);

            var val = cfgDefinitionCfgItem.Value;
            var dict = val.ConvertLdictToDictionary(true, true, true);

            if (configDefinition.Index == (int)ConfigIndex.FuncConfig)
            {
                var hasConsumersStr = dict.GetLdictValue("HasConsumers");
                if (!hasConsumersStr.IsNullOrEmpty()) configDefinition.HasConsumers = hasConsumersStr.JudgeJudgementFlag();
                if (configDefinition.HasConsumers)
                {
                    configDefinition.AuthorizationOption = ShowHideOrNone.Hide;
                    configDefinition.AuthorizationValue = (int)Authorization.TobePermitted;
                    var consumerOptionStr = dict.GetLdictValue("ConsumerOption");
                    if (!consumerOptionStr.IsNullOrEmpty())
                        configDefinition.ConsumerOption = EnumHelper.GetByName<PermittedOperatorType>(consumerOptionStr);
                }
                else
                {
                    configDefinition.AuthorizationOption = ShowHideOrNone.Hide;
                    configDefinition.AuthorizationValue = (int)Authorization.AnyOne;
                }

            }
            return configDefinition;
        }

        private ConfigDefinition SaveConfigFeatureToConfigDefinition(ConfigFeature cfgFeature)
        {
            var configDefinition = new ConfigDefinition();
            configDefinition.Index = cfgFeature.Index;
            configDefinition.IndexName = cfgFeature.IndexName;
            configDefinition.Module = cfgFeature.Module;
            configDefinition.ModelName = cfgFeature.ModelName;
            configDefinition.Type = cfgFeature.Type;
            configDefinition.TypeName = cfgFeature.TypeName;
            configDefinition.SubType = cfgFeature.SubType;
            configDefinition.SubTypeName = cfgFeature.SubTypeName;

            configDefinition.Name = cfgFeature.Name;
            configDefinition.AuthorizationOption = cfgFeature.AuthorizationOption;
            configDefinition.AuthorizationValue = cfgFeature.AuthorizationValue;
            configDefinition.HasPermissionMark = cfgFeature.HasPermissionMark;
            configDefinition.HasConsumers = cfgFeature.HasConsumers;
            configDefinition.ConsumerOption = cfgFeature.ConsumerOption;

            return configDefinition;
        }

        public List<KeyValueDescription> GetSysConfigTypeKeyValueDescriptions(string cfgIndex)
        {
            var keyValueDescriptions = new List<KeyValueDescription>();
            if (cfgIndex == (int)ConfigIndex.TntConfig + "")
            {
                var keyValueDescriptionList1 = EnumHelper.EnumToKeyValueDescriptionList(typeof(TntConfigType));
                keyValueDescriptions.AddRange(keyValueDescriptionList1);
            }
            else if (cfgIndex == (int)ConfigIndex.DevConfig + "")
            {
                var keyValueDescriptionList2 = EnumHelper.EnumToKeyValueDescriptionList(typeof(DevConfigType));
                keyValueDescriptions.AddRange(keyValueDescriptionList2);
            }
            else if (cfgIndex == (int)ConfigIndex.OrpConfig + "")
            {
                var keyValueDescriptionList3 = EnumHelper.EnumToKeyValueDescriptionList(typeof(OrpConfigType));
                keyValueDescriptions.AddRange(keyValueDescriptionList3);
            }
            else if (cfgIndex == (int)ConfigIndex.FuncConfig + "")
            {
                var keyValueDescriptionList4 = EnumHelper.EnumToKeyValueDescriptionList(typeof(FuncConfigType));
                keyValueDescriptions.AddRange(keyValueDescriptionList4);
            }
            else if (cfgIndex == (int)ConfigIndex.IndConfig + "")
            {
                var keyValueDescriptionList5 = EnumHelper.EnumToKeyValueDescriptionList(typeof(IndConfigType));
                keyValueDescriptions.AddRange(keyValueDescriptionList5);
            }
            return keyValueDescriptions;
        }


        public List<ConfigItem> GetConfigItems(string masterId, string attribute = null, string attribute1 = null)
        {
            return GetConfigItemsByMasterId(masterId, attribute, attribute1);
        }

        public List<ConfigItem> GetCustConfigItems(int type, string masterId = null, string attribute = null, string attribute1 = null)
        {
            return GetCustConfigItemsByType(type, masterId, attribute, attribute1);
        }
        public ConfigItem GetFirstOrDefaultConfigItem(string masterId, string attribute = null, string attribute1 = null)
        {
            var defaultConfigItem = GetDefaultConfigItem(masterId, attribute, attribute1);
            if (defaultConfigItem != null) return defaultConfigItem;
            return GetFirstConfigItem(masterId, attribute, attribute1);
        }
        public ConfigItem GetFirstConfigItem(string masterId, string attribute = null, string attribute1 = null)
        {
            var configItems = GetConfigItemsByMasterId(masterId, attribute, attribute1);
            return configItems.FirstOrDefault();
        }
        public ConfigItem GetDefaultConfigItem(string masterId, string attribute = null, string attribute1 = null)
        {
            var cfgItems = GetConfigItemsByMasterId(masterId, attribute, attribute1);
            return cfgItems.Find(x => x.IsDefault);

        }
        public ConfigItem GetConfigItem(string masterId, string key)
        {
            var cfgItem = _configItems.Find(x => x.MasterId == masterId & x.Key == key);
            return cfgItem;
        }


        public ConfigItem GetCurrentAction(string url)
        {
            var applicationServerName = GlobalContext.SystemSetting.ApplicationServer;
            var crtls = GetConfigItems(((int)DevConfigSubType.Controller).ToString(), applicationServerName);
            var crtlKeys = crtls.Select(x => x.Key).ToArray();
            var actions = GetConfigItems(((int)DevConfigSubType.Action).ToString());
            actions = actions.FindAll(x => crtlKeys.ToLower().Contains(x.Attribute1.ToLower()));
           
            var action = actions.Find(x => x.Attribute2.ToLower() == url.ToLower());
            return action;
        }

        //*private
        private ConfigFeature GetConfigFeature(string subTypeId)
        {
            if (!subTypeId.IsPlusInteger()) return null;
            var masterIdLong = Convert.ToInt64(subTypeId);
            var index = 0;
            var cfgFeature = new ConfigFeature();
            cfgFeature.SubType = masterIdLong;
            if (masterIdLong > 999999)//CustConfig
            {
                index = 3;
                var cfg = _custConfigs.Find(x => x.Id == masterIdLong);
                if (cfg == null) return null;
                cfgFeature.Type = cfg.Type;
                cfgFeature.TypeName = EnumHelper.GetNameById<CustConfigType>(cfgFeature.Type);
                cfgFeature.SubTypeName = cfg.Name;
                cfgFeature.Name = cfg.Name;
            }
            else if (masterIdLong > 10000 - 1 & masterIdLong < 100000) //FuncConfig
            {
                index = 10;
                cfgFeature.Type = (int)masterIdLong / 100 * 100;
                cfgFeature.TypeName = EnumHelper.GetNameById<FuncConfigType>(cfgFeature.Type);
                cfgFeature.SubTypeName = EnumHelper.GetNameById<FuncConfigSubType>((int)cfgFeature.SubType);
                var enum1 = EnumHelper.GetByName<FuncConfigSubType>(cfgFeature.SubTypeName);
                cfgFeature.Name = EnumHelper.GetDescription(enum1);
            }
            else if (masterIdLong > 4000 - 1 & masterIdLong < 5000) //IndConfig
            {
                index = 4;
                cfgFeature.Type = (int)masterIdLong / 100 * 100;
                cfgFeature.TypeName = EnumHelper.GetNameById<IndConfigType>(cfgFeature.Type);
                cfgFeature.SubTypeName = EnumHelper.GetNameById<IndConfigSubType>((int)cfgFeature.SubType);
                var enum1 = EnumHelper.GetByName<IndConfigSubType>(cfgFeature.SubTypeName);
                cfgFeature.Name = EnumHelper.GetDescription(enum1);
            }
            else if (masterIdLong > 2000 - 1 & masterIdLong < 3000) //OrpConfig
            {
                index = 2;
                cfgFeature.Type = (int)masterIdLong / 100 * 100;
                cfgFeature.TypeName = EnumHelper.GetNameById<OrpConfigType>(cfgFeature.Type);
                cfgFeature.SubTypeName = EnumHelper.GetNameById<OrpConfigSubType>((int)cfgFeature.SubType);
                var enum1 = EnumHelper.GetByName<OrpConfigSubType>(cfgFeature.SubTypeName);
                cfgFeature.Name = EnumHelper.GetDescription(enum1);
            }
            else if (masterIdLong > 1000 - 1 & masterIdLong < 2000) //DevConfig
            {
                index = 1;
                cfgFeature.Type = (int)masterIdLong / 100 * 100;
                cfgFeature.TypeName = EnumHelper.GetNameById<DevConfigType>(cfgFeature.Type);
                cfgFeature.SubTypeName = EnumHelper.GetNameById<DevConfigSubType>((int)cfgFeature.SubType);
                var enum1 = EnumHelper.GetByName<DevConfigSubType>(cfgFeature.SubTypeName);
                cfgFeature.Name = EnumHelper.GetDescription(enum1);
            }
            else if (masterIdLong > 100 - 1 & masterIdLong < 1000) //TntConfig
            {
                index = 0;
                cfgFeature.Type = (int)masterIdLong / 100 * 100;
                cfgFeature.TypeName = EnumHelper.GetNameById<TntConfigType>(cfgFeature.Type);
                cfgFeature.SubTypeName = EnumHelper.GetNameById<TntConfigSubType>((int)cfgFeature.SubType);
            }
            else
            {
                return null;
            }

            cfgFeature.Index = index;
            cfgFeature.IndexName = EnumHelper.GetNameById<ConfigIndex>(index);

            cfgFeature.AuthorizationOption = ShowHideOrNone.None;
            cfgFeature.HasPermissionMark = false;
            cfgFeature.ConsumerOption = PermittedOperatorType.UserAndMachine;

            if (cfgFeature.SubType == (int)DevConfigSubType.ClientView | cfgFeature.SubType == (int)DevConfigSubType.ClientViewButton
              | cfgFeature.SubType == (int)DevConfigSubType.Action | cfgFeature.SubType == (int)DevConfigSubType.MvcAdminPageButton)
            {
                cfgFeature.AuthorizationOption = ShowHideOrNone.Show;
                cfgFeature.HasPermissionMark = true;
            }
            else if (cfgFeature.SubType == (int)OrpConfigSubType.MvcSitePortal | cfgFeature.SubType == (int)OrpConfigSubType.MvcAdminPortal)
            {
                cfgFeature.AuthorizationOption = ShowHideOrNone.Show;
                cfgFeature.HasConsumers =true;
            }
            else if (cfgFeature.SubType == (int)OrpConfigSubType.Operation)
            {
                cfgFeature.HasPermissionMark = true;
            }
            else if (cfgFeature.Type == (int)CustConfigType.Page | cfgFeature.Type == (int)CustConfigType.DataForm | cfgFeature.Type == (int)CustConfigType.WorkFlowForm)
            {
                cfgFeature.AuthorizationOption = ShowHideOrNone.Show;
                cfgFeature.HasConsumers = true;
            }

            return cfgFeature;
        }
        private List<ConfigItem> GetConfigItemsByMasterId(string masterId, string attribute = null, string attribute1 = null)
        {
            var cfgItems = new List<ConfigItem>();
            cfgItems = _configItems.FindAll(x => x.MasterId == masterId);
            if (!attribute.IsNullOrEmpty()) cfgItems = cfgItems.FindAll(x => x.Attribute.ToLower() == attribute.ToLower());
            if (!attribute1.IsNullOrEmpty()) cfgItems = cfgItems.FindAll(x => x.Attribute1.ToLower() == attribute1.ToLower());
            return cfgItems;
        }

        private List<ConfigItem> GetCustConfigItemsByType(int type, string masterId = null, string attribute = null, string attribute1 = null)
        {
            var cfgItems = new List<ConfigItem>();
            var cfgs = _custConfigs.FindAll(x => x.Type == type);
            if (cfgs.Count > 0)
            {
                if (masterId.IsNullOrEmpty())
                {
                    var masterIds = cfgs.Select(x => x.Id.ToString());
                    cfgItems = _configItems.FindAll(x => masterIds.Contains(x.MasterId));
                }
                else
                {
                    cfgItems = _configItems.FindAll(x => masterId == x.MasterId);

                }
                if (!attribute.IsNullOrEmpty()) cfgItems = cfgItems.FindAll(x => x.Attribute.ToLower() == attribute.ToLower());
                if (!attribute1.IsNullOrEmpty()) cfgItems = cfgItems.FindAll(x => x.Attribute1.ToLower() == attribute1.ToLower());
            }
            return cfgItems;
        }


    }
}
