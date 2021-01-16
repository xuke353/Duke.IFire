using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using IFire.Framework.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IFire.WebHost.ServiceCollection {

    public static class SwaggerGenService {

        public static void AddSwaggerGen(this IServiceCollection services) {
            services.AddApiVersioning(option => option.ReportApiVersions = true);
            services.AddSwaggerGen(c => {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions) {
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo {
                        Title = "Duke.IFire API",
                        Version = description.GroupName,
                        Description = @"<a target=""_blank"" href=""https://github.com/xuke353/AdmBoots"">GitHub</a> &nbsp; <a target=""_blank"" href=""/healthchecks-ui"">健康检查</a> &nbsp; <code>Powered by .NET5</code>"
                    });
                }
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme() {
                    Description = "JWT认证请求头格式: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                //描述信息处理
                c.DocumentFilter<DescriptionDocumentFilter>();
                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //启用oauth2安全授权访问api接口
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                var basePath = AppContext.BaseDirectory;
                c.IncludeXmlComments(Path.Combine(basePath, "IFire.Application.xml"));
                c.IncludeXmlComments(Path.Combine(basePath, "IFire.WebHost.xml"));
            });

            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                // 注意: 只有在通过url段进行版本控制时，才需要此选项。替代格式还可以用于控制路由模板中API版本的格式。
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }

    /// <summary>
    /// 控制器和方法的描述信息处理
    /// </summary>
    public class DescriptionDocumentFilter : IDocumentFilter {

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) {
            SetControllerDescription(swaggerDoc, context);
            SetActionDescription(swaggerDoc, context);
        }

        /// <summary>
        /// 设置控制器的描述信息
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        private static void SetControllerDescription(OpenApiDocument swaggerDoc, DocumentFilterContext context) {
            if (swaggerDoc.Tags == null)
                swaggerDoc.Tags = new List<OpenApiTag>();

            foreach (var apiDescription in context.ApiDescriptions) {
                if (apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) && methodInfo.DeclaringType != null) {
                    var descAttr = (DescriptionAttribute)Attribute.GetCustomAttribute(methodInfo.DeclaringType, typeof(DescriptionAttribute));
                    if (descAttr != null && descAttr.Description.NotNull()) {
                        var controllerName = methodInfo.DeclaringType.Name;
                        controllerName = controllerName.Remove(controllerName.Length - 10);
                        if (swaggerDoc.Tags.All(t => t.Name != controllerName)) {
                            swaggerDoc.Tags.Add(new OpenApiTag {
                                Name = controllerName,
                                Description = descAttr.Description
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置方法的说明
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        private static void SetActionDescription(OpenApiDocument swaggerDoc, DocumentFilterContext context) {
            foreach (var path in swaggerDoc.Paths) {
                if (TryGetActionDescription(path.Key, context, out string description)) {
                    if (path.Value != null && path.Value.Operations != null && path.Value.Operations.Any()) {
                        var operation = path.Value.Operations.FirstOrDefault();
                        operation.Value.Description = description;
                        operation.Value.Summary = description;
                    }
                }
            }
        }

        /// <summary>
        /// 获取说明
        /// </summary>
        private static bool TryGetActionDescription(string path, DocumentFilterContext context, out string description) {
            foreach (var apiDescription in context.ApiDescriptions) {
                var apiPath = "/" + apiDescription.RelativePath.ToLower();
                if (apiPath.Equals(path) && apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)) {
                    var descAttr = (DescriptionAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(DescriptionAttribute));
                    if (descAttr != null && descAttr.Description.NotNull()) {
                        description = descAttr.Description;
                        return true;
                    }
                }
            }

            description = "";
            return false;
        }
    }
}
