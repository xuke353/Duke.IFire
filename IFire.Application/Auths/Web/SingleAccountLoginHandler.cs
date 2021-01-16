using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;
using IFire.Framework.Extensions;

namespace IFire.Application.Auths.Web {

    [Transient]
    public class SingleAccountLoginHandler : ISingleAccountLoginHandler {
        private readonly IAuthService _authService;
        private readonly IIFireSession _loginInfo;

        public SingleAccountLoginHandler(IAuthService authService, IIFireSession loginInfo) {
            _authService = authService;
            _loginInfo = loginInfo;
        }

        public async Task<bool> Validate() {
            var authInfo = await _authService.GetAuthInfo(_loginInfo.UserId.ToInt());
            return authInfo != null && authInfo.LoginTime != _loginInfo.LoginTime;
        }
    }
}
