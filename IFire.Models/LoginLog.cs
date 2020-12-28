using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IFire.Framework.Abstractions;

namespace IFire.Models {

    [Table("Login_Log")]
    public class LoginLog : Entity {

        /// <summary>
        /// 账户编号
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        [MaxLength(50)]
        public string IP { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [MaxLength(2000)]
        public string Error { get; set; }

        /// <summary>
        /// UA
        /// </summary>
        [MaxLength(1000)]
        public string UserAgent { get; set; }
    }
}
