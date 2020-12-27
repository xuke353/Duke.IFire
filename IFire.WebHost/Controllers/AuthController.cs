using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IFire.Application.Auths;
using IFire.Application.Auths.Dto;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;
using IFire.Framework.Interfaces;
using IFire.Framework.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IFire.WebHost.Controllers {

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;
        private readonly ILoginHandler _loginHandler;
        private readonly IConfigProvider _configProvider;

        public AuthController(IAuthService authService, ILoginHandler loginHandler, IConfigProvider configProvider) {
            _authService = authService;
            _loginHandler = loginHandler;
            _configProvider = configProvider;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [DisableAuditing]
        public async Task<IResultModel> Login(LoginInput input) {
            var result = await _authService.Login(input);
            return LoginHandle(result);
        }

        /// <summary>
        /// 登录处理
        /// </summary>
        private IResultModel LoginHandle(LoginResult result) {
            if (result.Success) {
                var auth = _configProvider.Get<AuthConfig>("Auth");
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                    new Claim(ClaimTypes.Name, result.Name),
                    new Claim(ClaimsName.Username, result.Username),
                    new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddSeconds(auth.Jwt.Expires * 60).ToString()),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.UtcNow.ToString("o")),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };
                return _loginHandler.Hand(claims, result.RefreshToken);
            }

            return ResultModel.Failed(result.Error);
        }
    }
}
