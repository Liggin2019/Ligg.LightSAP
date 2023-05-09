using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Utilities.LogUtil;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.WebApi
{
    public class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            string currentUrl = context.HttpContext.Request.Path;
            currentUrl = currentUrl.TrimStart('/');
            string auth = context.HttpContext.Request.Headers["Authorization"].ToString();
            
            auth = auth ?? "";
            var authArr = auth.Split(' ').Trim().Wash();
            var clientStr = "";
            if (authArr != null & authArr.Length > 0) clientStr = authArr[0];
            var token = JwtHelper.GetToken(context.HttpContext.Request);
            if (currentUrl.ToLower() != "sys/user/logon" & currentUrl.ToLower() != "sys/machine/logon")
            {
                var clientTypes = EnumHelper.GetNames<ClientType>();
                if (authArr == null)
                {
                    var actionLogType = ActionLogType.IllegalLogon;
                    RunningLogHelper.SaveActionLog(actionLogType, clientStr, context, null, token);
                    return;
                }
                else if (!clientTypes.Contains(clientStr))
                {
                    var actionLogType = ActionLogType.IllegalVisit;
                    RunningLogHelper.SaveActionLog(actionLogType, clientStr, context, null, token);
                    return;
                }

                if (token.IsNullOrEmpty())
                {
                    var actionLogType = ActionLogType.IllegalVisit;
                    RunningLogHelper.SaveActionLog(actionLogType, clientStr, context, null, token);
                    return;
                }
            }


            var configHandler = new ConfigHandler();
            var crtAction = configHandler.GetCurrentAction(currentUrl);

            if (crtAction == null)
            {
                var errMsg = "Action has not been configurated, pls contact adminstrator";
                errMsg = errMsg + "; url=" + currentUrl + ", ErrCode:0-1";
                LogHelper.Error(errMsg);
                return;
            }


            //pmsCheck
            var pmsHandler = new PermissionHandler();
            var verifyRst = pmsHandler.JudgeActionAvailability(crtAction, token);
            if (!verifyRst)
            {
                var errMsg = "You are not permitted to operate " + crtAction.Key;
                errMsg = errMsg + "; ErrCode: 0-2";
                LogHelper.Error(errMsg);

                var actionLogType = ActionLogType.NotPermitted;
                RunningLogHelper.SaveActionLog(actionLogType, clientStr, context, null, token);
                return;
            }

            var executedContext = await next();
            RunningLogHelper.SaveActionLog(ActionLogType.Normal, clientStr, context, executedContext, token);

        }
    }
}
