using System;
using Autofac.Extensions.DependencyInjection;
using IFire.Framework.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IFire.WebHost {

    public class Program {

        public static int Main(string[] args) {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false);

            var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environmentVariable.NotNull()) {
                configBuilder.AddJsonFile($"appsettings.{environmentVariable}.json", false);
            }
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configBuilder.Build())
                .CreateLogger();
            try {
                Log.Information("启动程序...");
                CreateHostBuilder(args).Build().Run();
                return 0;
            } catch (Exception ex) {
                Log.Fatal(ex, "程序意外终止");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder => {
                webBuilder
                .UseSerilog()
                .ConfigureKestrel(serverOptions => {
                    serverOptions.AllowSynchronousIO = true;//启用同步 IO
                })
                .UseStartup<Startup>();
                //.ConfigureAppConfiguration(cfgBuilder => {
                //    cfgBuilder.AddJsonFile("ipratelimit.json", optional: true, reloadOnChange: true);
                //});
            });
    }
}
