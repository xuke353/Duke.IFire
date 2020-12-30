using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using AutoMapper;
using IFire.Auth.Jwt;
using IFire.Data.EFCore;
using IFire.Framework.Helpers;
using IFire.Framework.Providers;
using IFire.Framework.StartupServices;
using IFire.WebHost.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace IFire.WebHost {

    public class Startup {
        public static readonly ILoggerFactory EFLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public List<Assembly> Assemblies { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration) {
            Configuration = configuration;
            Environment = environment;
            Assemblies = AssemblyHelper.Load();
            DbOptions.InitConfiguration(configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder) {
            //注册Module
            builder.RegisterAssemblyModules(Assemblies.ToArray());
        }

        public void ConfigureServices(IServiceCollection services) {
            //通用特性方式的DI
            services.RegisterAssemblyServices(Assemblies.ToArray());
            services.AddJwtAuth();
            services.AddCacheService();
            services.AddAutoMapper(Assembly.Load("IFire.Application"));
            services.AddHttpContextAccessor();

            AddSwaggerGen(services);

            services.AddDbContext<IFireDbContext>(option => {
                option.UseMySql(DbOptions.ConnectionString, new MySqlServerVersion(new Version(DbOptions.Version)),
                    p => p.MigrationsAssembly("IFire.Data"));
                if (Environment.IsDevelopment()) {
                    //打印sql
                    option.UseLoggerFactory(EFLoggerFactory);
                    option.EnableSensitiveDataLogging(true);//显示sql参数
                }
            });

            services.AddControllers().AddNewtonsoftJson();

            services.AddMiniProfiler(options => {
                options.RouteBasePath = "/profiler";
            }).AddEntityFramework();
            //API URL转小写
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            IocProvider.SetFactory(new IocFactory(app.ApplicationServices));
            //全局异常处理（在最上面）
            app.UseExceptionHandle();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                foreach (var description in provider.ApiVersionDescriptions) {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("IFire.WebHost.wwwroot.swagger.ui.index.html");
            });
            app.UseStaticFiles();
            app.UseRouting();

            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();
            app.UseMiniProfiler();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

        private static void AddSwaggerGen(IServiceCollection services) {
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
}
