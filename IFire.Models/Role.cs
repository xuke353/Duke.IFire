using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IFire.Framework.Abstractions;

namespace IFire.Models {

    [Table("Role")]
    public class Role : Entity<Guid> {

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(300)]
        public string Remarks { get; set; }
    }
}
