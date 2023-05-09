using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Business.HRM;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Ligg.Uwa.WebApi.Controllers
{
    [Route("hrm/[controller]/[action]")]
    [ApiController]
    [AuthorizationFilter]
    public class AttendanceController : BaseController
    {
        private readonly AttendanceService _service;
        public AttendanceController(AttendanceService attendanceService)
        {
            _service = attendanceService;
        }

        //#post
        [HttpPost]
        public async Task<TResult> AddMany([FromQuery] string orgId, [FromBody] List<Attendance> attendances)
        {
            return new TResult(1, "Import succeeded!");
        }

        [HttpGet]
        public async Task<TResult> GetPagedApproveModels([FromQuery] string month, [FromQuery] string orgId, [FromQuery] Pagination pagination)
        {
            if(month.IsNullOrEmpty())
            {
                var time = DateTime.Now.AddMonths(-1);
                month = time.ToString("yyyy-MM");
            }

            var data = new List<Attendance>();
            var obj = new Attendance();
            obj.Id = 1; obj.Name = "Mary"; obj.LeaveTime = 2.1F; obj.LeaveEarlyTime = 8.1F; obj.BeLateTime = 6.3F; obj.AbsentTime = 2.2F; obj.OverDutyTime = 3f; obj.WorkingTime = 150F; obj.Month = month;
            data.Add(obj);
            var obj1 = new Attendance();
            obj1.Id = 2; obj1.Name = "Lily"; obj1.LeaveTime = 5.1F; obj1.LeaveEarlyTime = 10.1F; obj1.BeLateTime = 2.4F; obj1.AbsentTime = 1F; obj1.OverDutyTime = 2f; obj1.WorkingTime = 150F; obj1.Month = month;
            data.Add(obj1);
            var rst = new TResult<List<Attendance>>(1, data);
            return rst;
        }
        [HttpPost]
        public async Task<TResult> Approve([FromQuery] string month,[FromQuery] string orgId)
        {
            var msg ="approved all in org-"+ orgId+" on "+ month;
            return new TResult(1, "Approve succeeded!");
        }

    }

    public class Attendance
    {
        public int Id;
        public string Name;
        public float LeaveTime;
        public float LeaveEarlyTime;
        public float BeLateTime;
        public float AbsentTime;
        public float OverDutyTime;
        public float WorkingTime;
        public string Month;
    }

}
