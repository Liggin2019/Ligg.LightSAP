using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Ligg.Uwa.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ligg.Uwa.Mvc.Controllers
{
    public class CustPageController : BaseController
    {
        private readonly TenantService _tenantService;
        private readonly UserService _userService;
        public CustPageController(TenantService tenantService, UserService userService)
        {
            _tenantService = tenantService;
            _userService = userService;
        }

        //*page
        //index=layout, none empty simple 
        //obj=pageKeyOrId
        //objId=menuItemId 0:none
        public IActionResult Show(string index, string obj, string objId)
        {
            var errCode = GetErrorCode();
            ViewBag.CurrentUrl = "/CustPage/index/" + index + "/" + obj + "/" + objId;
            var configHandler = new ConfigHandler();
            var currentTenant = _tenantService.GetCurrentShowDtoAsync().Result;
            ViewBag.CurrentTenant = currentTenant;

            var layout = "none";
            if (!index.IsNullOrEmpty())
            {
                layout = index.ToLower();
                if (layout != "none")
                    layout = layout.StartsWith("_") ? layout : "_custpage" + layout;
            }

            if (!"none,_custpagesimple,_custpageempty".Split(',').Contains(layout))
                return Redirect("/Home/Error/wcfg/layout/" + layout + "/" + errCode + "-1");
            ViewBag.Layout = layout;

            var pageKeyOrId = obj;
            if (pageKeyOrId.IsNullOrEmpty()) return Redirect("/Home/Error/wurl/pageKeyOrId/null" + "/" + errCode + "-2");

            var custPages = configHandler.GetCustConfigItems((int)CustConfigType.Page);
            var allPages = custPages;
            ConfigItem page = null;
            page = allPages.Find(x => x.Id.ToString() == obj.ToLower() | x.Key.ToLower() == pageKeyOrId.ToLower());
            if (page == null) return Redirect("/Home/Error/wurl/custPage/null" + "/" + errCode + "-3");
            if (page.Value.IsNullOrEmpty()) return Redirect("/Home/Error/wcfg/custPage.Value/null" + "/" + errCode + "-4");

            var pageAuth = page.Authorization;
            var pmsHandler = new PermissionHandler();
            if (!pmsHandler.JudgeCustPageAvailability(page))
            {
                return Redirect("/Home/Error/NoAuth/custPage/" + page.Key + "/" + errCode + "-5");
            }
            ViewBag.Title = page.Name;


            var model = GetCustPageModel(page);
            ViewBag.Option = model.Option;
            ViewBag.ObjectId = model.ObjectId;

            var viewName = model.View.ToLower();
            if (viewName != "article")
            {
                return Redirect("/Home/Error/noCfg/page.Attribute1/" + page.Attribute1 + "." + model.View + "/" + errCode + "-6");//
            }


            ViewBag.CustPageModel = model;
            return View();
        }

        public IActionResult Error(string index, string obj, string objId, string option, string addlMsg)
        {
            var txtHandler = new TextHandler();
            var errDetail = txtHandler.GetErrorDetail(index, obj, objId, option, addlMsg);
            ViewBag.ErrorDetail = errDetail;
            return View();
        }


        //*private
        private CustPageModel GetCustPageModel(ConfigItem page)
        {
            var model = new CustPageModel();
            var pageLayoutId = page.Attribute1 ?? "article";
            model.View = "article";

            if (pageLayoutId == "article")
            {
                var pageValueDict = page.Value.ConvertLdictToDictionary(true, true,true);
                model.Option = pageValueDict.GetLdictValue("Option");
                model.ObjectId = pageValueDict.GetLdictValue("ObjectId");
            }
            return model;
        }

    }
}
