using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace IFire.Framework.Extensions {

    public static class DistributedCacheExtensions {

        //设置默认 半小时过期
        private static readonly DistributedCacheEntryOptions s_defaultOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        public static TItem Get<TItem>(this IDistributedCache cache, string key) {
            try {
                var valueString = cache.GetString(key);

                if (string.IsNullOrEmpty(valueString)) {
                    return default;
                }

                return JsonSerializer.Deserialize<TItem>(valueString);
            } catch (Exception) {
                return default;
            }
        }

        public static async Task<TItem> GetAsync<TItem>(this IDistributedCache cache, string key, CancellationToken token = default(CancellationToken)) {
            try {
                var valueString = await cache.GetStringAsync(key, token);

                if (string.IsNullOrEmpty(valueString)) {
                    return default;
                }

                return JsonSerializer.Deserialize<TItem>(valueString);
            } catch (Exception) {
                return default;
            }
        }

        public static bool TryGetValue<TItem>(this IDistributedCache cache, string key, out TItem value) {
            var valueString = cache.GetString(key);
            if (!string.IsNullOrEmpty(valueString)) {
                value = JsonSerializer.Deserialize<TItem>(valueString);
                return true;
            }
            value = default;
            return false;
        }

        public static void Set<TItem>(this IDistributedCache cache, string key, TItem value) {
            cache.SetString(key, JsonSerializer.Serialize(value), s_defaultOptions);
        }

        public static void Set<TItem>(this IDistributedCache cache, string key, TItem value, DistributedCacheEntryOptions options) {
            cache.SetString(key, JsonSerializer.Serialize(value), options);
        }

        public static async Task SetAsync<TItem>(this IDistributedCache cache, string key, TItem value, CancellationToken token = default) {
            await cache.SetStringAsync(key, JsonSerializer.Serialize(value), s_defaultOptions, token);
        }

        public static async Task SetAsync<TItem>(this IDistributedCache cache, string key, TItem value, DistributedCacheEntryOptions options, CancellationToken token = default) {
            await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options, token);
        }

        public static TItem GetOrCreate<TItem>(this IDistributedCache cache, string key, Func<TItem> factory) {
            if (!cache.TryGetValue(key, out TItem obj)) {
                obj = factory();
                cache.Set(key, obj);
            }
            return obj;
        }

        public static TItem GetOrCreate<TItem>(this IDistributedCache cache, string key, Func<DistributedCacheEntryOptions, TItem> factory) {
            if (!cache.TryGetValue(key, out TItem obj)) {
                var options = new DistributedCacheEntryOptions();
                obj = factory.Invoke(options);
                cache.Set(key, obj, options);
            }
            return obj;
        }

        public static async Task<TItem> GetOrCreateAsync<TItem>(this IDistributedCache cache, string key, Func<Task<TItem>> factory) {
            if (!cache.TryGetValue(key, out TItem obj)) {
                obj = await factory();
                await cache.SetAsync(key, obj);
            }
            return obj;
        }

        public static async Task<TItem> GetOrCreateAsync<TItem>(this IDistributedCache cache, string key, Func<DistributedCacheEntryOptions, Task<TItem>> factory) {
            if (!cache.TryGetValue(key, out TItem obj)) {
                var options = new DistributedCacheEntryOptions();
                obj = await factory.Invoke(options);
                await cache.SetAsync(key, obj, options);
            }
            return obj;
        }

        //跟上面两个方法一样
        public static TItem GetOrCreate<TItem>(this IDistributedCache cache, string key, Func<TItem> factory, DistributedCacheEntryOptions options) {
            if (!cache.TryGetValue(key, out TItem obj)) {
                obj = factory();
                cache.Set(key, obj, options);
            }
            return obj;
        }

        public static async Task<TItem> GetOrCreateAsync<TItem>(this IDistributedCache cache, string key, Func<Task<TItem>> factory, DistributedCacheEntryOptions options) {
            if (!cache.TryGetValue(key, out TItem obj)) {
                obj = await factory();
                await cache.SetAsync(key, obj, options);
            }
            return obj;
        }
    }
}
