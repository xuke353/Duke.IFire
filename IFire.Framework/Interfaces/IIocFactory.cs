using System;
using Microsoft.Extensions.DependencyInjection;

namespace IFire.Framework.Interfaces {

    public interface IIocFactory {

        T Resolve<T>();

        T Resolve<T>(Type type);

        bool IsRegistered(Type type);

        bool IsRegistered<T>();

        T ResolveFromScope<T>(IServiceScope scope);

        T ResolveFromScope<T>(Type type, IServiceScope scope);

        IServiceScope CreateScope();
    }
}
