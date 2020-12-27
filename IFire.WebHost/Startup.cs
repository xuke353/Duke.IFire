using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using IFire.Auth.Jwt;
using IFire.Data.EFCore;
using IFire.Data.EFCore.Repositories;
using IFire.Domain.RepositoryIntefaces;
using IFire.Framework.Helpers;
using IFire.Framework.StartupServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IFire.WebHost", Version = "v1" });
            });

            services.AddDbContext<IFireDbContext>(option => {
                option.UseMySql(DbOptions.ConnectionString, new MySqlServerVersion(new Version(DbOptions.Version)));
                if (Environment.IsDevelopment()) {
                    //打印sql
                    option.UseLoggerFactory(EFLoggerFactory);
                    option.EnableSensitiveDataLogging(true);//显示sql参数
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IFire.WebHost v1"));
            }

            app.UseRouting();

            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
