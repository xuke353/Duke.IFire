using System.Collections.Generic;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;

namespace IFire.Auth.Web {

    /// <summary>
    /// 权限验证
    /// </summary>
    [Singleton]
    public class PermissionValidateHandler : IPermissionValidateHandler {
        //private readonly ILoginInfo _loginInfo;
        //private readonly IAccountPermissionResolver _permissionResolver;

        //public PermissionValidateHandler(ILoginInfo loginInfo, IAccountPermissionResolver permissionResolver) {
        //    _loginInfo = loginInfo;
        //    _permissionResolver = permissionResolver;
        //}

        public async Task<bool> Validate(IDictionary<string, string> routeValues, string httpMethod) {
            //var permissions = await _permissionResolver.Resolve(_loginInfo.AccountId, _loginInfo.Platform);

            var controller = routeValues["controller"];
            var action = routeValues["action"];
            return true;//permissions.Any(m => m.EqualsIgnoreCase($"{controller}_{action}_{httpMethod}"));
        }
    }
}
