using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.Framework.Abstractions;

namespace IFire.Models {

    /// <summary>
    /// 账户角色
    /// </summary>
    [Table("Account_Role")]
    public class AccountRole : Entity<int> {

        /// <summary>
        /// 账户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public Guid RoleId { get; set; }
    }
}
