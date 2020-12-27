using System;
using Autofac;
using IFire.Data.EFCore.Repositories;
using IFire.Data.EFCore.Uow;
using IFire.Domain.RepositoryIntefaces;

namespace IFire.Data {
    public class IFireDataModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<IFireUnitOfWork>().As<IIFireUnitOfWork>();
            builder.RegisterGeneric(typeof(IFireRepository<,>)).As(typeof(IRepository<,>));
            builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(m => m.Name.EndsWith("Repository"))
            .AsImplementedInterfaces();
        }
    }
}
