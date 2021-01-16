using System.Threading.Tasks;
using IFire.Application.Auths.Dto;
using IFire.Model;

namespace IFire.Application.Auths {

    public interface IAuthService {

        /// <summary>
        /// 用户名登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<LoginResult> Login(LoginInput input);

        /// <summary>
        /// 刷新令牌(只针对JWT认证方式)
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<LoginResult> RefreshToken(string refreshToken);

        /// <summary>
        /// 获取认证信息
        /// </summary>
        /// <returns></returns>
        Task<AuthInfoOutput> GetAuthInfo();

        /// <summary>
        /// 查询指定账户的认证信息(缓存优先)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AccountAuthInfo> GetAuthInfo(int userId);
    }
}