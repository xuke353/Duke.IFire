﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;
using IFire.Framework.Interfaces;
using IFire.Framework.Result;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IFire.Auth.Jwt {

    [Singleton]
    public class JwtLoginHandler : ILoginHandler {
        private readonly ILogger _logger;
        private readonly IConfigProvider _configProvider;

        public JwtLoginHandler(ILogger<JwtLoginHandler> logger, IConfigProvider configProvider) {
            _logger = logger;
            _configProvider = configProvider;
        }

        public IResultModel Hand(List<Claim> claims, string extendData) {
            var options = _configProvider.Get<AuthConfig>().Jwt;

            var token = Build(claims, options);

            _logger.LogDebug("生成JwtToken：{token}", token);

            var model = new JwtTokenModel {
                AccessToken = token,
                ExpiresIn = options.Expires * 60,
                RefreshToken = extendData
            };

            return ResultModel.Success(model);
        }

        private static string Build(List<Claim> claims, JwtConfig config) {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(config.Issuer, config.Audience, claims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(config.Expires), signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
