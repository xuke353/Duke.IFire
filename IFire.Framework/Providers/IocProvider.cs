using System.Runtime.CompilerServices;
using IFire.Framework.Interfaces;

namespace IFire.Framework.Providers {

    public class IocProvider {

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IIocFactory SetFactory(IIocFactory provider) {
            if (Current == null) {
                Current = provider;
            }
            return Current;
        }

        public static IIocFactory Current { get; private set; }
    }
}
