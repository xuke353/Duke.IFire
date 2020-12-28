namespace IFire.Models.Enums {

    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountType {
        未知 = -1,
        管理员 = 1,
        人员 = 2,
        企业 = 3
    }

    /// <summary>
    /// 账户状态
    /// </summary>
    public enum AccountStatus {
        激活 = 1,
        禁用 = 2,
        注销 = 3
    }
}
