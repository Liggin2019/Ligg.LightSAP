using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Ligg.Uwa.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureLogging((hostingContext, logging) =>
        {
            logging.ClearProviders(); //去掉默认的日志
            logging.AddFilter("System", LogLevel.Error);
            logging.AddFilter("Microsoft", LogLevel.Error);
            //logging.SetMinimumLevel(LogLevel.Trace);
            //logging.AddLog4Net();
            logging.AddNLog("nlog.config");
        })
       .UseServiceProviderFactory(new AutofacServiceProviderFactory())  //将默认ServiceProviderFactory指定为AutofacServiceProviderFactory

       .ConfigureWebHostDefaults(webBuilder =>
       {
           //webBuilder.UseUrls("http://*:4000/sys");
           //following is ok 
           webBuilder.UseStartup<Startup>();
           //webBuilder.UseStartup<Startup>().UseUrls("http://*:81");
           //webBuilder.UseIIS().UseStartup<Startup>();
       });

    }

}
