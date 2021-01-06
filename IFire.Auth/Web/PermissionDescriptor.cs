namespace IFire.Auth.Web {

    /// <summary>
    /// 权限描述符
    /// </summary>
    public class PermissionDescriptor {

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 控制名称
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 唯一编码
        /// </summary>
        public string Code => $"{Controller}_{Action}_{HttpMethod}".ToLower();
    }
}
