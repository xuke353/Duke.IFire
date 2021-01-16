using IFire.Framework.Attributes;
using IFire.Framework.Providers;
using Microsoft.AspNetCore.Http;

namespace IFire.Framework.Helpers {

    /// <summary>
    /// IP帮助类
    /// </summary>
    [Transient]
    public class NetHelper {
        public static HttpContext HttpContext => IocProvider.Current.Resolve<IHttpContextAccessor>().HttpContext;

        /// <summary>
        /// 获取当前用户IP
        /// </summary>
        public static string ClientIP {
            get {
                var ip = HttpContext?.Connection?.RemoteIpAddress.ToString();
                if (HttpContext != null && HttpContext.Request != null) {
                    if (HttpContext.Request.Headers.ContainsKey("X-Real-IP")) {
                        ip = HttpContext.Request.Headers["X-Real-IP"].ToString();
                    }

                    if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For")) {
                        ip = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
                    }
                }
                return ip;
            }
        }

        /// <summary>
        /// 获取当前用户请求的User-Agent
        /// </summary>
        public static string UserAgent {
            get {
                return HttpContext?.Request == null ? "" : (string)HttpContext.Request.Headers["User-Agent"];
            }
        }
    }
}
