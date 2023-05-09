using Autofac;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Application.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.Logging;

namespace Ligg.Uwa.Mvc
{
    public class Startup
    {
        private IConfiguration _configuration { get; }
        private IWebHostEnvironment _webHostEnvironment { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _webHostEnvironment = env;
            GlobalContext.HostingEnvironment = env;
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            AutofacContainer.ConfigureContainer(builder);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                services.AddRazorPages().AddRazorRuntimeCompilation();
            }
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddMemoryCache();  //如果 MemoryCacheHandler以构造函数方式解析，可以取消
            services.AddSession();
            services.AddHttpContextAccessor();

            services.AddOptions();
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // 注册Encoding

            GlobalContext.SystemSetting = _configuration.GetSection("SystemSetting").Get<SystemSetting>();
            GlobalContext.Services = services;
            GlobalContext.Configuration = _configuration;
            GlobalContext.SetDbHelper();
            GlobalContext.SetLogHelper();
            GlobalContext.LogWhenStart(_webHostEnvironment);
            GlobalContext.CheckAppSetting();

            var dbType = GlobalContext.SystemSetting.DbProvider;
            if (dbType == Consts.DBTYPE_MYSQL)
            {
                var connectionString = GlobalContext.SystemSetting.DbConnectionString;
                services.AddDbContext<DbSetContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), p => p.CommandTimeout(GlobalContext.SystemSetting.DbCommandTimeoutSeconds)));
            }
            else
            {
                services.AddDbContext<DbSetContext>(options => options.UseSqlServer(GlobalContext.SystemSetting.DbConnectionString));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                GlobalContext.SystemSetting.Debug = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            string resource = Path.Combine(env.ContentRootPath, "Resource");
            //string resource = Path.Combine("D:\\tmp1\\Mwf", "Resource"); 
            Directory.CreateDirectory(resource);
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Resource",
                FileProvider = new PhysicalFileProvider(resource),
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseSession();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{index?}/{obj?}/{objid?}/{option?}");
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{index?}/{obj?}/{objid?}/{option?}");
            });

            GlobalContext.ServiceProvider = app.ApplicationServices;
        }
    }
}
