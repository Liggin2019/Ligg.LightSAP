using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Ligg.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.WebApi.Controllers
{
    [Route("sys/[controller]/[action]")]
    [ApiController]
    [AuthorizationFilter]
    public class UserController : BaseController
    {
        private readonly UserService _service;
        public UserController(UserService userService)
        {
            _service = userService;
        }

        [HttpPost]
        public async Task<TResult> Logon([FromQuery] string account, [FromQuery] string password)
        {
            var rst = new TResult(0, "账号或密码错误，请重新输入");
            var oprtRst = await _service.Login(account, password, (int)WebClientType.Mvc);
            string token = null;
            if (oprtRst.Flag == 1)
            {
                var oprt = oprtRst.Data;
                var userRepo = new UserDbRepository();
                userRepo.UpdateOperator(oprt);
                token = CurrentOperator.Instance.AddCurrent(oprt.Id.ToString());
                rst.Flag = 1;
                rst.Message = token;
            }
            else
            {
                RunningLogHelper.SaveEntrylog(0, account, false, oprtRst.Message, token);
            }
            RunningLogHelper.SaveEntrylog(0, account, true, "Log in", token);
            return rst;
        }

        [HttpGet]
        public async Task<TResult> GetCurrentUser()
        {
            GetErrorCode();
            string token = JwtHelper.GetToken(Request);
            var rst = new TResult(0, "User does not exist");
            if (token.IsNullOrEmpty()) return rst;
            var currentOprtor = CurrentOperator.Instance.GetCurrent(token);
            if (currentOprtor == null) return rst;
            var mdl = currentOprtor.MapTo<ShowSelfModel>();
            //mdl.Id = currentOprtor.ActorId;
            if (mdl != null)
            {
                rst = new  TResult<ShowSelfModel>(1, mdl);
                return rst;
            }
            else return rst;
        }

        [HttpPost]
        public async Task<TResult> Logoff()
        {
            var rst = new TResult(0, "User does not exist");
            string token = JwtHelper.GetToken(Request);
            var crtOprtor = CurrentOperator.Instance.GetCurrent(token);
            if (crtOprtor == null) return rst;
            if (crtOprtor.IsMachine == true) { rst.Message = "illegal request";  return rst; }

            RunningLogHelper.SaveEntrylog(1, crtOprtor.Account, true, "Log off", token);
            CurrentOperator.Instance.RemoveCurrent();
            return new TResult(1);
        }
    }
}
