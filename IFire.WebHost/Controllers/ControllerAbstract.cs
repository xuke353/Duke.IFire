using IFire.Auth.Web;
using Microsoft.AspNetCore.Mvc;

namespace IFire.WebHost.Controllers {

    /// <summary>
    /// 控制器抽象
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [PermissionValidate]
    public abstract class ControllerAbstract : ControllerBase {
    }
}
