﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;

namespace IFire.Application.Auths {
    [Transient]
    public class UserPermissionResolver : IFireAppServiceBase, IUserPermissionResolver {
        public Task<IList<string>> Resolve(int userId) {
            throw new NotImplementedException();
        }

        public Task<IList<string>> ResolveButtons(Guid userId) {
            throw new NotImplementedException();
        }

        public Task<IList<string>> ResolvePages(Guid userId) {
            throw new NotImplementedException();
        }
    }
}
