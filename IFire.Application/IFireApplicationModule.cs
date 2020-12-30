using Autofac;
using Autofac.Extras.DynamicProxy;
using IFire.Application.Auths.Web;
using IFire.Auth.Abstractions;
using IFire.Data.EFCore.Uow;

namespace IFire.Application {

    public class IFireApplicationModule : Module {

        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<AuditingHandler>().As<IAuditingHandler>();
            builder.RegisterType<UnitOfWorkInterceptor>().AsSelf();
            builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(m => m.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .PropertiesAutowired()                       //支持属性注入
            .EnableInterfaceInterceptors()               //启用接口拦截
            .InterceptedBy(typeof(UnitOfWorkInterceptor));
        }
    }
}
