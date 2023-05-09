using Ligg.Infrastructure.Utilities.LogUtil;
using Ligg.Uwa.Application.Shared;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.WebApi
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            LogHelper.Error(exception);
            var code = HttpStatusCode.OK;
            var result = JsonConvert.SerializeObject(new TResult
            {
                Flag = 0,
                Message = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
