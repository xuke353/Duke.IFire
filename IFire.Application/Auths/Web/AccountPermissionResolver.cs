using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;
using Microsoft.Extensions.Caching.Distributed;

namespace IFire.Application.Auths.Web {

    [Transient]
    public class AccountPermissionResolver : IFireAppServiceBase, IAccountPermissionResolver {
        private readonly IDistributedCache _cache;

        public AccountPermissionResolver(IDistributedCache cache) {
            _cache = cache;
        }

        public async Task<IList<string>> Resolve(int userId) {
            var list = new List<string> {
                "WeatherForecast_Get_Get"
            };
            return list;
        }

        public Task<IList<string>> ResolveButtons(int userId) {
            throw new NotImplementedException();
        }

        public Task<IList<string>> ResolvePages(int userId) {
            throw new NotImplementedException();
        }
    }
}
