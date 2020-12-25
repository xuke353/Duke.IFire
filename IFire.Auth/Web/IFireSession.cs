using System;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;

namespace IFire.Auth.Web {

    [Transient]
    public class IFireSession : IIFireSession {
        public int UserId => throw new NotImplementedException();

        public string Username => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public bool IsAuthenticated => throw new NotImplementedException();
    }
}
