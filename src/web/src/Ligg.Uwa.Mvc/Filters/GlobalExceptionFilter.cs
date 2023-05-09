using Ligg.Infrastructure.Utilities.LogUtil;
using Ligg.Uwa.Application.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System.Web;
using Ligg.Infrastructure.DataModels;


namespace Ligg.Uwa.Mvc
{
    public class GlobalExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            LogHelper.Error(context.Exception);
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                TResult rst = new TResult();
                var ex = context.Exception;

                rst.Message = context.Exception.GetOriginalException().Message;
                if (string.IsNullOrEmpty(rst.Message))
                {
                    rst.Message = "error occoured! ";
                }
                context.Result = new JsonResult(rst);
                context.ExceptionHandled = true;
            }
            else
            {

                string errMsg = context.Exception.GetOriginalException().Message;
                context.Result = new RedirectResult("~/Home/Error?msg=" + HttpUtility.UrlEncode(errMsg));
                context.ExceptionHandled = true;
            }
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}