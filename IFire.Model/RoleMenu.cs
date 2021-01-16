using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.Framework.Abstractions;

namespace IFire.Model {

    /// <summary>
    /// 角色菜单
    /// </summary>
    [Table("Role_Menu")]
    public partial class RoleMenu : Entity<int> {

        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 菜单编号
        /// </summary>
        public int MenuId { get; set; }
    }
}
