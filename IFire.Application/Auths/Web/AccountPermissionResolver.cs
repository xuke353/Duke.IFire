using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Domain;
using IFire.Framework.Attributes;
using IFire.Framework.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using IFire.Domain.RepositoryIntefaces;
using IFire.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace IFire.Application.Auths.Web {

    [Transient]
    public class AccountPermissionResolver : IAccountPermissionResolver {
        private readonly IDistributedCache _cache;
        private readonly IRepository<RolePermission, int> _rolePermissionRepository;
        private readonly IRepository<AccountRole, int> _accountRoleRepository;

        public AccountPermissionResolver(IDistributedCache cache,
            IRepository<RolePermission, int> rolePermissionRepository,
            IRepository<AccountRole, int> accountRoleRepository) {
            _cache = cache;
            _rolePermissionRepository = rolePermissionRepository;
            _accountRoleRepository = accountRoleRepository;
        }

        public async Task<IList<string>> Resolve(int userId) {
            if (userId == 0)
                return new List<string>();

            //var key = $"{CacheKeys.ACCOUNT_PERMISSIONS}{userId}";

            //if (!_cache.TryGetValue(key, out IList<string> list)) {
            //    list = await (from rp in _rolePermissionRepository.GetAll()
            //                  join ar in _accountRoleRepository.GetAll()
            //                  on rp.RoleId equals ar.RoleId
            //                  where ar.UserId == userId
            //                  select rp.PermissionCode)
            //            .AsNoTracking().ToListAsync();
            //    await _cache.SetAsync(key, list);
            //}

            //return list;

            var list = new List<string> {
                "WeatherForecast_Get_Get", "Permission_Tree_Get"
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
