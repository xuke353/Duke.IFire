using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IFire.Framework.Abstractions;

namespace IFire.Models {
    [Table("Account_Auth_Info")]
    public class AccountAuthInfo:Entity {

        /// <summary>
        /// 账户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        [MaxLength(50)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 刷新令牌过期时间
        /// </summary>
        public DateTime RefreshTokenExpiredTime { get; set; }

        /// <summary>
        /// 最后登录时间戳
        /// </summary>        
        public long LoginTime { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [MaxLength(50)]
        public string LoginIP { get; set; } = string.Empty;
    }
}
