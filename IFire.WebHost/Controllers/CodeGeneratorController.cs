using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.CodeGenerator.Model;
using IFire.CodeGenerator.Template;
using IFire.Data.EFCore;
using Microsoft.AspNetCore.Mvc;

namespace IFire.WebHost.Controllers {

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Description("代码生成器")]
    public class CodeGeneratorController : ControllerBase {

        public CodeGeneratorController() {
        }

        [HttpPost]
        public async Task<IActionResult> CodeGenerate([FromQuery] InputModel input) {
            var template = new SingleTableTemplate();
            var result = await template.CreateCode(input);
            return Ok(result);
        }
    }
}
