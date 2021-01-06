using IFire.Framework.Attributes;
using IFire.Framework.Interfaces;

namespace IFire.Framework.Abstractions {

    /// <summary>
    /// 系统配置
    /// </summary>
    [Section("System")]
    public class SystemConfig : IConfig {

        /// <summary>
        /// 系统标题
        /// </summary>
        public string Title { get; set; }
    }
}
