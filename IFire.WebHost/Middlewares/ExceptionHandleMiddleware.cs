using System;
using System.Threading.Tasks;
using IFire.Framework.CustomExceptions;
using IFire.Framework.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IFire.WebHost.Middlewares {

    public class ExceptionHandleMiddleware {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionHandleMiddleware> _logger;

        public ExceptionHandleMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionHandleMiddleware> logger) {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext) {
            try {
                await _next(httpContext);
            } catch (Exception ex) {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex) {
            object result;
            if (ex.GetType() == typeof(BusinessException)) {
                context.Response.StatusCode = StatusCodes.Status200OK;
                result = ResultModel.Failed(ex.Message);
            } else {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                result = _env.IsDevelopment() ? ex.Message : "服务器发生了意外的内部错误";
                //记录到日志
                _logger.LogError($" ↓\r\n【异常信息】：{ex.Message} \r\n【异常类型】：{ex.GetType().Name} \r\n【堆栈调用】：{ex.StackTrace}");
            }

            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonConvert.SerializeObject(result,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }
    }
}
