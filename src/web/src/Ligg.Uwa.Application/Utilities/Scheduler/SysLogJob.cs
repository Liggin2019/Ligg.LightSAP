//using System.Threading.Tasks;
//using Quartz;
//using Ligg.Uwa.Business.OpenJobs;
//using Microsoft.Extensions.Logging;
//using Ligg.Uwa.Application.Shared;

//namespace Ligg.Uwa.Business.SysLogs
//{
//    public class SysLogJob : IJob
//    {
//        private SysLogApp _sysLogApp;
//        private OpenJobApp _openJobApp;
//        private ILogger<SysLogJob> _logger;

//        public SysLogJob(SysLogApp sysLogApp, OpenJobApp openJobApp)
//        {
//            _sysLogApp = sysLogApp;
//            _openJobApp = openJobApp;
//        }

//        public Task Execute(IJobExecutionContext context)
//        {
//            var jobId = context.MergedJobDataMap.GetString("Consts.JOBMAPKEY");
//            //todo:这里可以加入自己的自动任务逻辑
//            _logger.LogInformation($"运行了自动任务：{jobId}");
//            _openJobApp.RecordRun(jobId);
//            return Task.Delay(1);
//        }
//    }
//}