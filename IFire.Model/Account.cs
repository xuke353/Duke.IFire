using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IFire.Framework.Abstractions;
using IFire.Framework.Result;
using IFire.Model.Enums;

namespace IFire.Model {

    /// <summary>
    /// 账户
    /// </summary>
    [Table("Account")]
    public class Account : Entity {

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "请输入用户名")]
        [MaxLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请输入名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 账户状态
        /// </summary>
        public AccountStatus Status { get; set; } = AccountStatus.激活;

        /// <summary>
        /// 类型
        /// </summary>
        public AccountType Type { get; set; }

        /// <summary>
        /// 账户是否锁定(锁定后不允许在账户管理中修改)
        /// </summary>
        public bool IsLock { get; set; }

        /// <summary>
        /// 绑定角色列表
        /// </summary>
        [Required(ErrorMessage = "请选择角色")]
        [NotMapped]
        public List<int> Roles { get; set; }

        public bool Deleted { get; set; }

        /// <summary>
        /// 账户检测
        /// </summary>
        public IResultModel Check() {
            if (Deleted || Status == AccountStatus.注销)
                return ResultModel.Failed("账户不存在");

            if (Status == AccountStatus.禁用)
                return ResultModel.Failed("账户已禁用，请联系管理员");

            return ResultModel.Success();
        }
    }
}
