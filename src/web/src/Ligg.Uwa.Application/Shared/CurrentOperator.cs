using Ligg.Uwa.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using Ligg.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace Ligg.Uwa.Application.Shared
{
    public class CurrentOperator
    {
        public static CurrentOperator Instance
        {
            get { return new CurrentOperator(); }
        }
        //private readonly CacheHandler _cacheHandler = new CacheHandler();
        private readonly ICacheHandler _cacheManager = GlobalContext.ServiceProvider?.GetService<ICacheHandler>();
        private string _logonMode = GlobalContext.SystemSetting.LogonMode.ToLower();
        private string _tokenKey = Consts.TOKEN_KEY; //cookie name or session name

        public OperatorInfo GetCurrent(string token = null)
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            var oprtId = "";
            switch (_logonMode)
            {
                case "cookie":
                    if (hca.HttpContext != null)
                    {
                        token = CookieHelper.GetCookie(_tokenKey);
                        if (token.IsNullOrEmpty()) return null;
                        //oprtId = token.Split('-')[0];
                        oprtId=JwtHelper.FindClaim(token); //it somewhat consuming time
                    }
                    break;

                case "session":
                    if (hca.HttpContext != null)
                    {
                        token = SessionHelper.GetSession(_tokenKey);
                        if (token.IsNullOrEmpty()) return null;
                        token = string.IsNullOrEmpty(token) ? string.Empty : token.Trim('"');
                        //oprtId = token.Split('-')[0];
                        oprtId=JwtHelper.FindClaim(token); //it somewhat consuming time
                    }
                    break;
                default://webapi
                    if (hca.HttpContext != null)
                    {
                        oprtId = JwtHelper.FindClaim(token);
                    }
                    break;
            }

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var oprtInfo = _cacheManager.GetCache<OperatorInfo>(oprtId);
            if (oprtInfo != null)
            {
                var exclusiveOnline = GlobalContext.SystemSetting.ExclusiveOnline;
                if (exclusiveOnline)
                {
                    if (oprtInfo.Token != token) return null;
                }
                oprtInfo.Token = token;
            }
            //oprtInfo.Token = "";
            return oprtInfo;
        }

        public string AddCurrent(string oprtId)
        {
            var dbRepo = new UserDbRepository();
            OperatorInfo oprtInfo = dbRepo.GetOperatorInfo(oprtId);

            //var token = oprtId + "-" + GlobalContext.SystemSetting.ApplicationServerId + "-" + Guid.NewGuid().GetShortGuid();
            var token = JwtHelper.GenerateToken(oprtInfo);
            switch (_logonMode)
            {
                case "cookie":
                    CookieHelper.WriteCookie(_tokenKey, token);
                    break;
                case "session":
                    SessionHelper.WriteSession(_tokenKey, token);
                    break;
                default:
                    //token = JwtHelper.GenerateToken(oprtInfo);
                    break;
            }

            oprtInfo.Token = token;
            if (oprtInfo != null)
            {
                _cacheManager.SetCache(oprtId, oprtInfo);
            }
            return token;
        }

        public void RemoveCurrent(string token = "")
        {
            switch (_logonMode)
            {
                case "cookie":
                    CookieHelper.RemoveCookie(_tokenKey);
                    break;
                case "session":
                    SessionHelper.RemoveSession(_tokenKey);
                    break;
                default:
                    var exclusiveOnline = GlobalContext.SystemSetting.ExclusiveOnline;
                    if (exclusiveOnline)
                    {
                        var oprtInfo = GetCurrent(token);
                        if (oprtInfo != null)
                        {
                            oprtInfo.Token = "";
                            _cacheManager.SetCache(oprtInfo.Id, oprtInfo);
                        }
                    }
                    break;
            }
        }
    }
}
