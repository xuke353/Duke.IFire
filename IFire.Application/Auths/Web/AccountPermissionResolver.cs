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
using IFire.Models.Enums;

namespace IFire.Application.Auths.Web {

    [Transient]
    public class AccountPermissionResolver : IAccountPermissionResolver {
        private readonly IDistributedCache _cache;
        private readonly IRepository<RolePermission, int> _rolePermissionRepository;
        private readonly IRepository<AccountRole, int> _accountRoleRepository;
        private readonly IRepository<RoleMenu, int> _roleMenuRepository;
        private readonly IRepository<Menu, int> _menuRepository;

        public AccountPermissionResolver(IDistributedCache cache,
            IRepository<RolePermission, int> rolePermissionRepository,
            IRepository<AccountRole, int> accountRoleRepository,
            IRepository<RoleMenu, int> roleMenuRepository,
            IRepository<Menu, int> menuRepository) {
            _cache = cache;
            _rolePermissionRepository = rolePermissionRepository;
            _accountRoleRepository = accountRoleRepository;
            _roleMenuRepository = roleMenuRepository;
            _menuRepository = menuRepository;
        }

        public async Task<IList<string>> Resolve(int userId) {
            if (userId == 0)
                return new List<string>();

            var key = $"{CacheKeys.ACCOUNT_PERMISSIONS}{userId}";

            if (!_cache.TryGetValue(key, out IList<string> list)) {
                list = await (from rp in _rolePermissionRepository.GetAll()
                              join ar in _accountRoleRepository.GetAll()
                              on rp.RoleId equals ar.RoleId
                              where ar.UserId == userId
                              select rp.PermissionCode)
                        .AsNoTracking().ToListAsync();
                await _cache.SetAsync(key, list);
            }

            return list;
        }

        public async Task<IList<AccountMenuItem>> ResolveMenus(int userId) {
            var key = $"{CacheKeys.ACCOUNT_MENUS}{userId}";
            if (!_cache.TryGetValue(key, out List<AccountMenuItem> tree)) {
                var all = await (from m in _menuRepository.GetAll()
                                 join rm in _roleMenuRepository.GetAll()
                                 on m.Id equals rm.MenuId
                                 join ar in _accountRoleRepository.GetAll()
                                 on rm.RoleId equals ar.RoleId
                                 where ar.UserId == userId && m.Show
                                 select new AccountMenuItem {
                                     Id = m.Id,
                                     ParentId = m.ParentId,
                                     Type = m.Type,
                                     Name = m.Name,
                                     Route = m.Route,
                                     Url = m.Url,
                                     Icon = m.Icon,
                                     Show = m.Show,
                                     Code = m.Code,
                                     Level = m.Level,
                                     Sort = m.Sort
                                 }).AsNoTracking().ToListAsync();

                tree = all.Where(e => !e.ParentId.HasValue).OrderBy(e => e.Sort).ToList();

                tree.ForEach(menu => {
                    if (menu.Type == MenuType.节点)
                        SetChildren(menu, all);
                });

                await _cache.SetAsync(key, tree);
            }

            return tree;
        }

        private void SetChildren(AccountMenuItem parent, List<AccountMenuItem> all) {
            parent.Children = all.Where(e => e.ParentId == parent.Id).OrderBy(e => e.Sort).ToList();

            if (parent.Children.Any()) {
                parent.Children.ForEach(menu => {
                    if (menu.Type == MenuType.节点)
                        SetChildren(menu, all);
                });
            }
        }
    }
}
