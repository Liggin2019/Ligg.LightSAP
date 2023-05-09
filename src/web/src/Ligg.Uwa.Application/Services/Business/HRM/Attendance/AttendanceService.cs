using Ligg.EntityFramework.Entities;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Ligg.Uwa.Business.HRM
{
    public class AttendanceService
    {

        //*manage
        public async Task<List<string>> GetManageDtosAsync(CommonReqArgs param)
        {
            var dtos =new List<string>();
            dtos.Add("material1");
            dtos.Add("material2");
            dtos.Add("material3");
            return dtos;
        }


    }
}
