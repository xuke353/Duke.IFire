using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IFire.Auth.Jwt;

namespace IFire.Auth.Abstractions {

    /// <summary>
    /// 登录处理
    /// </summary>
    public interface ILoginHandler {

        /// <summary>
        /// 登录处理
        /// </summary>
        /// <param name="claims">信息</param>
        /// <param name="extendData">扩展数据</param>
        /// <returns></returns>
        JwtTokenModel Hand(List<Claim> claims, string extendData);
    }
}
