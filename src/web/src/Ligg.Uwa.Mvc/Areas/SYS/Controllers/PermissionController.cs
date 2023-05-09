using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ligg.Uwa.Mvc.Controllers.SYS
{
    [Area("Sys")]
    public class PermissionController : BaseController
    {
        private readonly PermissionService _service;
        private readonly ConfigService _configService;
        private readonly UserGroupService _userGroupService;
        private readonly UserService _userService;
        public PermissionController(PermissionService permissionService, UserGroupService userGroupService, UserService userService, ConfigService configService)
        {
            _service = permissionService;
            _configService = configService;
            _userGroupService = userGroupService;
            _userService = userService;
        }

        //*page
        public IActionResult Manage()
        {
            var type = (int)PermissionType.Operate;
            var typeDictJson = typeof(PermissionType).EnumToDictionaryJson();

            var actorTypeNames = "AuthorizationGroup" + " User";
            var objectName= "操作";
            actorTypeNames = "Role" + " User";

            ViewBag.Type =type;
            ViewBag.TypeDictJson = typeDictJson;
            ViewBag.ActorTypeNames = actorTypeNames;
            ViewBag.ObjectId = "";
            ViewBag.ObjectName = objectName;

            return View("Permission/Manage");
        }
        public IActionResult ManageConfig(int index, string objId)
        {
            var errCode = GetErrorCode();
            var indexes = EnumHelper.GetIds<PermissionType>();
            if (!indexes.Contains(index)| index== (int)PermissionType.Operate) return Redirect("~/Home/Error/Wurl/index/" + index + "/" + errCode + "-1");

            var type = EnumHelper.GetById<PermissionType>(index, PermissionType.Operate);
            var typeDictJson = typeof(PermissionType).EnumToDictionaryJson();

            var actorTypeNames = "AuthorizationGroup" + " User";
            var objectName = "";
            if (type == PermissionType.GrantAsManagerForConfig)
            {
                objectName = "配置";
            }
            else if (type == PermissionType.GrantAsProducerForSysConfig)
            {
                var cfgIndexInt = Convert.ToInt32(objId);
                var cfg = EnumHelper.GetById(cfgIndexInt, ConfigIndex.DevConfig);
                var cfgDes = cfg.GetDescription();
                objectName = cfgDes + "类型";
            }
            else if (type == PermissionType.GrantAsProducerForCustConfig)
            {
                var cfgType = System.Convert.ToInt32(objId);
                var enum1 = EnumHelper.GetById(cfgType, CustConfigType.Page);
                objectName =enum1.GetDescription();
            }
            else if (type == PermissionType.GrantAsConsumerForConfigItem)
            {
                var cfgDefinition = new ConfigHandler().GetConfigDefinition(objId);
                actorTypeNames = new PermissionHandler().GetActorTypeNamesByPermittedOperatorType(cfgDefinition.ConsumerOption);
                objectName = cfgDefinition.Name;
            }
            else
            {
                return Redirect("~/Home/Error/Wurl/index/" + index + "/" + errCode + "-2");
            }

            ViewBag.Type = (int)type;
            ViewBag.TypeDictJson = typeDictJson;
            ViewBag.ActorTypeNames = actorTypeNames;
            ViewBag.ObjectId = objId;
            ViewBag.ObjectName = objectName;

            return View("Permission/Manage");
        }

        //*modal
        public IActionResult AddModal(int type, string masterId, int actorType)
        {
            if (actorType == (int)ActorType.User)
            {
                ViewBag.Url = "/Sys/Permission/GetActorTreeOrDictItemsJsonForAdd?actorType=" + actorType;
                var updateDataUrl = "/Sys/Permission/GetActorIdsStringJsonForAdd?type=" + type + "&masterId=" + masterId + "&actorType=" + actorType;
                ViewBag.UpdateDataUrl = updateDataUrl;
                return View("Selectors/RelatedIdsTreeSelector");
            }
            else
            {
                ViewBag.Url = "/Sys/Permission/GetActorTreeOrDictItemsJsonForAdd" + "?actorType=" + actorType;
                ViewBag.UpdateDataUrl = "/Sys/Permission/GetActorIdsStringJsonForAdd?type=" + type + "&masterId=" + masterId + "&actorType=" + actorType;
                return View("Selectors/RelatedIdsSelector");
            }
        }


        //*get
        [HttpGet]
        public async Task<IActionResult> GetPagedManageDtosJson(string objId, ListPermissionsReqArgs param, Pagination pagination)
        {
            var dtos = await _service.GetPagedManageDtosAsync(param, pagination);
            var keyValueDescriptions = GetMasterKeyValueDescriptionsByType(param.Type ?? -1, objId);
            if (keyValueDescriptions.Count > 0)
            {
                foreach (var dto in dtos)
                {
                    var keyValueDescription = keyValueDescriptions.Find(x => x.Key == dto.MasterId);
                    if (keyValueDescription != null) dto.MasterName = keyValueDescription != null ? (keyValueDescription.Value ?? "Undefined") : "Undefined";
                }
            }

            var rst = new TResult<List<ManagePermissionsDto>>(1, dtos);
            rst.Total = pagination.Total;
            return Json(rst);
        }


        public async Task<IActionResult> GetMasterTreeJsonForManage(int type, string objId)
        {
            var ids = EnumHelper.GetIds<PermissionType>();
            if (!ids.Any(x => x != type)) return Json(new TResult(0, "Incorrect url"));

            var treeItems = new List<TreeItem>();

            var objectName = "";
            var masterKeyValueDescriptions = GetMasterKeyValueDescriptionsByType(type, objId);
            if (type == (int)PermissionType.Operate)
            {
                objectName = "操作";
                var topTreeItem = new TreeItem();
                topTreeItem.pId = "-2";
                topTreeItem.id = "-1";
                topTreeItem.name = "所有" + objectName;
                treeItems.Add(topTreeItem);
                var appServerAndModules = masterKeyValueDescriptions.Select(x => x.Description).Distinct();
                var keyValDess = new List<KeyValueDescription>();
                foreach (var appServerAndModule in appServerAndModules)
                {
                    var keyValDes1 = new KeyValueDescription();
                    var arr = appServerAndModule.Split("_");
                    keyValDes1.Key = arr[0];
                    keyValDes1.Value = arr.Length > 1 ? arr[1] : "Undefined";
                    keyValDes1.Description = appServerAndModule;
                    keyValDess.Add(keyValDes1);
                }
                var appServers = keyValDess.Select(x => x.Key).Distinct();

                var ct = 1;
                foreach (var appServer in appServers)
                {
                    var treeItem1 = new TreeItem();
                    treeItem1.pId = "-1";
                    var virtualId1 = "virtual-" + ct;
                    treeItem1.id = virtualId1;
                    var enum1 = EnumHelper.GetByName(appServer, ApplicationServer.MvcBasis);
                    treeItem1.name = enum1.GetDescription();
                    treeItems.Add(treeItem1);
                    foreach (var keyValDes in keyValDess.Where(x => x.Key == appServer))
                    {
                        var treeItem2 = new TreeItem();
                        treeItem2.pId = virtualId1;
                        var module = keyValDes.Value;
                        var virtualId2 = "virtual-" + module + ct;
                        treeItem2.id = virtualId2;

                        var enum2 = EnumHelper.GetByName(module, Module.SYS);
                        treeItem2.name = enum2.GetDescription();
                        treeItems.Add(treeItem2);
                        foreach (var masterKeyValueDescription in masterKeyValueDescriptions.Where(x => x.Description == keyValDes.Description))
                        {
                            var treeItem = new TreeItem();
                            treeItem.pId = virtualId2;
                            treeItem.id = masterKeyValueDescription.Key;
                            treeItem.name = masterKeyValueDescription.Value;
                            treeItems.Add(treeItem);
                        }
                    }
                    ct++;
                }
            }
            else if (type == (int)PermissionType.GrantAsManagerForConfig)
            {
                var parentTreeItem = new TreeItem();
                parentTreeItem.pId = "-3";
                var virtualId = "-2";
                parentTreeItem.id = virtualId;
                parentTreeItem.name = "所有配置";
                treeItems.Add(parentTreeItem);
                foreach (var masterKeyValueDescription in masterKeyValueDescriptions)
                {
                    var treeItem = new TreeItem();
                    treeItem.pId = virtualId;
                    treeItem.id = masterKeyValueDescription.Key;
                    treeItem.name = masterKeyValueDescription.Value;
                    treeItems.Add(treeItem);
                }
            }
            else if (type == (int)PermissionType.GrantAsProducerForSysConfig)
            {
                var parentTreeItem = new TreeItem();
                parentTreeItem.pId = "-3";
                var virtualId = "-2";
                parentTreeItem.id = virtualId;
                parentTreeItem.name = "所有类型";
                treeItems.Add(parentTreeItem);
                foreach (var masterKeyValueDescription in masterKeyValueDescriptions.Where(x => x.Description.ToLower() == objId))
                {
                    var treeItem = new TreeItem();
                    treeItem.pId = virtualId;
                    treeItem.id = masterKeyValueDescription.Key;
                    treeItem.name = masterKeyValueDescription.Value;
                    treeItems.Add(treeItem);
                }
            }

            else if (type == (int)PermissionType.GrantAsProducerForCustConfig)
            {
                var cfgType = System.Convert.ToInt32(objId);
                var enum1 = EnumHelper.GetById(cfgType, CustConfigType.Page);
                var parentTreeItem = new TreeItem();
                parentTreeItem.pId = "-3";
                var virtualId = "-2";
                parentTreeItem.id = virtualId;
                parentTreeItem.name = "所有"+ enum1.GetDescription();
                treeItems.Add(parentTreeItem);
                foreach (var masterKeyValueDescription in masterKeyValueDescriptions)
                {
                    var treeItem = new TreeItem();
                    treeItem.pId = virtualId;
                    treeItem.id = masterKeyValueDescription.Key;
                    treeItem.name = masterKeyValueDescription.Value;
                    treeItems.Add(treeItem);
                }

            }
            else if (type == (int)PermissionType.GrantAsConsumerForConfigItem)
            {
                var cfgDefinition = new ConfigHandler().GetConfigDefinition(objId);
                var parentTreeItem = new TreeItem();
                parentTreeItem.pId = "-3";
                var virtualId = "-2";
                parentTreeItem.id = virtualId;
                parentTreeItem.name = "所有" + cfgDefinition.Name;
                treeItems.Add(parentTreeItem);
                foreach (var masterKeyValueDescription in masterKeyValueDescriptions)
                {
                    var treeItem = new TreeItem();
                    treeItem.pId = virtualId;
                    treeItem.id = masterKeyValueDescription.Key;
                    treeItem.name = masterKeyValueDescription.Value;
                    treeItems.Add(treeItem);
                }
            }

            var rst = new TResult<List<TreeItem>>(1, treeItems);
            return Json(rst);
        }

        public async Task<IActionResult> GetActorTreeOrDictItemsJsonForAdd(int actorType)
        {
            if (actorType == (int)ActorType.User)
            {
                var orgId = new Organization().Id.ToString();
                var dtos = await _userService.GetOrganizationAndSubUserTreeByOrgId(orgId, true);
                var rst = new TResult<List<TreeItem>>(1, dtos);
                return Json(rst);
            }
            else
            {
                var dtos = await _userGroupService.GetListDtoDictItemsByType(actorType, false);
                var rst = new TResult<List<DictItem>>(1, dtos);
                return Json(rst);
            }
        }

        public async Task<IActionResult> GetActorIdsStringJsonForAdd(int type, string masterId, int actorType)
        {
            var ids = await _service.GetActorIdsByTypeMasterIdAndActorType(type, masterId, actorType);
            if (actorType == (int)ActorType.User)
            {
                var rst = new TResult<string>(1, ids);
                return Json(rst);
            }
            else
            {
                var dto = new RelatedIdsDto();
                dto.RelatedIds = ids;
                var rst = new TResult<RelatedIdsDto>(1, dto);
                return Json(rst);
            }
        }

        //*post
        [HttpPost]
        public async Task<IActionResult> Add(int type, string masterId, int actorType, RelatedIdsDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var msg = await _service.AddAsync(type, masterId, actorType, model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSelected(string ids)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var msg = await _service.DeleteByIdsAsync(ids);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";

            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        private List<KeyValueDescription> GetMasterKeyValueDescriptionsByType(int type, string objId)
        {
            var configHandler = new ConfigHandler();
            var keyValueDescriptions = new List<KeyValueDescription>();
            if (type == (int)PermissionType.Operate)
            {
                var oprts = configHandler.GetConfigItems(((int)OrpConfigSubType.Operation).ToString()).OrderBy(x => x.Sequence);
                var allCtrls = configHandler.GetConfigItems(((int)DevConfigSubType.Controller).ToString());
                foreach (var oprt in oprts)
                {
                    var keyValueDescription = new KeyValueDescription();
                    keyValueDescription.Key = oprt.Id.ToString();
                    keyValueDescription.Value = oprt.Name;
                    var ctrl = allCtrls.Where(x => x.Key.ToLower() == oprt.Attribute.ToLower()).FirstOrDefault();
                    if (ctrl != null)
                    {
                        var appServerAndModule = ctrl.Attribute.Trim() + "_" + ctrl.Attribute1.Trim();
                        if (appServerAndModule != null)
                        {
                            keyValueDescription.Description = appServerAndModule;
                            keyValueDescriptions.Add(keyValueDescription);
                        }
                    }
                }
            }
            else if (type == (int)PermissionType.GrantAsManagerForConfig)
            {
                var cfgIndexs = EnumHelper.EnumToKeyValueDescriptionList(typeof(ConfigIndex), ConfigIndex.TntConfig.ToString() + " " + ConfigIndex.IndConfig.ToString(), true);
                var crtOperator = CurrentOperator.Instance.GetCurrent();
                var ownnerCfgItems = new ConfigHandler().GetConfigItems(((int)OrpConfigSubType.ConfigOwner).ToString());
                foreach (var cfgIndex in cfgIndexs)
                {
                    cfgIndex.Key = cfgIndex.Key;
                    cfgIndex.Value = cfgIndex.Description;
                    if (crtOperator.IsRoot) keyValueDescriptions.Add(cfgIndex);
                    else
                    {
                        var ownnerCfgItem = ownnerCfgItems.Find(x => x.Attribute.ToLower() == (EnumHelper.GetNameById<ConfigIndex>(Convert.ToInt32(cfgIndex.Key))).ToLower() & x.Attribute1 == crtOperator.ActorId);
                        if (ownnerCfgItem != null) keyValueDescriptions.Add(cfgIndex);
                    }

                }
            }
            else if (type == (int)PermissionType.GrantAsProducerForSysConfig)
            {
                var cfgIndex = objId;
                var permissionHandler = new PermissionHandler();
                var isManager = permissionHandler.IsManger(PermissionType.GrantAsManagerForConfig, cfgIndex);
                if (!isManager) return keyValueDescriptions;
                keyValueDescriptions = new ConfigHandler().GetSysConfigTypeKeyValueDescriptions(cfgIndex);
                foreach (var keyValueDescription in keyValueDescriptions)
                {
                    keyValueDescription.Value = keyValueDescription.Description;
                    keyValueDescription.Description = objId;
                }
            }
            else if (type == (int)PermissionType.GrantAsProducerForCustConfig)
            {
                var cfgIndex = (int)ConfigIndex.CustConfig + "";
                var cfgType = objId;
                var permissionHandler = new PermissionHandler();
                var isManager = permissionHandler.IsManger(PermissionType.GrantAsManagerForConfig, cfgIndex);
                if (!isManager) return keyValueDescriptions;
                var cfgs = configHandler.GetCustConfigs().FindAll(x => x.Type.ToString() == cfgType);
                foreach (var cfg in cfgs)
                {
                    var newKeyValueDescription = new KeyValueDescription();
                    newKeyValueDescription.Key = cfg.Id.ToString();
                    newKeyValueDescription.Value = cfg.Name;
                    newKeyValueDescription.Description = cfg.Type.ToString();
                    keyValueDescriptions.Add(newKeyValueDescription);
                }
            }
            else if (type == (int)PermissionType.GrantAsConsumerForConfigItem)
            {

                var cfgSubType = objId;
                var cfgHandler = new ConfigHandler();
                var cfgDefinition = cfgHandler.GetConfigDefinition(cfgSubType);
                if (cfgDefinition == null) return keyValueDescriptions;

                var cfgIndex = cfgDefinition.Index + "";
                var permissionHandler = new PermissionHandler();
                var isManager = permissionHandler.IsManger(PermissionType.GrantAsManagerForConfig, cfgIndex);
                if (!isManager) return keyValueDescriptions;

                var cfgItems = cfgHandler.GetConfigItems(cfgSubType).FindAll(x => x.Authorization == (int)Authorization.TobePermitted);
                foreach (var cfgItem in cfgItems)
                {
                    var keyValueDescription = new KeyValueDescription();
                    keyValueDescription.Key = cfgItem.Id.ToString();
                    keyValueDescription.Value = cfgItem.Name;
                    keyValueDescription.Description = cfgItem.MasterId;
                    keyValueDescriptions.Add(keyValueDescription);
                }
            }
            return keyValueDescriptions;
        }




    }
}