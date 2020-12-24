using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using IFire.Framework.Interfaces;

namespace IFire.Framework.Providers {

    public class IocProvider {

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IIocFactory SetProvider(IIocFactory provider) {
            if (Current == null) {
                Current = provider;
            }
            return Current;
        }

        public static IIocFactory Current { get; private set; }
    }
}
