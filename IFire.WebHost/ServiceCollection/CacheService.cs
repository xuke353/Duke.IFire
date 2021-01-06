using IFire.Framework.Abstractions;
using IFire.Framework.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace IFire.WebHost.ServiceCollection {

    public static class CacheService {

        /// <summary>
        /// 分布式缓存 注入接口IDistributedCache
        /// </summary>
        /// <param name="services"></param>
        public static void AddCacheService(this IServiceCollection services) {
            var configProvider = services.BuildServiceProvider().GetService<IConfigProvider>();
            var cacheConfig = configProvider.Get<CacheConfig>();

            if (cacheConfig.Provider == CacheProvider.Redis) {
                var redis = cacheConfig.Redis;

                //添加数据保护服务，设置统一应用程序名称，并指定使用Reids存储私钥
                services.AddDataProtection()
                    .SetApplicationName("Duke.IFire")
                    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redis.ConnectionString), "DataProtection-Keys");

                //添加Redis缓存用于分布式Session
                services.AddStackExchangeRedisCache(options => {
                    options.Configuration = redis.ConnectionString;
                    options.InstanceName = redis.InstanceName;
                });
            } else {
                services.AddDistributedMemoryCache();
            }
        }
    }
}
