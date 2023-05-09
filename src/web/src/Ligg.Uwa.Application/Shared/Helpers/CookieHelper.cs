
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ligg.Uwa.Application.Shared
{
    public static class CookieHelper
    {
        public static void WriteCookie(string name, string value, bool httpOnly = true)
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);
            option.HttpOnly = httpOnly;
            option.SameSite = SameSiteMode.Lax;
            hca?.HttpContext?.Response.Cookies.Append(name, value, option);
        }

        //httpOnly=true 代表浏览器的js不能获取到的cookie
        public static void WriteCookie(string name, string value, int expires, bool httpOnly = true)
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(expires);
            option.HttpOnly = httpOnly;
            option.SameSite = SameSiteMode.Lax;
            hca?.HttpContext?.Response.Cookies.Append(name, value, option);
        }

        public static string GetCookie(string name)
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            return hca?.HttpContext?.Request.Cookies[name];
        }

        public static void RemoveCookie(string name)
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Response.Cookies.Delete(name);
        }
    }
}
