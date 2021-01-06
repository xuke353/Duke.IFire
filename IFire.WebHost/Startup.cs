using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AutoMapper;
using IFire.Auth.Jwt;
using IFire.Data.EFCore;
using IFire.Framework.Helpers;
using IFire.Framework.Providers;
using IFire.WebHost.Middlewares;
using IFire.WebHost.ServiceCollection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            services.AddSwaggerGen();

            services.AddDbContext<IFireDbContext>(option => {
                option.UseMySql(DbOptions.ConnectionString, new MySqlServerVersion(new Version(DbOptions.Version)));
                if (Environment.IsDevelopment()) {
                    //打印sql
                    option.UseLoggerFactory(EFLoggerFactory);
                    option.EnableSensitiveDataLogging(true);//显示sql参数
                }
            });

            services.AddControllers().AddNewtonsoftJson(options => {
                //设置日期格式化格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            services.AddMiniProfiler(options => {
                options.RouteBasePath = "/profiler";
            }).AddEntityFramework();
            //API URL转小写
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddCors(options => {
                options.AddPolicy("Default",
                    builder =>
                    builder.SetIsOriginAllowed(origin => true)//允许所有 origin 来源
                                                              //.WithOrigins(Configuration["Startup:Cors"].Split(','))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
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
            //跨域
            app.UseCors("Default");
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
