using System;

namespace Ligg.Uwa.Application.Utilities
{
    // 全局静态获取服务
    public class AutofacResolver
    {
        static private IServiceProvider _provider;
        public static void ConfigServiceProvider(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }
        public static TService GetService<TService>() where TService : class
        {
            Type typeParameterType = typeof(TService);
            return (TService)_provider.GetService(typeParameterType);
        }
    }

    public interface IDependency
    {
    }
}
