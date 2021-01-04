using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFire.Auth.Web {

    /// <summary>
    /// 通用权限(只要登录即可访问，不需要授权)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CommonAttribute : Attribute {
    }
}
