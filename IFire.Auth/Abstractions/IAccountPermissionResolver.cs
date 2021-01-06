using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFire.Auth.Abstractions {

    public interface IAccountPermissionResolver {

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        Task<IList<string>> Resolve(int userId);

        /// <summary>
        /// 解析菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //Task<IList<AccountMenuItem>> ResolveMenus(Guid userId);

        /// <summary>
        /// 解析页面编码列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<string>> ResolvePages(int userId);

        /// <summary>
        /// 解析按钮编码列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<string>> ResolveButtons(int userId);
    }
}
