using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Basis.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Ligg.Uwa.Application.Shared
{
    public class PermissionHandler
    {
        private readonly CacheHandler _cacheHandler;
        private readonly List<Permission> _permissions;
        private readonly List<ConfigItem> _configItems;
        public PermissionHandler()
        {
            _cacheHandler = new CacheHandler();
            _configItems = _cacheHandler.GetAllCachedConfigItems().FindAll(x => x.Status);
            _permissions = _cacheHandler.GetAllCachedPermissions();
        }

        //*proc
        //####role mvc####################################################
        //*get
        public List<MenuItem> GetAvailableMvcMenuItems(string menuId)
        {
            var menuItems = GetRecursiveChildrenMenuItemsById(menuId, false);
            var currentOprt = CurrentOperator.Instance.GetCurrent();
            if (currentOprt != null)
            {
                if (currentOprt.IsRoot) return menuItems;
            }
            return GetAvailableMvcMenuItemsByMenus(menuItems);
        }

        public List<ConfigDetail> GetUnavailableMvcAdminPageButtons(ConfigItem page)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent();
            if (currentOprt != null)
            {
                if (currentOprt.IsRoot) return new List<ConfigDetail>();
            }

            return GetUnavailableMvcAdminPageButtonsByPage(page);
        }

        //*judge
        public bool JudgeActionAvailability(ConfigItem action, string token = null)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt != null)
            {
                if (currentOprt.IsRoot) return true;
            }

            return IsAvailableAction(action, token);
        }

        public bool JudgePortalAvailability(ConfigItem portal)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent();
            if (currentOprt != null)
                if (currentOprt.IsRoot) return true;

            return CanBeConsumed(PermissionType.GrantAsConsumerForConfigItem, portal);
        }

        public bool JudgeCustPageAvailability(ConfigItem custPage)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent();
            if (currentOprt != null)
                if (currentOprt.IsRoot) return true;

            return CanBeConsumed(PermissionType.GrantAsConsumerForConfigItem, custPage);
        }

        //*proc
        //####role client####################################################
        //*Get
        public List<MenuItem> GetAvailableClientMenuItems(string menuId, string token)
        {
            var menuItems = GetRecursiveChildrenMenuItemsById(menuId, false);
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt != null)
            {
                if (currentOprt.IsRoot) return menuItems;
            }

            return GetAvailableClientMenuItemsByMenus(menuItems, token);
        }

        public string GetUnavailableClientButtons(ConfigItem view, string token)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt != null)
            {
                if (currentOprt.IsRoot) return string.Empty;
            }

            var unavailableButtons = GetUnavailableClientViewButtonsByView(view, token);
            if (unavailableButtons.Count == 0) return string.Empty;
            return unavailableButtons.Select(x => x.Attribute1).ToArray().Unwrap(",");
        }

        public string GetUnavailableClientViews(string portalCode, string token)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt != null)
            {
                if (currentOprt.IsRoot) return string.Empty;
            }
            var unavailableViews = GetUnavailableClientViewsByPortal(portalCode, token);
            if (unavailableViews.Count == 0) return string.Empty;
            return unavailableViews.Select(x => x.Key).ToArray().Unwrap(",");
        }



        //*proc
        //####authgrp####################################################
        //*get
        public string GetPermissionHeadCount(PermissionType permissionType, string masterId)
        {
            if (permissionType == PermissionType.Operate | permissionType == PermissionType.GrantAsManagerForConfig |
                permissionType == PermissionType.GrantAsProducerForSysConfig | permissionType == PermissionType.GrantAsProducerForCustConfig
               )
            {
                return GetHeadCount(permissionType, masterId).ToString();
            }
            else if (permissionType == PermissionType.GrantAsConsumerForConfigItem)
            {
                var master = _configItems.Find(x => x.Id.ToString() == masterId);
                if (master != null)
                {
                    if (master.Authorization == (int)Authorization.TobePermitted)
                        return GetHeadCount(permissionType, masterId).ToString();
                }
            }
            return null;
        }

        public string[] GetAavailableMasterIdsByManager(PermissionType permissionType, string token = null)
        {
            var permissions = new List<Permission>();
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt == null) return null;
            if (permissionType == PermissionType.GrantAsManagerForConfig)
            {
                permissions = _permissions.FindAll(x => x.Type == (int)permissionType);//.Select(x=>x.MasterId).ToArray();
                if (currentOprt.IsRoot) return permissions.Select(x => x.MasterId).Distinct().ToArray();

                var newPermissions = new List<Permission>();
                foreach (var permission in permissions)
                {
                    if (JudgePemissionAvailabilityByTypeAndMasterId(permissionType, permission.MasterId, token)) ;
                    newPermissions.Add(permission);
                }
                newPermissions.Select(x => x.MasterId).Distinct().ToArray();
            }
            return null;
        }


        //*judge
        public bool IsManger(PermissionType permissionType, string masterId, string token = null)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt == null) return false;
            if (currentOprt.IsRoot) return true;

            if (permissionType == PermissionType.GrantAsManagerForConfig)
                return JudgePemissionAvailabilityByTypeAndMasterId(permissionType, masterId, token);
            return false;

        }
        public bool IsProducer(PermissionType permissionType, string masterId, string token = null)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt == null) return false;
            if (currentOprt.IsRoot) return true;

            if (permissionType == PermissionType.GrantAsProducerForSysConfig | permissionType == PermissionType.GrantAsProducerForCustConfig)
            {
                return JudgePemissionAvailabilityByTypeAndMasterId(permissionType, masterId, token);
            }
            return false;
        }



        public string GetActorTypeNamesByPermittedOperatorType(PermittedOperatorType consumerOption)
        {
            var actorTypeNames = "";
            if (consumerOption == PermittedOperatorType.OnlyUser)
            {
                actorTypeNames = ActorType.AuthorizationGroup + " " + ActorType.User;
            }
            else if (consumerOption == PermittedOperatorType.OnlyMachine)
            {
                actorTypeNames = ActorType.AuthorizationGroup + " " + ActorType.Machine;
            }
            else actorTypeNames = ActorType.AuthorizationGroup + " " + ActorType.Machine + " " + ActorType.Machine;
            return actorTypeNames;
        }

        //*func
        //####role mvc####################################################
        private List<MenuItem> GetAvailableMvcMenuItemsByMenus(List<MenuItem> menuItems)
        {
            var availableMenuItems = new List<MenuItem>();
            foreach (var menuItem in menuItems)
            {
                if (menuItem.Type == (int)MenuItemType.Directory)
                {
                    if (menuItems.FindAll(x => x.ParentId == menuItem.Id.ToString()).Count() > 0)
                        availableMenuItems.Add(menuItem);
                }
                else if (menuItem.Type == (int)MenuItemType.SystematicPage)
                {
                    var sysPage = _configItems.Find(x => x.MasterId == ((int)DevConfigSubType.Action).ToString() & x.Id.ToString() == menuItem.PageId);
                    if (sysPage != null)
                    {
                        if (sysPage.Attribute.ToLower() == ActionType.Page.ToString().ToLower())
                        {
                            var controller = sysPage.Attribute1;
                            //var oprtTags = sysPage.Attribute2;
                            //var oprtTags = sysPage.PermissionMark;
                            if (IsAvailableAction(sysPage))
                            {
                                availableMenuItems.Add(menuItem);
                            }
                        }

                    }
                }
                else if (menuItem.Type == (int)MenuItemType.CustomizedPage)
                {
                    var custPage = _configItems.Find(x => x.Id.ToString() == menuItem.PageId);
                    if (custPage != null)
                        if (CanBeConsumed(PermissionType.GrantAsConsumerForConfigItem, custPage))
                            availableMenuItems.Add(menuItem);

                }
                else//outlink
                {
                    availableMenuItems.Add(menuItem);
                }
            }
            return availableMenuItems;
        }

        //#mvc
        private List<ConfigDetail> GetUnavailableMvcAdminPageButtonsByPage(ConfigItem page)
        {
            var unavailableButtons = new List<ConfigDetail>();
            if (page != null)
            {
                var btns = _configItems.FindAll(x => x.MasterId == ((int)DevConfigSubType.MvcAdminPageButton).ToString());
                var buttons = btns.FindAll(x => x.Attribute.ToLower() == page.Key.ToLower());
                if (buttons.Count() > 0)
                {
                    foreach (var btn in buttons)
                    {
                        //if (!JudgeAvailabilityByControllerAndTags(page.Attribute, btn.Attribute2))
                        if (!JudgeAvailabilityByControllerAndPermissionMark(page.Attribute1, btn.PermissionMark))
                        {
                            var configDetail = new ConfigDetail();
                            configDetail.MasterId = btn.MasterId;
                            configDetail.Attribute1 = btn.Attribute1;//eid
                            unavailableButtons.Add(configDetail);
                        }
                    }
                }
            }
            return unavailableButtons;
        }

        //*func
        //####role client####################################################
        private List<MenuItem> GetAvailableClientMenuItemsByMenus(List<MenuItem> menuItems, string token)
        {
            var availableMenuItems = new List<MenuItem>();
            foreach (var menuItem in menuItems)
            {
                if (menuItem.Type == (int)MenuItemType.Directory)
                {
                    if (menuItems.FindAll(x => x.ParentId == menuItem.Id.ToString()).Count() > 0)
                        availableMenuItems.Add(menuItem);
                }
                else if (menuItem.Type == (int)MenuItemType.VueView)
                {
                    var cltView = _configItems.Find(x => x.MasterId == ((int)DevConfigSubType.ClientView).ToString() & x.Id.ToString() == menuItem.PageId);
                    if (cltView != null)
                    {
                        //var vmsg = VerifyClientViewAvailability(cltView, token);
                        //if (vmsg == Consts.OK)
                        if (VerifyClientViewAvailability(cltView, token))
                        {
                            availableMenuItems.Add(menuItem);
                        }
                    }
                }
                else//outlink
                {
                    availableMenuItems.Add(menuItem);
                }
            }
            return availableMenuItems;
        }

        private List<ConfigItem> GetUnavailableClientViewsByPortal(string portalCode, string token)
        {
            var unavailableViews = new List<ConfigItem>();
            var cltViews = _configItems.FindAll(x => x.MasterId == ((int)DevConfigSubType.ClientView).ToString() & (x.Attribute2.ToLower() == portalCode.ToLower() | x.Attribute2.ToLower() == "shared"));
            foreach (var view in cltViews)
            {
                //var vmsg = VerifyClientViewAvailability(view, token);
                //if (vmsg != Consts.OK)
                if (!VerifyClientViewAvailability(view, token))
                {
                    unavailableViews.Add(view);
                }
            }

            return unavailableViews;
        }

        private List<ConfigItem> GetUnavailableClientViewButtonsByView(ConfigItem view, string token)
        {
            var unavailableButtons = new List<ConfigItem>();
            if (view != null)
            {
                var ctrl = view.Attribute;
                var buttons = _configItems.FindAll(x => x.MasterId == ((int)DevConfigSubType.ClientViewButton).ToString() & x.Attribute.ToLower() == view.Key.ToLower());
                if (buttons.Count() > 0)
                {
                    foreach (var btn in buttons)
                    {
                        if(!JudgeAvailabilityByControllerAndPermissionMark(ctrl, btn.PermissionMark, token))
                        {
                            unavailableButtons.Add(btn);
                        }
                    }
                }
            }
            return unavailableButtons;
        }

        private bool VerifyClientViewAvailability(ConfigItem clientView, string token)
        {

            var auth = clientView.Authorization;
            var rst = VerifyAuthorization(auth, token);
            if (rst == PolicyType.Allow) return true;
            else if (rst == PolicyType.Disallow) return false;
            return JudgeAvailabilityByControllerAndPermissionMark(clientView.Attribute, clientView.PermissionMark, token);
        }



        //*common
        //#######common#################################################
        public bool IsAvailableAction(ConfigItem action, string token = null)
        {
            var auth = action.Authorization;
            var rst = VerifyAuthorization(auth, token);
            if (rst == PolicyType.Allow) return true;
            else if (rst == PolicyType.Disallow) return false;
            return JudgeAvailabilityByControllerAndPermissionMark(action.Attribute1, action.PermissionMark, token);
        }
        public bool CanBeConsumed(PermissionType permissionType, object master, string token = null)
        {


            var masterId = "";
            if (permissionType == PermissionType.GrantAsConsumerForConfigItem)
            {
                var configItem = master as ConfigItem;
                var rst = VerifyAuthorization(configItem.Authorization, token);
                if (rst == PolicyType.Allow) return true;
                if (rst == PolicyType.Disallow) return false;

                var currentOprt = CurrentOperator.Instance.GetCurrent(token);
                if (currentOprt == null) return false;
                if (currentOprt.IsRoot) return true;

                masterId = configItem.Id.ToString();
                return JudgePemissionAvailabilityByTypeAndMasterId(permissionType, masterId, token);
            }

            return false;
        }

        private bool JudgeAvailabilityByControllerAndPermissionMark(string controller, string pmsMark, string token = null)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt == null) return false;

            var allOperations = _configItems.FindAll(x => x.MasterId == ((int)OrpConfigSubType.Operation).ToString());
            var operations = allOperations.FindAll(x => x.Attribute.ToLower() == controller.ToLower());
            var pmsMarkArr = pmsMark.GetLarrayArray(true, true);
            if (operations.Count > 0)
            {
                foreach (var operation in operations)
                {
                    var operationauthMarkArr = operation.PermissionMark.GetLarrayArray(true, true);
                    if (operationauthMarkArr.Intersect(pmsMarkArr).Count() > 0)
                        if (JudgeOperationAvailability(operation.Id.ToString(), token)) return true;
                }
            }

            return false;
        }


        private bool JudgeOperationAvailability(string operationId, string token = null)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt == null) return false;

            var allOperations = _configItems.FindAll(x => x.MasterId == ((int)OrpConfigSubType.Operation).ToString());
            var operation = allOperations.Find(x => x.Id.ToString() == operationId);
            var immuneToAdmin = false;
            if (operation.Attribute1 != null)
                immuneToAdmin = operation.Attribute1.JudgeJudgementFlag();
            if (immuneToAdmin & currentOprt.IsAdministrator) return true;
            return JudgePemissionAvailabilityByTypeAndMasterId((int)PermissionType.Operate, operation.Id.ToString(), token);
        }

        //#role or usergroup

        private int GetHeadCount(PermissionType permissionType, string masterId)
        {
            var permissions = _permissions.FindAll(x => x.Type == (int)permissionType);
            if (!masterId.IsNullOrEmpty())
                permissions = permissions.FindAll(x => x.MasterId == masterId);
            var arr = permissions.Select(x => x.Id).Distinct().ToArray();
            return arr == null ? 0 : arr.Length;
        }


        //*private
        //##########private########################################
        private bool JudgePemissionAvailabilityByTypeAndMasterId(PermissionType pmsType, string masterId, string token = null)
        {
            var currentOprt = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprt == null) return false;
            if (_permissions == null) return false;
            if (_permissions.Count == 0) return false;
            var permissions = _permissions.FindAll(x => x.Type == (int)pmsType && x.MasterId == masterId);
            foreach (var permission in permissions)
            {
                if (!currentOprt.IsMachine)
                {
                    if (permission.ActorType == (int)ActorType.User)
                    {
                        if (permission.ActorId == currentOprt.ActorId) return true;
                    }
                    else if (permission.ActorType == (int)ActorType.Role)
                    {
                        var currentOprtRoleArr = currentOprt.RoleIds.GetLarrayArray(true, true);
                        if (currentOprtRoleArr != null)
                        {
                            if (currentOprtRoleArr.Contains(permission.ActorId)) return true;
                        }

                    }
                    else if (permission.ActorType == (int)ActorType.AuthorizationGroup)
                    {
                        var currentOprtAuthGrpArr = currentOprt.AuthorizationGroupIds.GetLarrayArray(true, true);
                        if (currentOprtAuthGrpArr != null)
                        {
                            if (currentOprtAuthGrpArr.Contains(permission.ActorId)) return true;
                        }
                    }
                }
            }

            return false;

        }

        private PolicyType VerifyAuthorization(int authInt, string token = null)
        {
            var oprtor = CurrentOperator.Instance.GetCurrent(token);
            var auth = EnumHelper.GetById<Authorization>(authInt, Authorization.TobePermitted);
            if (auth == Authorization.Anonymous) { return PolicyType.Allow; }
            else if (auth == Authorization.AnyOne)
            {
                if (oprtor == null) return PolicyType.Disallow;
                else return PolicyType.Allow;
            }
            else if (auth == Authorization.AnyUser)
            {
                if (oprtor != null)
                {
                    if (oprtor.IsMachine) return PolicyType.Disallow;
                    else return PolicyType.Allow;
                }
                else return PolicyType.Disallow;

            }
            else if (auth == Authorization.AnyMachine)
            {
                if (oprtor != null)
                {
                    if (oprtor.IsMachine) return PolicyType.Allow;
                    else return PolicyType.Disallow;
                }
                else return PolicyType.Disallow;
            }

            return PolicyType.Undefined;

        }

        public PolicyType VerifyAuthorization1(string authStr, string token = null)
        {
            if (string.IsNullOrEmpty(authStr)) return PolicyType.Undefined;

            var oprtor = CurrentOperator.Instance.GetCurrent(token);
            if (authStr.ToLower() == Authorization.Anonymous.ToString().ToLower()) { return PolicyType.Allow; }
            else if (authStr.ToLower() == Authorization.AnyOne.ToString().ToLower())
            {
                if (oprtor == null) return PolicyType.Disallow;
                else return PolicyType.Allow;
            }
            else if (authStr.ToLower() == Authorization.AnyUser.ToString().ToLower())
            {
                if (oprtor != null)
                {
                    if (oprtor.IsMachine) return PolicyType.Disallow;
                    else return PolicyType.Allow;
                }
                else return PolicyType.Disallow;

            }
            else if (authStr.ToLower() == Authorization.AnyMachine.ToString().ToLower())
            {
                if (oprtor != null)
                {
                    if (oprtor.IsMachine) return PolicyType.Allow;
                    else return PolicyType.Disallow;
                }
                else return PolicyType.Disallow;
            }

            return PolicyType.Undefined;
        }



        private List<MenuItem> GetRecursiveChildrenMenuItemsById(string id, bool includeParent)
        {
            var menuItems = _cacheHandler.GetAllCachedMenuItems().FindAll(x => x.Status);
            var list = new List<MenuItem>();
            var topMenu = menuItems.Find(x => x.Id.ToString() == id.ToString());
            if (includeParent)
            {
                list = menuItems.FindAll(x => x.CascadedNo.Contains(topMenu.CascadedNo)).ToList();
            }
            else
            {
                list = menuItems.FindAll(x => x.CascadedNo.Contains(topMenu.CascadedNo) & x.CascadedNo != topMenu.CascadedNo).ToList();
            }
            return list;
        }


    }
}
