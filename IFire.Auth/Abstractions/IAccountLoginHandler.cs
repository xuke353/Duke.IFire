using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFire.Auth.Abstractions {
    public interface IAccountLoginHandler {
        /// <summary>
        /// 登录处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<LoginResultModel> Handle(UserNameLoginModel model);
    }
}
