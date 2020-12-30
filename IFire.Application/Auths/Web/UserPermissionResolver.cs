using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;
using Microsoft.Extensions.Caching.Distributed;

namespace IFire.Application.Auths.Web {

    [Transient]
    public class UserPermissionResolver : IFireAppServiceBase, IUserPermissionResolver {
        private readonly IDistributedCache _cache;

        public UserPermissionResolver(IDistributedCache cache) {
            _cache = cache;
        }

        public async Task<IList<string>> Resolve(int userId) {
            var list = new List<string> {
                "WeatherForecast_Get_Get"
            };
            return list;
        }

        public Task<IList<string>> ResolveButtons(Guid userId) {
            throw new NotImplementedException();
        }

        public Task<IList<string>> ResolvePages(Guid userId) {
            throw new NotImplementedException();
        }
    }
}
