﻿using System.Security.Claims;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;
using IFire.Framework.Extensions;
using Microsoft.AspNetCore.Http;

namespace IFire.Auth.Web {

    [Transient]
    public class IFireSession : IIFireSession {
        private readonly ClaimsIdentity _claimsIdentity;

        public IFireSession(IHttpContextAccessor accessor) {
            _claimsIdentity = accessor?.HttpContext?.User.Identity as ClaimsIdentity;
        }

        public int? UserId {
            get {
                var claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                return claim?.Value.ToInt();
            }
        }

        public string Name => _claimsIdentity.Name;

        public string Username => _claimsIdentity.FindFirst(ClaimsName.Username)?.Value;

        public bool IsAuthenticated => _claimsIdentity.IsAuthenticated;
    }
}
