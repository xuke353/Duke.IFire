using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IFire.WebHost.Controllers {

    [AllowAnonymous]
    public class AuthController : ControllerAbstract {
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
        public async Task<IActionResult> Login(LoginInput input) {
            var result = await _authService.Login(input);
            return Ok(LoginHandle(result));
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
                    new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddSeconds(auth.Jwt.Expires * 60).ToString()),
                    new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.UtcNow.ToString("o")),
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
        public async Task<IResultModel> RefreshToken([BindRequired] string refreshToken) {
            var result = await _authService.RefreshToken(refreshToken);
            return LoginHandle(result);
        }

        [HttpGet]
        public Task<IResultModel> AuthInfo() {
            return _authService.GetAuthInfo();
        }
    }
}
