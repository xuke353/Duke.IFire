using System;
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
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public string Encrypt(string username, string password) {
            return Md5Encrypt.Encrypt($"{username.ToLower()}_{password}");
        }

        public string Decrypt(string encryptPassword) {
            throw new NotSupportedException("MD5加密无法解密");
        }
    }
}
