using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;

namespace IFire.Auth.Web {

    [Transient]
    public class IFireSession : IIFireSession {
    }
}
