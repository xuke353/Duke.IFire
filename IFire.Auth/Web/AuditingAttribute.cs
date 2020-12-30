using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace IFire.Auth.Web {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuditingAttribute : Attribute, IAsyncActionFilter {

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            //如果禁用审计功能，直接走下一步
            if (!ShouldSaveAudit(context)) {
                return next();
            }

            var handler = context.HttpContext.RequestServices.GetService<IAuditingHandler>();

            return handler.Hand(context, next);
        }

        /// <summary>
        /// 判断是否禁用审计功能
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        private static bool ShouldSaveAudit(ActionExecutingContext context, bool defaultValue = false) {
            if (!(context.ActionDescriptor is ControllerActionDescriptor))
                return false;
            var methodInfo = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo;

            if (methodInfo == null) {
                return false;
            }

            if (!methodInfo.IsPublic) {
                return false;
            }

            if (methodInfo.HasAttribute<AuditingAttribute>()) {
                return true;
            }

            if (methodInfo.HasAttribute<DisableAuditingAttribute>()) {
                return false;
            }

            var classType = methodInfo.DeclaringType;
            if (classType != null) {
                if (classType.GetTypeInfo().HasAttribute<AuditingAttribute>()) {
                    return true;
                }

                if (classType.GetTypeInfo().HasAttribute<DisableAuditingAttribute>()) {
                    return false;
                }
            }
            return defaultValue;
        }
    }
}
