using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Utilities.ImageUtil;
using Ligg.Infrastructure.Utilities.LogUtil;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SCC;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ligg.Uwa.Mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly TenantService _tenantService;
        private readonly UserService _userService;
        private readonly MenuItemService _menuService;
        private readonly ArticleService _articleService;
        public HomeController(TenantService tenantService, UserService userService, MenuItemService menuService, ArticleService articleService)
        {
            _tenantService = tenantService;
            _userService = userService;
            _menuService = menuService;
        }

        //*page
        [HttpGet]
        //index=portalAbbr
        //obj=designatedLandingPageKeyOrId for site
        public IActionResult Index(string index, string obj)
        {
            LogHelper.Trace("Index begins");
            var errCode = GetErrorCode();
            var configHandler = new ConfigHandler();
            var currentTenant = _tenantService.GetCurrentShowDtoAsync().Result;
            ViewBag.CurrentTenant = currentTenant;

            var currentOprtor = CurrentOperator.Instance.GetCurrent();
            ViewBag.CurrentUser = currentOprtor.MapTo<ShowSelfModel>();

            ConfigItem currentPortal = null;
            var clientType = ClientType.MvcSite;
            if (index.IsNullOrEmpty())
            {
                currentPortal = configHandler.GetDefaultConfigItem(((int)OrpConfigSubType.MvcSitePortal).ToString(), GlobalContext.SystemSetting.ApplicationServer);
            }
            else
            {
                var sitePortals = configHandler.GetConfigItems(((int)OrpConfigSubType.MvcSitePortal).ToString(), GlobalContext.SystemSetting.ApplicationServer);
                currentPortal = sitePortals.Find(x => x.Attribute2.ToLower() == index.ToLower());
                if (currentPortal == null)
                {
                    var adminPortals = configHandler.GetConfigItems(((int)OrpConfigSubType.MvcAdminPortal).ToString(), GlobalContext.SystemSetting.ApplicationServer);
                    currentPortal = adminPortals.Find(x => x.Attribute2.ToLower() == index.ToLower());
                    clientType = ClientType.MvcAdmin;
                }
            }
            if (currentPortal == null)
            {
                return Redirect("/Home/Error/wurl/portal/" + (index ?? "null") + "/" + errCode + "-1");
            }
            var pmsHandler = new PermissionHandler();
            var auth = currentPortal.Authorization;

            var anonymous = auth == (int)Authorization.Anonymous;
            if (clientType == ClientType.MvcAdmin)
                if (anonymous) return Redirect("/Home/Error/wcfg/portal/" + currentPortal.Key + "/" + errCode + "-2"+ 
                    "?addlMsg=" + OrpConfigSubType.MvcAdminPortal.ToString() + " can't be anonymously visited" ); 

            ViewBag.Anonymous = anonymous;
            var available = pmsHandler.JudgePortalAvailability(currentPortal);
            if (!available)
            {
                return Redirect("/Sys/User/Logon?portal=" + currentPortal.Attribute2.ToLower());
            }

            ViewBag.CurrentPortal = currentPortal;
            ViewBag.Title = currentPortal.Name;
            var ptlVal = currentPortal.Value;
            if (ptlVal.IsNullOrEmpty()) return Redirect("/Home/Error/wurl/portal.Value/null" + "/" + errCode + "-3");
            var ptlValDict = ptlVal.ConvertLdictToDictionary(true, true, true);
            ViewBag.Keywords = ptlValDict.GetLdictValue("Keywords");
            ViewBag.Theme = ptlValDict.GetLdictValue("Theme");

            var allPages = new List<ConfigItem>();
            var custPages = configHandler.GetCustConfigItems((int)CustConfigType.Page);
            allPages.AddRange(custPages);
            var sysPages = configHandler.GetConfigItems(((int)DevConfigSubType.Action).ToString());
            allPages.AddRange(sysPages);
            var menuItems = new List<MenuItem>();
            var menuId = ptlValDict.GetLdictValue("MenuId");
            if (!menuId.IsNullOrEmpty())
            {
                menuItems = pmsHandler.GetAvailableMvcMenuItems(menuId);
            }
            if (menuItems.Count > 0)
            {
                var outlinks = configHandler.GetConfigItems(((int)OrpConfigSubType.MenuItemOutlink).ToString());
                outlinks = outlinks.FindAll(x => x.Attribute.ToLower() == GlobalContext.SystemSetting.ApplicationServer.ToLower() & (x.Attribute1.ToLower() == currentPortal.Key.ToLower() | x.Attribute1.ToLower() == "shared"));
                var listMenuItemsDtos = new List<ListMenuItemsDto>();
                foreach (var menuItem in menuItems)
                {
                    var ListMenuItemsDto = menuItem.MapTo<ListMenuItemsDto>();
                    if (menuItem.Type == (int)MenuItemType.SystematicPage)
                    {
                        menuItem.IsBlankTarget = false;
                        var action = allPages.Where(x => x.Id.ToString() == menuItem.PageId).FirstOrDefault();
                        if (action != null) ListMenuItemsDto.Url = action.Attribute2 + menuItem.UrlParams;
                        else ListMenuItemsDto.Url = "remove";
                    }
                    else if (menuItem.Type == (int)MenuItemType.CustomizedPage)
                    {
                        var custPage = allPages.Where(x => x.Id.ToString() == menuItem.PageId).FirstOrDefault();
                        if (custPage != null)
                        {
                            var isSamePortalPage = custPage.Attribute.ToLower() == currentPortal.Key.ToLower();
                            var isSharedPortalPage = custPage.Attribute.ToLower() == "shared";
                            var custPageLayout = clientType == ClientType.MvcAdmin ? "empty" : "none";
                            if (isSharedPortalPage)
                            {
                                if (menuItem.IsBlankTarget)
                                {
                                    ListMenuItemsDto.Url = "CustPage/Show/simple" + "/" + custPage.Key + "/" + menuItem.Id + menuItem.UrlParams;
                                }
                                else
                                {
                                    ListMenuItemsDto.Url = "CustPage/Show/" + custPageLayout + "/" + custPage.Key + "/" + menuItem.Id + menuItem.UrlParams;
                                }
                            }
                            else if (isSamePortalPage)
                            {
                                ListMenuItemsDto.Url = "CustPage/Show/" + custPageLayout + "/" + custPage.Key + "/" + menuItem.Id + menuItem.UrlParams;
                                if (menuItem.IsBlankTarget)
                                {
                                    menuItem.IsBlankTarget = false;
                                }
                            }
                            else
                            {
                                var custPagePtlKey = custPage.Attribute;
                                var custPagePtl = configHandler.GetConfigItem((int)OrpConfigSubType.MvcSitePortal + "", custPagePtlKey);
                                if (custPagePtl != null)
                                {
                                    ListMenuItemsDto.Url = "Home/index/" + custPagePtl.Attribute2 + "/" + custPage.Key;
                                    if (!menuItem.IsBlankTarget)
                                    {
                                        menuItem.IsBlankTarget = true;
                                    }
                                }
                                else ListMenuItemsDto.Url = "remove";
                            }

                            if (clientType == ClientType.MvcSite)
                            {
                                var custPageValueDict = custPage.Value.ConvertLdictToDictionary(true, true, true);
                                if (custPageValueDict != null)
                                {
                                    var custPageViewArgs = custPageValueDict.GetLdictValue("ViewArgs");
                                    if (!custPageViewArgs.IsNullOrEmpty())
                                    {
                                        var isCustPageTransparentHeader = custPageViewArgs.GetLdictValue("IsTransparentHeader", true, true);
                                        ListMenuItemsDto.IsTransparentHeader = isCustPageTransparentHeader.JudgeJudgementFlag();
                                    }
                                }
                            }
                        }
                        else
                        {
                            ListMenuItemsDto.Url = "remove";
                        }
                    }
                    else if (menuItem.Type == (int)MenuItemType.OuterLink)
                    {
                        menuItem.IsBlankTarget = true;
                        var outlink = outlinks.Find(x => x.Id.ToString() == menuItem.PageId);
                        ListMenuItemsDto.Url = "";
                        if (outlink != null) ListMenuItemsDto.Url = outlink.Value;
                    }
                    else //dir
                    {
                        var childNum = menuItems.Where(x => x.ParentId == menuItem.Id.ToString()).Count();
                        if (childNum == 0) ListMenuItemsDto.Url = "remove";
                    }
                    if (ListMenuItemsDto.Url != "remove") listMenuItemsDtos.Add(ListMenuItemsDto);

                }

                ViewBag.TopMenuItemId = menuId;
                var orderedMenuItems = listMenuItemsDtos.OrderBy(x => x.Sequence).ToList();
                ViewBag.MenuItems = orderedMenuItems;
            }

            if (clientType == ClientType.MvcAdmin)
            {
                var layout = currentPortal.Attribute1 ?? "";
                if (!"_admin".Split(',').Contains(layout.ToLower()))
                    layout = "_admin";
                ViewBag.Layout = layout;

                var theme = ptlValDict.GetLdictValue("Theme");
                if (!theme.IsNullOrEmpty())
                {
                    var themeArr = theme.GetLarrayArray(true, true);
                    if (themeArr.Length == 2)
                    {
                        var skins = new string[] { "skin-blue", "skin-green", "skin-purple", "skin-red", "skin-yellow" };
                        var themes = new string[] { "theme-dark", "theme-light" };
                        if (skins.Contains(themeArr[0].ToLower()) & themes.Contains(themeArr[1].ToLower()))
                        {
                            var themeValue = themeArr[0].ToLower() + "|" + themeArr[1].ToLower();
                            var themeCookie = CookieHelper.GetCookie("Skin");
                            if (themeValue != themeCookie)
                                CookieHelper.WriteCookie("Skin", themeValue, false);
                        }
                    }
                }

                var landingPageIdOrKey = ptlValDict.GetLdictValue("LandingPage");
                if (landingPageIdOrKey.IsNullOrEmpty()) ViewBag.LandingPageUrl = "/Home/Error/wcfg/portal.Value.LandingPage/null" + " /" + errCode + "-10" + "?addlMsg=pls check the config in " + OrpConfigSubType.MvcAdminPortal.ToString() + ": " + currentPortal.Key;
                else
                {
                    var landingPage = allPages.Find(x => x.Id.ToString() == landingPageIdOrKey | x.Key.ToLower() == landingPageIdOrKey.ToLower());
                    if (landingPage == null)
                    {
                        ViewBag.LandingPageUrl =
                        "/Home/Error/wcfg/portal.Value.LandingPage/" + landingPageIdOrKey + "/" + errCode + "-10" +
                        "?addlMsg=pls check the config of " + OrpConfigSubType.MvcAdminPortal.ToString() + ": " + currentPortal.Key + ", the Page should be valid and enabled";
                    }
                    else if (landingPage.Attribute.ToLower() != currentPortal.Key.ToLower() & landingPage.Attribute.ToLower() != "shared")
                    {
                        ViewBag.LandingPageUrl =
                          "/Home/Error/wcfg/portal.Value.LandingPage/" + landingPageIdOrKey + "/" + errCode + "-11" +
                          "?addlMsg=pls check the config of Page:" + landingPageIdOrKey + ", the PortalKey of LandingPage should be 'shared' or " + "'" + currentPortal.Key + "'";
                    }
                    else
                    {
                        var isCustPage = landingPage.MasterId != ((int)DevConfigSubType.Action).ToString();
                        ViewBag.LandingPageUrl = isCustPage ? ("/CustPage/Show/empty/" + landingPage.Key + "/0") : ("/" + landingPage.Attribute2);
                    }
                }

            }
            else //if (protalType == "site")
            {
                var layout = currentPortal.Attribute1 ?? "";
                if (!"_site".Split(',').Contains(layout.ToLower()))
                    return Redirect("/Home/Error/wcfg/layout/" + layout + "/" + errCode + "-20");
                ViewBag.Layout = layout;
                var layoutComponents = configHandler.GetConfigItems(((int)OrpConfigSubType.PortalLayoutComponent).ToString(), currentPortal.Key);
                ViewBag.LayoutComponents = layoutComponents;

                var landingPage = new ConfigItem();
                var landingPageIdOrKey = ptlValDict.GetLdictValue("LandingPage");
                if (!obj.IsNullOrEmpty())
                {
                    var designatedLandingPageKeyOrId = obj.ToLower();
                    landingPage = custPages.Find(x => x.Id.ToString() == designatedLandingPageKeyOrId | x.Key.ToLower() == designatedLandingPageKeyOrId);
                }
                else
                {
                    if (landingPageIdOrKey.IsNullOrEmpty()) return Redirect("/Home/Error/wurl/portal.Value.LandingPage/null" + "/" + errCode + "-21" + "?addlMsg=pls check the config in " + OrpConfigSubType.MvcSitePortal.ToString() + ": " + currentPortal.Key);
                    landingPage = custPages.Find(x => x.Id.ToString() == landingPageIdOrKey | x.Key.ToLower() == landingPageIdOrKey.ToLower());
                }

                if (landingPage == null) return Redirect("/Home/Error/wcfg/portal.Value.LandingPage/" + landingPageIdOrKey + "/" + errCode + "-22"
                    + "?addlMsg=pls check the config of " + OrpConfigSubType.MvcSitePortal.ToString() + ": " + currentPortal.Key + ", the Page should be valid and enabled");
                var landingPagePortalKey = landingPage.Attribute ?? "null";
                if (landingPagePortalKey.ToLower() != currentPortal.Key.ToLower() & landingPagePortalKey.ToLower() != "shared") return Redirect("/Home/Error/wcfg/portal.Value.LandingPage/" + landingPageIdOrKey + "/" + errCode + "-23" +
                     "?addlMsg=pls check the config of Page:" + landingPageIdOrKey + ", the PortalKey of LandingPage should be 'shared' or " + "'" + currentPortal.Key + "'");

                var landingPageValueDict = landingPage.Value.ConvertLdictToDictionary(true, true, true);
                if (landingPageValueDict == null) return Redirect("/Home/Error/wcfg/page.LandingPage.Value/" + landingPage.Key + "." + landingPage.Value + "/" + errCode + "-24");
                var landingPageViewArgs = landingPageValueDict.GetLdictValue("ViewArgs");
                if (!landingPageViewArgs.IsNullOrEmpty())
                {
                    var isTransparentHeader = landingPageViewArgs.GetLdictValue("IsTransparentHeader", true, true);
                    ViewBag.IsTranparentHeader = isTransparentHeader.JudgeJudgementFlag();
                }

                ViewBag.LandingPageUrl = "/CustPage/Show/none/" + landingPage.Key + "/0";
            }

            LogHelper.Trace("Index ends");
            return View("Index/" + (clientType == ClientType.MvcAdmin ? "Admin" : "Site") + "/" + "index");

        }

        public IActionResult Error(string index, string obj, string objId, string option, string addlMsg)
        {
            var currentTenant = _tenantService.GetCurrentShowDtoAsync().Result;
            ViewBag.CurrentTenant = currentTenant;

            var txtHandler = new TextHandler();
            var errKey = index;
            var errCode = option;
            var errDetail = txtHandler.GetErrorDetail(errKey, obj, objId, errCode, addlMsg);
            ViewBag.ErrorDetail = errDetail;
            return View();
        }


        //*page
        public IActionResult Skin()
        {
            return View();
        }

        //*get
        public IActionResult GetCaptchaImage()
        {
            Tuple<string, int> captchaCode = CaptchaHelperArithmetic.GetCaptchaCode();
            byte[] bytes = CaptchaHelperArithmetic.CreateCaptchaImage(captchaCode.Item1);
            SessionHelper.WriteSession("CaptchaCode", captchaCode.Item2);
            return File(bytes, @"image/jpeg");
        }

    }
}
