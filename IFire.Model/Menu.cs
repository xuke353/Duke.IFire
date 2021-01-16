using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IFire.Application.Auths.Web;
using IFire.Framework.Abstractions;

namespace IFire.Model {

    [Table("Menu")]
    public class Menu : Entity {

        /// <summary>
        /// 页面编码
        /// </summary>
        [MaxLength(200)]
        public string Code { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public MenuType Type { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        [MaxLength(300)]
        public string Route { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        [MaxLength(300)]
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [MaxLength(50)]
        public string Icon { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength]
        public string Remarks { get; set; }
    }
}
