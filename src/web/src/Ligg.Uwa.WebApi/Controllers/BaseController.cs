using Ligg.Uwa.Application.Shared;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Ligg.Uwa.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected string GetErrorCode()
        {
            //var rr=Request.Headers["Authorization"].ToString();
            //string url = Request.Path;
            //var area = RouteData.Values["area"];
            //var controller = RouteData.Values["controller"].ToString();
            //string action = RouteData.Values["Action"].ToString();
            //string currentUrl = area == null ? controller + "/" + action : area + "/" + controller + "/" + action;
            var currentUrl= Request.Path;
            if(currentUrl==null) return "Undefined"; 
            var currentUrlStr = currentUrl.ToString().TrimStart('/');
            var configHandler = new ConfigHandler();
            var crtAction = configHandler.GetCurrentAction(currentUrlStr);
            if (crtAction == null) return "Undefined";
            return crtAction.Sequence.ToString();

        }
    }
}