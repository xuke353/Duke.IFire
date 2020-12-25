using System;
using System.Linq;
using System.Reflection;
using IFire.Framework.Attributes;
using IFire.Framework.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace IFire.Framework.StartupServices {

    public static class DIExtension {

        /// <summary>
        /// 注册按约定命名的接口类
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly">程序集</param>
        /// <param name="fullNameEnd">特定字符串结尾</param>
        public static IServiceCollection AddImplementedInterfaceServices(this IServiceCollection services, string assemblyName, string endTypeName) {
            Check.NotNull(assemblyName, nameof(assemblyName), "注册失败，未找到程序集");
            var assembly = AssemblyHelper.LoadByNameEndString(assemblyName);
            var types = assembly.GetTypes();
            var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.FullName.EndsWith(endTypeName, StringComparison.OrdinalIgnoreCase));
            foreach (var serviceType in interfaces) {
                var implementationType = types.FirstOrDefault(m => m != serviceType && serviceType.IsAssignableFrom(m));
                if (implementationType != null) {
                    services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
                }
            }
            return services;
        }

        /// <summary>
        /// 从指定程序集中注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddServicesFromAssembly(this IServiceCollection services, Assembly assembly) {
            foreach (var type in assembly.GetTypes()) {

                #region ==单例注入==

                var singletonAttr = (SingletonAttribute)Attribute.GetCustomAttribute(type, typeof(SingletonAttribute));
                if (singletonAttr != null) {
                    //注入自身类型
                    if (singletonAttr.Itself) {
                        services.AddSingleton(type);
                        continue;
                    }

                    var interfaces = type.GetInterfaces().Where(m => m != typeof(IDisposable)).ToList();
                    if (interfaces.Any()) {
                        foreach (var i in interfaces) {
                            services.AddSingleton(i, type);
                        }
                    } else {
                        services.AddSingleton(type);
                    }

                    continue;
                }

                #endregion ==单例注入==

                #region ==瞬时注入==

                var transientAttr = (TransientAttribute)Attribute.GetCustomAttribute(type, typeof(TransientAttribute));
                if (transientAttr != null) {
                    //注入自身类型
                    if (transientAttr.Itself) {
                        services.AddSingleton(type);
                        continue;
                    }

                    var interfaces = type.GetInterfaces().Where(m => m != typeof(IDisposable)).ToList();
                    if (interfaces.Any()) {
                        foreach (var i in interfaces) {
                            services.AddTransient(i, type);
                        }
                    } else {
                        services.AddTransient(type);
                    }
                    continue;
                }

                #endregion ==瞬时注入==

                #region ==Scoped注入==

                var scopedAttr = (ScopedAttribute)Attribute.GetCustomAttribute(type, typeof(ScopedAttribute));
                if (scopedAttr != null) {
                    //注入自身类型
                    if (scopedAttr.Itself) {
                        services.AddSingleton(type);
                        continue;
                    }

                    var interfaces = type.GetInterfaces().Where(m => m != typeof(IDisposable)).ToList();
                    if (interfaces.Any()) {
                        foreach (var i in interfaces) {
                            services.AddScoped(i, type);
                        }
                    } else {
                        services.AddScoped(type);
                    }
                }

                #endregion ==Scoped注入==
            }

            return services;
        }

        /// <summary>
        /// 注入所有服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWithAttributeServices(this IServiceCollection services) {
            var assemblies = AssemblyHelper.Load();
            foreach (var assembly in assemblies) {
                services.AddServicesFromAssembly(assembly);
            }
            return services;
        }
    }
}
