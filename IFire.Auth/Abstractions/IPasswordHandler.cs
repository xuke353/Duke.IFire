using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.Framework.Attributes;

namespace IFire.Auth.Abstractions {
    /// <summary>
    /// 密码处理器接口
    /// </summary>
    public interface IPasswordHandler {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string Encrypt(string userName, string password);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptPassword"></param>
        /// <returns></returns>
        string Decrypt(string encryptPassword);
    }
}
