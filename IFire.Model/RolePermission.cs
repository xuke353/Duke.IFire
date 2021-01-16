using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IFire.Framework.Abstractions;

namespace IFire.Model {

    [Table("Role_Permission")]
    public class RolePermission : Entity<int> {

        /// <summary>
        /// 角色
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        [MaxLength(200)]
        public string PermissionCode { get; set; }
    }
}
