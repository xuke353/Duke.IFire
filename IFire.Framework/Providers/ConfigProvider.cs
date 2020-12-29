using IFire.Framework.Attributes;
using IFire.Framework.Extensions;
using IFire.Framework.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IFire.Framework.Providers {

    [Singleton]
    public class ConfigProvider : IConfigProvider {
        private readonly IConfiguration _configuration;

        public ConfigProvider(IConfiguration configuration) {
            _configuration = configuration;
        }

        public TConfig Get<TConfig>() where TConfig : IConfig, new() {
            IConfig config = new TConfig();
            var section = config.GetType().GetAttribute<SectionAttribute>();
            var sectionName = nameof(TConfig);
            if (section != null) {
                if (!string.IsNullOrEmpty(section.Name)) {
                    sectionName = section.Name;
                }
            }
            _configuration.GetSection(sectionName).Bind(config);
            return (TConfig)config;
        }
    }
}
