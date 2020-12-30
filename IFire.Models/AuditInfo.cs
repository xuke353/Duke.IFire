using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IFire.Framework.Abstractions;

namespace IFire.Models {

    /// <summary>
    ///
    /// </summary>
    [Table("Audit_Info")]
    public class AuditInfo : Entity {

        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        [MaxLength(100)]
        public string Controller { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        [MaxLength(100)]
        public string Action { get; set; }

        /// <summary>
        /// 调用参数
        /// </summary>
        [MaxLength]
        public string Parameters { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        [MaxLength]
        public string Result { get; set; }

        /// <summary>
        /// 方法执行的开始时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 方法调用的总持续时间（毫秒）
        /// </summary>
        public int ExecutionDuration { get; set; }

        /// <summary>
        /// 客户端的IP地址
        /// </summary>
        [MaxLength(100)]
        public string ClientIp { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        [MaxLength(1000)]
        public string BrowserInfo { get; set; }

        /// <summary>
        /// 方法执行期间发生异常
        /// </summary>
        [MaxLength(500)]
        public string Exception { get; set; }

        public override string ToString() {
            var loggedUserId = UserId.HasValue
                                   ? "user " + Username
                                   : "an anonymous user";

            var exceptionOrSuccessMessage = Exception != null
                ? "exception: " + Exception
                : "succeed";

            return $"AUDIT LOG: {Controller}.{Action} is executed by {loggedUserId} in {ExecutionDuration} ms from {ClientIp} IP address with {exceptionOrSuccessMessage}.";
        }
    }
}
