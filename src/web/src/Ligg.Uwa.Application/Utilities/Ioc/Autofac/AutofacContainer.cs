
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.Quartz;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using IContainer = Autofac.IContainer;

namespace Ligg.Uwa.Application.Utilities
{
    public static class AutofacContainer
    {
        private static IContainer _container;

        public static void ConfigureContainer(ContainerBuilder builder)
        {

            //注册数据库基础操作和工作单元
            builder.RegisterGeneric(typeof(DbSetRepository<,,>)).As(typeof(IDbSetRepository<,,>));
            builder.RegisterGeneric(typeof(UnitWork<>)).As(typeof(IUnitWork<>));

            //注册app层
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            if (GlobalContext.SystemSetting.CacheProvider == "Enyim")
                builder.RegisterType(typeof(EnyimCacheHandler)).As(typeof(ICacheHandler));
            else if (GlobalContext.SystemSetting.CacheProvider == "Redis")
                builder.RegisterType(typeof(RedisCacheHandler)).As(typeof(ICacheHandler));
            else
            {
                builder.RegisterType(typeof(MemoryCacheHandler)).As(typeof(ICacheHandler));
            }

            builder.RegisterType(typeof(HttpContextAccessor)).As(typeof(IHttpContextAccessor));
            RegisterAssemblyTypes(builder);
            builder.RegisterModule(new QuartzAutofacFactoryModule());
        }


        public static IContainer ConfigureServices(IServiceCollection services)   //use for unit testing 
        {
            var builder = new ContainerBuilder();

            //注册数据库基础操作和工作单元
            services.AddScoped(typeof(IDbSetRepository<,,>), typeof(DbSetRepository<,,>));
            services.AddScoped(typeof(IUnitWork<>), typeof(UnitWork<>));


            //注册app层
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            //防止单元测试时已经注入
            if (services.All(u => u.ServiceType != typeof(ICacheHandler)))
            {
                if (GlobalContext.SystemSetting.CacheProvider == "Memory")
                    services.AddScoped(typeof(ICacheHandler), typeof(MemoryCacheHandler));
                else if (GlobalContext.SystemSetting.CacheProvider == "Redis")
                    services.AddScoped(typeof(ICacheHandler), typeof(RedisCacheHandler));
                else services.AddScoped(typeof(ICacheHandler), typeof(EnyimCacheHandler));
            }

            if (services.All(u => u.ServiceType != typeof(IHttpContextAccessor)))
            {
                services.AddScoped(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            }

            RegisterAssemblyTypes(builder);

            builder.RegisterModule(new QuartzAutofacFactoryModule());

            builder.Populate(services);

            _container = builder.Build();
            return _container;

        }

        /// 注入所有继承了IDependency接口
        private static void RegisterAssemblyTypes(ContainerBuilder builder)
        {
            Type baseType = typeof(IDependency);
            var compilationLibrary = DependencyContext.Default
                .CompileLibraries
                .Where(x => !x.Serviceable
                            && x.Type == "project")
                .ToList();
            var count1 = compilationLibrary.Count;
            List<Assembly> assemblyList = new List<Assembly>();

            foreach (var _compilation in compilationLibrary)
            {
                try
                {
                    assemblyList.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(_compilation.Name)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(_compilation.Name + ex.Message);
                }
            }

            builder.RegisterAssemblyTypes(assemblyList.ToArray())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf().AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

    }
}