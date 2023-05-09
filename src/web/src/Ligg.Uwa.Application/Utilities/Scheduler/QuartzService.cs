using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Ligg.Web.Application.Service
{
    public class QuartzService : IHostedService, IDisposable
    {
        private readonly ILogger<QuartzService> _logger;
        private IScheduler _scheduler;

        public QuartzService(ILogger<QuartzService> logger, IScheduler scheduler)
        {
            _logger = logger;
            _scheduler = scheduler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scheduled job load started.");
            _scheduler.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _scheduler.Shutdown();
            _logger.LogInformation("Scheduled job load stopped.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
           
        }
    }
}