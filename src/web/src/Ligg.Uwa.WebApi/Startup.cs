using Autofac;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Application.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Ligg.Uwa.WebApi
{
    public class Startup
    {
        private IConfiguration _configuration { get; }
        private IWebHostEnvironment _webHostEnvironment { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            GlobalContext.LogWhenStart(env);
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
            services.AddControllers(options =>
            {
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            })
            .AddNewtonsoftJson(
                options =>
                {
                    // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver 是小写
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }
            );
            services.AddControllersWithViews();
            //services.AddSession();

            services.AddSwaggerGen(options =>
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ligg.Uwa.WebApi", Version = "v1" });
                options.IncludeXmlComments(xmlPath, true);
            });
            services.AddOptions();
            services.AddMemoryCache();
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // 注册Encoding

            GlobalContext.SystemSetting = _configuration.GetSection("SystemSetting").Get<SystemSetting>();
            GlobalContext.Services = services;
            GlobalContext.Configuration = _configuration;
            GlobalContext.SetDbHelper();
            GlobalContext.SetLogHelper();
            GlobalContext.LogWhenStart(_webHostEnvironment);
            GlobalContext.CheckAppSetting();

            if (GlobalContext.SystemSetting.CorsPolicy.ToLower() != "regardless") services.AddCors();

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                GlobalContext.SystemSetting.Debug = true;
                app.UseDeveloperExceptionPage();
            }

            //app.UseSession();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api";
                c.SwaggerEndpoint("v1/swagger.json", "Ligg.Uwa.WebApi v1");
            });

            app.UseMiddleware(typeof(GlobalExceptionMiddleware));

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (string.IsNullOrEmpty(GlobalContext.SystemSetting.CorsPolicy))
            {
                var policyOption = GlobalContext.SystemSetting.CorsPolicy ?? "any";
                var corsSites = GlobalContext.SystemSetting.CorsSites ?? "";
                var onlyCorsSite = GlobalContext.SystemSetting.OnlyCorsSite ?? "";
                if (policyOption.ToLower() == PolicyOption.Any.ToString().ToLower()) app.UseCors("AnyCors");
                else if (policyOption.ToLower() == PolicyOption.Some.ToString().ToLower())
                {
                    if (!string.IsNullOrEmpty(corsSites))
                    {
                        app.UseCors(builder =>
                        {
                            builder.WithOrigins(corsSites).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                        });
                    }
                }
                else if (policyOption.ToLower() == PolicyOption.None.ToString().ToLower())
                {
                    if (!string.IsNullOrEmpty(onlyCorsSite))
                    {
                        app.UseCors(builder =>
                        {
                            builder.WithOrigins(onlyCorsSite.Split(',')).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                        });
                    }
                }
            }

            GlobalContext.ServiceProvider = app.ApplicationServices;
        }
    }
}
