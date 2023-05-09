using Ligg.Infrastructure.Utilities.LogUtil;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var clientStr = ClientType.MvcAdmin.ToString();
            var area = RouteData.Values["area"];
            var controller = RouteData.Values["controller"].ToString();
            string action = RouteData.Values["Action"].ToString();
            string currentUrl = area == null ? controller + "/" + action : area + "/" + controller + "/" + action;
            var configHandler = new ConfigHandler();
            var crtAction = configHandler.GetCurrentAction(currentUrl);
            if (crtAction == null)
            {
                var errMsg = "Action has not been configurated, pls contact adminstrator";
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    errMsg = errMsg+"; url=" + currentUrl + ", ErrCode:1-1";
                    var rst = new TResult(0, errMsg);
                    context.Result = new JsonResult(rst);
                }
                else context.Result = new RedirectResult("~/Home/Error/NoCfg/Action"+ "/" + currentUrl + "/1-1"+"?addlMsg="+ errMsg);
                LogHelper.Error(errMsg);
                return;
            }

            var pmsHandler = new PermissionHandler();
            var verifyRst= pmsHandler.JudgeActionAvailability(crtAction);
            if(!verifyRst)
            {
                var errMsg = "You are not permitted to operate " + crtAction.Key;
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    errMsg = errMsg + "; ErrCode: 1-20";
                    var rst = new TResult(0, errMsg);
                    context.Result = new JsonResult(rst);
                    var actionLogType = ActionLogType.NotPermitted;
                    RunningLogHelper.SaveActionLog(actionLogType, clientStr, context, null);
                    return;
                }
                else
                {
                    var actionLogType = ActionLogType.NotPermitted;
                    RunningLogHelper.SaveActionLog(actionLogType, clientStr, context, null);
                    context.Result = new RedirectResult("~/Home/Error/NoAuth/Action/" + crtAction.Key + "/1-21" + "?addlMsg=" + errMsg);
                    return;
                }
            }

            var executedContext = await next();
            if (crtAction.Key.ToLower().StartsWith("pag")) ViewBag.PageId = crtAction.Id.ToString();
            RunningLogHelper.SaveActionLog(ActionLogType.Normal, clientStr, context, executedContext);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
        protected string GetErrorCode()
        {
            var area = RouteData.Values["area"];
            var controller = RouteData.Values["controller"].ToString();
            string action = RouteData.Values["Action"].ToString();
            string currentUrl = area == null ? controller + "/" + action : area + "/" + controller + "/" + action;
            var configHandler = new ConfigHandler();
            var crtAction = configHandler.GetCurrentAction(currentUrl);
            if (crtAction == null) return "Undefined";
            return crtAction.Sequence.ToString();

        }

    }
}