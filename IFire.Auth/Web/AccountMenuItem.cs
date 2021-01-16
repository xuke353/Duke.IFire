using System.Collections.Generic;

namespace IFire.Application.Auths.Web {

    public class AccountMenuItem {
        public int Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
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
        public string Name { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
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
        /// 子菜单
        /// </summary>
        public List<AccountMenuItem> Children { get; set; }
    }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType {
        未知 = 1,
        节点 = 2,
        路由 = 3,
        链接 = 4
    }
}
