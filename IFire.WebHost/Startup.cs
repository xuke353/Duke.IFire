using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using AutoMapper;
using IFire.Auth.Jwt;
using IFire.Data.EFCore;
using IFire.Data.EFCore.Repositories;
using IFire.Domain.RepositoryIntefaces;
using IFire.Framework.Helpers;
using IFire.Framework.StartupServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

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
            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                // 注意: 只有在通过url段进行版本控制时，才需要此选项。替代格式还可以用于控制路由模板中API版本的格式。
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddHttpContextAccessor();
            services.AddControllers();
            AddSwaggerGen(services);

            services.AddDbContext<IFireDbContext>(option => {
                option.UseMySql(DbOptions.ConnectionString, new MySqlServerVersion(new Version(DbOptions.Version)));
                if (Environment.IsDevelopment()) {
                    //打印sql
                    option.UseLoggerFactory(EFLoggerFactory);
                    option.EnableSensitiveDataLogging(true);//显示sql参数
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                foreach (var description in provider.ApiVersionDescriptions) {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
                c.IndexStream = () => Assembly.GetExecutingAssembly().GetManifestResourceStream("IFire.WebHost.wwwroot.swagger.ui.index.html");               
            });
            app.UseStaticFiles();
            app.UseRouting();

            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }


        private static void AddSwaggerGen(IServiceCollection services) {
            services.AddSwaggerGen(options => {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions) {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo {
                        Title = "Duke.IFire API",
                        Version = description.ApiVersion.ToString(),
                        Description = @"<a target=""_blank"" href=""https://github.com/xuke353/AdmBoots"">GitHub</a> &nbsp; <a target=""_blank"" href=""/healthchecks-ui"">健康检查</a> &nbsp; <code>Powered by .NET5</code>"
                    });
                }
                options.DocInclusionPredicate((_, _) => true);
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme() {
                    Description = "JWT认证请求头格式: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                var basePath = AppContext.BaseDirectory;
                options.IncludeXmlComments(Path.Combine(basePath, "IFire.Application.xml"));
                options.IncludeXmlComments(Path.Combine(basePath, "IFire.WebHost.xml"));
            });
            services.AddApiVersioning(option => option.ReportApiVersions = true);
            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                // 注意: 只有在通过url段进行版本控制时，才需要此选项。替代格式还可以用于控制路由模板中API版本的格式。
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
