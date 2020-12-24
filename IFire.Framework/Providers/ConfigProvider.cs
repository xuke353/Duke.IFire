using System;
using System.Collections.Generic;
using System.Text;
using IFire.Framework.Attributes;
using IFire.Framework.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IFire.Framework.Providers {

    [Singleton]
    public class ConfigProvider : IConfigProvider {
        private readonly IConfiguration _configuration;

        public ConfigProvider(IConfiguration configuration) {
            _configuration = configuration;
        }

        public TConfig Get<TConfig>(string section = "") where TConfig : IConfig, new() {
            IConfig config = new TConfig();
            _configuration.GetSection(string.IsNullOrEmpty(section) ? nameof(TConfig) : section).Bind(config);
            return (TConfig)config;
        }
    }
}
