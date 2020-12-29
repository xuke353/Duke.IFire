using Microsoft.AspNetCore.Builder;

namespace IFire.WebHost.Middlewares {

    public static class MiddlewareExtensions {

        public static IApplicationBuilder UseExceptionHandle(this IApplicationBuilder app) {
            app.UseMiddleware<ExceptionHandleMiddleware>();

            return app;
        }
    }
}
