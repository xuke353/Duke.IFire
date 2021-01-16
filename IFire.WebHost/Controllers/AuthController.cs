using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IFire.Application.Auths;
using IFire.Application.Auths.Dto;
using IFire.Auth.Abstractions;
using IFire.Auth.Web;
using IFire.Framework.Interfaces;
using IFire.Framework.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IFire.WebHost.Controllers {

    [Description("身份认证")]
    public class AuthController : IFireControllerBase {
        private readonly IAuthService _authService;
        private readonly ILoginHandler _loginHandler;
        private readonly IConfigProvider _configProvider;

        public AuthController(IAuthService authService, ILoginHandler loginHandler, IConfigProvider configProvider) {
            _authService = authService;
            _loginHandler = loginHandler;
            _configProvider = configProvider;
        }

        [HttpPost]
        [AllowAnonymous]
        [DisableAuditing]
        [Description("登陆")]
        public async Task<IResultModel> Login(LoginInput input) {
            var result = await _authService.Login(input);
            return LoginHandle(result);
        }

        /// <summary>
        /// 登录处理
        /// </summary>
        private IResultModel LoginHandle(LoginResult result) {
            if (result.Success) {
                var auth = _configProvider.Get<AuthConfig>();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                    new Claim(ClaimTypes.Name, result.Name),
                    new Claim(ClaimsName.Username, result.Username),
                    new Claim(ClaimTypes.Expiration, result.LoginTime.AddSeconds(auth.Jwt.Expires * 60).ToString()),
                    new Claim(JwtRegisteredClaimNames.AuthTime, result.LoginTime.ToString("o")),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };
                return _loginHandler.Hand(claims, result.RefreshToken);
            }

            return ResultModel.Failed(result.Error);
        }

        [HttpGet]
        [AllowAnonymous]
        [DisableAuditing]
        [Description("刷新token")]
        public async Task<IResultModel> RefreshToken([BindRequired] string refreshToken) {
            var result = await _authService.RefreshToken(refreshToken);
            return LoginHandle(result);
        }

        [HttpGet]
        [Description("获取认证信息")]
        public async Task<IResultModel> AuthInfo() {
            return ResultModel.Success(await _authService.GetAuthInfo());
        }
    }
}
