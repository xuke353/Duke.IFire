using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFire.Auth.Abstractions {

    /// <summary>
    /// 单账户登录处理器
    /// </summary>
    public interface ISingleAccountLoginHandler {

        /// <summary>
        /// 验证账户是否已在别处登录
        /// </summary>
        /// <returns></returns>
        Task<bool> Validate();
    }
}
