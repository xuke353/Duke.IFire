using System;
using System.Text;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IFire.Auth.Jwt {

    public static class ServiceCollectionExtensions {

        /// <summary>
        /// 添加Jwt认证
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddJwtAuth(this IServiceCollection services) {
            var configProvider = services.BuildServiceProvider().GetService<IConfigProvider>();
            var jwtConfig = configProvider.Get<AuthConfig>().Jwt;
            services.AddAuthentication(o => {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options => {
                   options.TokenValidationParameters = new TokenValidationParameters {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidIssuer = jwtConfig.Issuer,
                       ValidAudience = jwtConfig.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
                   };
                   options.Events = new JwtBearerEvents {
                       OnAuthenticationFailed = context => {
                           // 如果过期，则把<是否过期>添加到，返回头信息中
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException)) {
                               context.Response.Headers.Add("Token-Expired", "true");
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

            //注入权限集合
            //services.AddScoped<PermissionCollection>();

            return services;
        }
    }
}
