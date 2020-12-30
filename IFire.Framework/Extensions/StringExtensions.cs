using System;
using System.Linq;
using System.Text;

namespace IFire.Framework.Extensions {

    public static class StringExtensions {

        /// <summary>
        /// 判断字符串是否为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNull(this string s) {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 判断字符串是否不为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool NotNull(this string s) {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 与字符串进行比较，忽略大小写
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string s, string value) {
            return s.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string s) {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToLower() + s.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string s) {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToUpper() + s.Substring(1);
            return str;
        }

        /// <summary>
        /// 转为Base64，UTF-8格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBase64(this string s) {
            return s.ToBase64(Encoding.UTF8);
        }

        /// <summary>
        /// 转为Base64
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ToBase64(this string s, Encoding encoding) {
            if (s.IsNull())
                return string.Empty;

            var bytes = encoding.GetBytes(s);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string TruncateWithPostfix(this string str, int maxLength) {
            return TruncateWithPostfix(str, maxLength, "...");
        }

        public static string TruncateWithPostfix(this string str, int maxLength, string postfix) {
            if (str == null) {
                return null;
            }

            if (str == string.Empty || maxLength == 0) {
                return string.Empty;
            }

            if (str.Length <= maxLength) {
                return str;
            }

            if (maxLength <= postfix.Length) {
                return postfix.Substring(0, maxLength);
            }

            return str.Substring(0, maxLength - postfix.Length) + postfix;
        }
    }
}
