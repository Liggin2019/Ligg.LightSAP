using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ligg.Uwa.Application.Shared;
using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [AuthorizationFilter]
    public class HomeController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController()
        {
        }

        //*test
        [HttpGet]
        public IEnumerable<WeatherForecast> Index()
        {
            
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public TResult<string> GetSession(string key)
        {
            var rst = new TResult<string>();
            var msg = SessionHelper.GetSession(key);
            rst.Flag = 0;
            rst.Message = msg;
            return rst;
        }

        [HttpPost]
        public TResult<string> SetSession(string key)
        {
            var rst = new TResult<string>();
            SessionHelper.WriteSession(key,key+"-value");
            rst.Flag = 0;
            rst.Message = SessionHelper.GetSession(key);
            return rst;
        }

        [HttpPost]
        public TResult<string> RemoveSession(string key)
        {
            var rst = new TResult<string>();
            SessionHelper.RemoveSession(key);
            rst.Flag = 0;
            rst.Message = SessionHelper.GetSession(key);
            return rst;
        }

    }
}

