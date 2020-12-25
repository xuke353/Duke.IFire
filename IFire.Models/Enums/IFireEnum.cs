using System;
using System.Collections.Generic;
using System.Text;

namespace IFire.Models.Enums {
    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountType {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown = -1,
        /// <summary>
        /// 管理员
        /// </summary>
        Admin,
        /// <summary>
        /// 人员
        /// </summary>
        User,
        /// <summary>
        /// 企业
        /// </summary>
        Enterprise
    }

    /// <summary>
    /// 账户状态
    /// </summary>
    public enum AccountStatus {
        激活,
        禁用,
        注销
    }
}
