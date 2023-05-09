using ExpressionBuilder.Configuration;
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Utilities.DbUtil;
using Ligg.Infrastructure.Utilities.LogUtil;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Text;
using System.Linq;


namespace Ligg.Uwa.Application.Shared
{
    public static class GlobalContext
    {
        /// All registered service and class instance container. Which are used for dependency injection.
        public static IServiceCollection Services { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

        public static IConfiguration Configuration { get; set; }

        public static IWebHostEnvironment HostingEnvironment { get; set; }

        public static SystemSetting SystemSetting { get; set; }

        public static ExpressionBuilderConfig ExpressionBuilder { get; set; }

        public static string GetVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return version.Major + "." + version.Minor;
        }

        /// 程序启动时，记录目录
        public static void LogWhenStart(IWebHostEnvironment env)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("程序启动");
            sb.Append(" |ContentRootPath:" + env.ContentRootPath);
            sb.Append(" |WebRootPath:" + env.WebRootPath);
            sb.Append(" |IsDevelopment:" + env.IsDevelopment());
            LogHelper.Debug(sb.ToString());
        }


        public static void SetCacheControl(StaticFileResponseContext context)
        {
            int second = 365 * 24 * 60 * 60;
            context.Context.Response.Headers.Add("Cache-Control", new[] { "public,max-age=" + second });
            context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123
        }
        public static void SetDbHelper()
        {
            DbHelper.DbHandler = new DbHandlerFactory(SystemSetting.DbProvider, SystemSetting.DbConnectionString, "");
        }

        public static void SetLogHelper()
        {
            LogHelper.LogHandler = new LogFactory(SystemSetting.LoggerProvider);
        }

        public static void CheckAppSetting()
        {
            CheckApplicationServer();
        }
        private static void CheckApplicationServer()
        {
            var applicationServerName = SystemSetting.ApplicationServer;
            var applicationServerNames = EnumHelper.GetNames<ApplicationServer>();
            if (!applicationServerNames.Contains(applicationServerName))
            {
                LogHelper.Error("Startup error: ApplicationServer setting error in appsettings.json" + ", ApplicationServer=" + applicationServerName + "; ApplicationServer should be in " + applicationServerNames.Unwrap(", "));
                throw new ArgumentNullException();
            }
            SystemSetting.ApplicationServerId = (int)EnumHelper.GetByName(applicationServerName, ApplicationServer.MvcBasis);

            var logMode = SystemSetting.LogonMode;
            if (SystemSetting.ApplicationServer.StartsWith("Mvc") & logMode.ToLower() != "cookie" & logMode.ToLower() != "session")
            {
                LogHelper.Error("Startup error: LogonMode setting error in appsettings.json" + ", LogonMode=" + logMode + "; LogonMode should be Cokkie or Session under Mvc Application");
                throw new ArgumentNullException();
            }
            if (SystemSetting.ApplicationServer.StartsWith("Api"))
            {
                SystemSetting.LogonMode = "";  
                if (SystemSetting.JwtSecurityKey.IsNullOrEmpty())
                    LogHelper.Error("Startup error: JwtSecurityKey setting error in appsettings.json" + ", JwtSecurityKey can't be null  under WebApi Application");
            }
        }


    }
}
