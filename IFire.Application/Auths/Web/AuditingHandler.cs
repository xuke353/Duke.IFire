using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using IFire.Application.AuditInfos;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;
using IFire.Framework.Extensions;
using IFire.Framework.Interfaces;
using IFire.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace IFire.Application.Auths.Web {

    /// <summary>
    /// 审计日志处理
    /// </summary>
    [Transient]
    public class AuditingHandler : IAuditingHandler {
        private readonly IIFireSession _iFireSession;
        private readonly IAuditInfoService _auditInfoService;
        private readonly ILogger _logger;
        private readonly IConfigProvider _configProvider;

        public AuditingHandler(IIFireSession iFireSession, IAuditInfoService auditInfoService, ILogger<AuditingHandler> logger, IConfigProvider configProvider) {
            _iFireSession = iFireSession;
            _auditInfoService = auditInfoService;
            _logger = logger;
            _configProvider = configProvider;
        }

        public async Task Hand(ActionExecutingContext context, ActionExecutionDelegate next) {
            var config = _configProvider.Get<AuthConfig>();
            if (!config.Auditing) {
                await next();
                return;
            }

            var routeValues = context.ActionDescriptor.RouteValues;
            var stopwatch = Stopwatch.StartNew();

            var auditInfo = new AuditInfo {
                UserId = _iFireSession?.UserId,
                Username = _iFireSession?.Name,
                Controller = routeValues["controller"],
                Action = routeValues["action"],
                Parameters = JsonSerializer.Serialize(context.ActionArguments).TruncateWithPostfix(2000),
                ExecutionTime = DateTime.Now,
                BrowserInfo = context.HttpContext.Request.Headers["User-Agent"],
                ClientIp = context.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
            };

            ActionExecutedContext resultContext = null;
            try {
                resultContext = await next();
                if (resultContext.Exception != null && !resultContext.ExceptionHandled) {
                    auditInfo.Exception = resultContext.Exception.Message;
                }
            } catch (Exception ex) {
                auditInfo.Exception = ex.Message;
                throw;
            } finally {
                stopwatch.Stop();
                auditInfo.ExecutionDuration = stopwatch.Elapsed.TotalMilliseconds.ToInt();

                if (resultContext != null) {
                    //执行结果
                    if (resultContext.Result is ObjectResult result) {
                        auditInfo.Result = JsonSerializer.Serialize(result.Value).TruncateWithPostfix(2000);
                    }
                }
                Console.WriteLine(auditInfo.ToString());
                try {
                    await _auditInfoService.Add(auditInfo);
                } catch (Exception ex) {
                    _logger.LogError("审计日志插入异常：{@ex}", ex);
                }
            }
        }
    }
}
