using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Ligg.Uwa.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())  //��Ĭ��ServiceProviderFactoryָ��ΪAutofacServiceProviderFactory
                .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.ClearProviders(); //ȥ��Ĭ�ϵ���־
                     logging.AddFilter("System", LogLevel.Error);
                     logging.AddFilter("Microsoft", LogLevel.Error);
                     logging.SetMinimumLevel(LogLevel.Trace);
                     //logging.AddLog4Net();
                     logging.AddNLog("nlog.config");
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseStartup<Startup>();
                    webBuilder.UseStartup<Startup>().UseUrls("http://*:86");
                });
    }
}

