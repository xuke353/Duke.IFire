using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace IFire.Domain {
    public class IFireDomainModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(m => m.Name.EndsWith("Domain"))
            .AsImplementedInterfaces();
        }
    }
}
