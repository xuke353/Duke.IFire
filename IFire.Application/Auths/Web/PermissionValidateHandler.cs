using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;
using IFire.Framework.Extensions;

namespace IFire.Application.Auths.Web {

    /// <summary>
    /// 权限验证
    /// </summary>
    [Singleton]
    public class PermissionValidateHandler : IPermissionValidateHandler {
        private readonly IIFireSession _iFireSession;
        private readonly IAccountPermissionResolver _permissionResolver;

        public PermissionValidateHandler(IIFireSession iFireSession, IAccountPermissionResolver permissionResolver) {
            _iFireSession = iFireSession;
            _permissionResolver = permissionResolver;
        }

        public async Task<bool> Validate(IDictionary<string, string> routeValues, string httpMethod) {
            var permissions = await _permissionResolver.Resolve(_iFireSession.UserId.ToInt());

            var controller = routeValues["controller"];
            var action = routeValues["action"];
            return permissions.Any(m => m.EqualsIgnoreCase($"{controller}_{action}_{httpMethod}"));
        }
    }
}
