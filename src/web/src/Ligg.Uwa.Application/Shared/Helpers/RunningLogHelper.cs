using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using Ligg.Infrastructure.Utilities.NetworkUtil;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ligg.Uwa.Application.Shared
{
    public static class RunningLogHelper
    {
        public static void SaveActionLog(ActionLogType type, string client, ActionExecutingContext context, ActionExecutedContext executedContext, string token = null)
        {
            var area = context.RouteData.Values["area"] + "/";
            var controller = context.RouteData.Values["controller"] + "/";
            string action = context.RouteData.Values["Action"].ToString();
            string currentUrl = area + controller + action;

            //*test
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            var configHandler = new ConfigHandler();
            var applicationServerName = GlobalContext.SystemSetting.ApplicationServer;
            var actionLogPolicy = configHandler.GetConfigItems(((int)OrpConfigSubType.ActionLogPolicy).ToString(), applicationServerName).FirstOrDefault();

            if (actionLogPolicy == null) return;

            var actionLogPolicDict = actionLogPolicy.Value.ConvertLdictToDictionary(true, true, true);
            if (actionLogPolicDict == null) return;
            var policyType = actionLogPolicy.Attribute1 ?? "";
            if (policyType.ToLower() == PolicyType.Disallow.ToString().ToLower()) return;

            var toLogByAction = true;
            var crtAction = configHandler.GetCurrentAction(currentUrl);
            if (crtAction == null) return;
            var crtActionKey = crtAction.Key;
            var actionPolicy = actionLogPolicDict.GetLdictValue("ActionPolicy");
            if (actionPolicy == PolicyOption.None.ToString())
            { toLogByAction = false; }
            else if (actionPolicy == PolicyOption.Some.ToString())
            {
                var permittedActionsStr = actionLogPolicDict.GetLdictValue("Actions");
                var permittedActionsArr = permittedActionsStr.GetLarrayArray(true, true);
                if (permittedActionsArr == null) toLogByAction = false;
                else if (!permittedActionsArr.ToLower().Contains(crtActionKey.ToLower())) toLogByAction = false;
            }
            else if (actionPolicy == PolicyOption.AllBut.ToString()) //AllBut
            {
                var excludedActionsStr = actionLogPolicDict.GetLdictValue("Actions");
                var excludedActionsStrArr = excludedActionsStr.GetLarrayArray(true, true);
                if (excludedActionsStrArr != null)
                    if (!excludedActionsStrArr.ToLower().Contains(crtActionKey.ToLower())) toLogByAction = false;
            }
            else toLogByAction = false;
            if (!toLogByAction) return;

            var crtOprtor = CurrentOperator.Instance.GetCurrent(token);
            var actorTypeInt = (int)PolicyType.Undefined;
            if (crtOprtor != null)
                actorTypeInt = crtOprtor.IsMachine?(int)ActorType.Machine:(int)ActorType.User;
            var operatorTypeStr = PolicyType.Undefined.ToString();
            var permittedOperatorTypeStr = actionLogPolicDict.GetLdictValue("OperatorTypePolicy");
            var verifyAuthorizationRst = new PermissionHandler().VerifyAuthorization1(operatorTypeStr);
            if (verifyAuthorizationRst == PolicyType.Disallow) return;
            if (verifyAuthorizationRst == PolicyType.Allow)
            {
                DoSaveActionLog(type, client, context, executedContext, currentUrl, actionLogPolicDict, actorTypeInt);
                return;
            }

            var toLogByOperator = true;
            if (permittedOperatorTypeStr.ToLower() == PermittedOperatorType.OnlyUser.ToString().ToLower())
            {
                if (crtOprtor == null) return;
                if (crtOprtor.IsMachine) return;
            }
            else if (permittedOperatorTypeStr.ToLower() == PermittedOperatorType.OnlyMachine.ToString().ToLower())
            {
                if (crtOprtor == null) return;
                if (!crtOprtor.IsMachine) return;

            }
            else if (permittedOperatorTypeStr.ToLower() == PermittedOperatorType.UserAndMachine.ToString().ToLower())
            {
                if (crtOprtor == null) return;
            }
            else return;

            if (crtOprtor != null) toLogByOperator = JudgeByOperator(actionLogPolicDict, crtOprtor);
            if (!toLogByOperator) return;

            DoSaveActionLog(type, client, context, executedContext, currentUrl, actionLogPolicDict, actorTypeInt);
            //*test
            //sw.Stop();
        }
        private static void DoSaveActionLog(ActionLogType type, string client, ActionExecutingContext context, ActionExecutedContext executedContext, string currentUrl, Dictionary<string, string> actionLogPolicDict, int actorTypeInt)
        {
            var actionLog = new ActionLog();
            actionLog.Type = (int)type;
            actionLog.ApplicationServerId = GlobalContext.SystemSetting.ApplicationServerId;
            actionLog.OperatorType = actorTypeInt;

            //get Request.QueryString.Value
            switch (context.HttpContext.Request.Method.ToUpper())
            {
                case "GET":
                    actionLog.RequestString = context.HttpContext.Request.QueryString.Value ?? "".ToString();
                    break;

                case "POST":
                    if (context.ActionArguments?.Count > 0)
                    {
                        actionLog.Url += context.HttpContext.Request.QueryString.Value ?? "".ToString();
                        actionLog.Url = (JsonConvert.SerializeObject(context.ActionArguments));
                    }
                    else
                    {
                        actionLog.RequestString = context.HttpContext.Request.QueryString.Value ?? "".ToString();
                    }
                    break;
            }

            StringBuilder sbException = new StringBuilder();
            actionLog.State = true;
            if (executedContext != null)
            {
                if (executedContext.Exception != null)
                {
                    Exception exception = executedContext.Exception;
                    sbException.AppendLine(exception.Message);
                    while (exception.InnerException != null)
                    {
                        sbException.AppendLine(exception.InnerException.Message);
                        exception = exception.InnerException;
                    }
                    sbException.AppendLine(executedContext.Exception.StackTrace);
                    actionLog.State = false;
                }
            }

            string ip = WebClientHelper.Ip;
            actionLog.Client = client.GetSubString(15);
            actionLog.IpAddress = ip.GetSubString(15);
            actionLog.Url = currentUrl.Replace("//", "/").GetSubString(255);
            actionLog.RequestString = actionLog.RequestString.GetSubString(255);
            actionLog.Exception = sbException.ToString().GetSubString(511);

            //will be consuming too much time
            var logIpLocation = actionLogPolicDict.GetLdictValue("LogIpLocation").JudgeJudgementFlag();
            if (logIpLocation)
                actionLog.IpLocation = IpHelper.GetIpLocation(ip).GetSubString(127);

            var runningDuration = 0;

            //runningDuration = sw.ElapsedMilliseconds;
            actionLog.RunningDuration = runningDuration;
            var dbRepository = new RunningLogDbRepository();
            dbRepository.SaveActionLog(actionLog);
        }

        public static void SaveEntrylog(int type, string account, bool state, string msg, string token = null)
        {

            var configHandler = new ConfigHandler();
            var entryLogPolicy = configHandler.GetConfigItems(((int)OrpConfigSubType.EntryLogPolicy).ToString(), GlobalContext.SystemSetting.ApplicationServer).FirstOrDefault();
            if (entryLogPolicy == null) return;
            var entryLogPolicyDict = entryLogPolicy.Value.ConvertLdictToDictionary(true, true, true);
            if (entryLogPolicyDict == null) return;

            var policyType = entryLogPolicy.Attribute1 ?? "";
            if (policyType.ToLower() == PolicyType.Disallow.ToString().ToLower()) return;

            var crtOprtor = CurrentOperator.Instance.GetCurrent(token);
            var actorTypeInt = (int)PolicyType.Undefined;
            if (crtOprtor != null)
                actorTypeInt = crtOprtor.IsMachine ? (int)ActorType.Machine : (int)ActorType.User;
            var operatorTypeStr = PolicyType.Undefined.ToString();
            var permittedOperatorTypeStr = entryLogPolicyDict.GetLdictValue("OperatorTypePolicy");
            var verifyAuthorizationRst = new PermissionHandler().VerifyAuthorization1(operatorTypeStr);
            if (verifyAuthorizationRst == PolicyType.Disallow) return;
            if (verifyAuthorizationRst == PolicyType.Allow)
            {
                DoSaveEntrylog(type, account, state, msg, actorTypeInt, entryLogPolicyDict);
                return;
            }

            var toLogByOperator = true;
            if (permittedOperatorTypeStr.ToLower() == PermittedOperatorType.OnlyUser.ToString().ToLower())
            {
                if (crtOprtor == null) return;
                if (crtOprtor.IsMachine) return;
            }
            else if (permittedOperatorTypeStr.ToLower() == PermittedOperatorType.OnlyMachine.ToString().ToLower())
            {
                if (crtOprtor == null) return;
                if (!crtOprtor.IsMachine) return;

            }
            else if (permittedOperatorTypeStr.ToLower() == PermittedOperatorType.UserAndMachine.ToString().ToLower())
            {
                if (crtOprtor == null) return;
            }
            else return;

            if (crtOprtor != null) toLogByOperator = JudgeByOperator(entryLogPolicyDict, crtOprtor);
            if (!toLogByOperator) return;

            DoSaveEntrylog(type, account, state, msg, actorTypeInt, entryLogPolicyDict);
        }
        public static void DoSaveEntrylog(int type, string account, bool state, string msg, int actorTypeInt, Dictionary<string, string> entryLogPolicyDict)
        {
            var entryLog = new EntryLog();
            entryLog.Type = type;
            entryLog.ApplicationServerId = GlobalContext.SystemSetting.ApplicationServerId;
            entryLog.State = state;
            entryLog.Account = account.GetSubString(31);
            entryLog.OperatorType = actorTypeInt;
            entryLog.Remark = msg.GetSubString(255);
            string ip = WebClientHelper.Ip;
            entryLog.IpAddress = ip.GetSubString(15);
            entryLog.IpLocation = string.Empty;
            //will be consuming too much time
            var logIpLocation = entryLogPolicyDict.GetLdictValue("LogIpLocation").JudgeJudgementFlag();
            if (logIpLocation)
                entryLog.IpLocation = IpHelper.GetIpLocation(ip).GetSubString(127);
            entryLog.Browser = WebClientHelper.Browser.GetSubString(255);
            entryLog.OS = WebClientHelper.GetOSVersion().GetSubString(255);
            entryLog.UserAgent = WebClientHelper.UserAgent.GetSubString(255);
            var nonEfRepository = new RunningLogDbRepository();
            nonEfRepository.SaveEntryLog(entryLog);

        }
        private static bool JudgeByOperator(Dictionary<string, string> dict, OperatorInfo crtOprtor)
        {
            var toLogByOperator = true;
            var operatorTypeStr = crtOprtor.IsMachine ? ActorType.Machine.ToString() : ActorType.Machine.ToString();

            var oprtPolicy = dict.GetLdictValue(operatorTypeStr + "Policy");
            if (oprtPolicy == PolicyOption.None.ToString())
            {
                toLogByOperator = false;
            }
            //else if (oprtPolicy == PolicyOption.Any.ToString())
            //{
            //    toLogByOperator = true;
            //}
            else if (oprtPolicy == PolicyOption.Some.ToString())
            {
                var permittedOprtsStr = dict.GetLdictValue(crtOprtor.IsMachine ? "Machines" : "Users");
                var permittedOprtsArr = permittedOprtsStr.GetLarrayArray(true, true);
                if (permittedOprtsArr == null) toLogByOperator = false;
                else if (!permittedOprtsArr.Contains(crtOprtor.ActorId)) toLogByOperator = false;

            }
            else if (oprtPolicy == PolicyOption.AllBut.ToString())
            {
                var excludedOprtsStr = dict.GetLdictValue(crtOprtor.IsMachine ? "Machines" : "Users");
                var excludedOprtsArr = excludedOprtsStr.GetLarrayArray(true, true);
                if (excludedOprtsArr.Contains(crtOprtor.ActorId)) toLogByOperator = false;
            }
            return toLogByOperator;
        }
    }
}
