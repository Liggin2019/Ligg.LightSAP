using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;

namespace Ligg.Uwa.WebApi.Controllers
{
    [Route("sys/[controller]/[action]")]
    [ApiController]
    [AuthorizationFilter]
    public class PermissionController : BaseController
    {
        private readonly ConfigService _service;
        public PermissionController(ConfigService configService)
        {
            _service = configService;
        }
        [HttpGet]
        public TResult<string> GetUnavailableClientViews([FromQuery] string portal)
        {
            var errCode = GetErrorCode();

            if (portal.IsNullOrEmpty()) return new TResult<string>(0, null, "illegal request,ErrCode=" + errCode + "-1");
            var token = JwtHelper.GetToken(Request);
            var pmsHandler = new PermissionHandler();
            var codes = pmsHandler.GetUnavailableClientViews(portal, token);

            var rst = new TResult<string>(1, codes);
            return rst;
        }

        [HttpGet]
        public TResult<string> GetUnavailableClientViewButtons([FromQuery] string view)
        {
            var errCode = GetErrorCode();
            var viewKey = view;
            if (viewKey.IsNullOrEmpty()) return new TResult<string>(0, null, "illegal request,ErrCode=" + errCode + "-1");
            var cacheHandler = new CacheHandler();
            var configHandler = new ConfigHandler();
            var masterId = ((int)DevConfigSubType.ClientView).ToString();
            var views = configHandler.GetConfigItems(masterId).FindAll(x => x.Status);
            var viewObj = views.Find(x=>x.Key.ToLower()== viewKey.ToLower());
            if(viewObj == null) return new TResult<string>(0, null, "illegal request,ErrCode=" + errCode + "-2");
  
            var token = JwtHelper.GetToken(Request);
            var pmsHandler = new PermissionHandler();
            var codes = pmsHandler.GetUnavailableClientButtons(viewObj, token);
  
            var rst = new TResult<string>(1, codes);
            return rst;
        }



    }

}
