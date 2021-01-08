using System.Collections.Generic;
using System.Threading.Tasks;
using IFire.Application.Auths.Web;

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
        Task<IList<AccountMenuItem>> ResolveMenus(int userId);
    }
}
