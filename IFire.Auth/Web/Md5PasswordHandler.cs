using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Framework.Attributes;
using IFire.Framework.Helpers;

namespace IFire.Auth.Web {
    /// <summary>
    /// 密码处理器
    /// </summary>
    [Singleton]
    public class Md5PasswordHandler : IPasswordHandler {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public string Encrypt(string userName, string password) {
            return Md5Encrypt.Encrypt($"{userName.ToLower()}_{password}");
        }

        public string Decrypt(string encryptPassword) {
            throw new NotSupportedException("MD5加密无法解密");
        }
    }
}
