using IFire.Framework.Attributes;
using IFire.Framework.Extensions;
using IFire.Framework.Interfaces;

namespace IFire.Framework.Abstractions {

    /// <summary>
    /// 缓存配置
    /// </summary>
    [Section("Cache")]
    public class CacheConfig : IConfig {

        /// <summary>
        /// 缓存提供器
        /// </summary>
        public CacheProvider Provider { get; set; } = CacheProvider.MemoryCache;

        /// <summary>
        /// Redis配置
        /// </summary>
        public RedisConfig Redis { get; set; } = new RedisConfig();
    }

    public class RedisConfig {
        private string _instanceName;

        /// <summary>
        /// 实例名
        /// </summary>
        public string InstanceName {
            get => _instanceName.NotNull() ? _instanceName : "IFireRedis";
            set => _instanceName = value;
        }

        private string _connnectionString;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString {
            get => _connnectionString.NotNull() ? _connnectionString : "127.0.0.1";
            set => _connnectionString = value;
        }
    }

    /// <summary>
    /// 缓存提供器
    /// </summary>
    public enum CacheProvider {

        /// <summary>
        /// 内存缓存
        /// </summary>
        MemoryCache,

        /// <summary>
        /// Redis
        /// </summary>
        Redis
    }
}
