using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IFire.Framework.Extensions;
using IFire.Framework.Providers;
using Microsoft.AspNetCore.Hosting;

namespace IFire.CodeGenerator.Model {

    public class FileConfigModel {
        public string TableName { get; set; }
        public string ClassPrefix { get; set; }
        public string Description { get; set; }
        public string CreatorName { get; set; }
        public string CreateDate { get; set; }

        public string LowerClassPrefix => ClassPrefix.FirstCharToLower();
        public string EntityName => ClassPrefix;

        public string ServiceName => $"{ClassPrefix}Service";

        public string ControllerName => $"{ClassPrefix}Controller";

        public string DtoGetInputName => $"Get{ClassPrefix}Input";
        public string DtoGetOutputName => $"Get{ClassPrefix}Output";
        public string DtoUpdateInputName => $"Update{ClassPrefix}Input";

        public OutputConfigModel Output => new OutputConfigModel(ClassPrefix);
    }

    public class OutputConfigModel {
        public string EntityPath { get; private set; }
        public string ServicePath { get; private set; }
        public string DtoPath { get; private set; }
        public string ControllerPath { get; private set; }

        public OutputConfigModel(string classPrefix) {
            var basePath = IocProvider.Current.Resolve<IWebHostEnvironment>().ContentRootPath.Trim('\\');
            basePath = Directory.GetParent(basePath).FullName;
            ControllerPath = Path.Combine(basePath, "IFire.WebHost", "Controllers");
            ServicePath = Path.Combine(basePath, "IFire.Application", $"{classPrefix}s");
            DtoPath = Path.Combine(ServicePath, "Dto");
            EntityPath = Path.Combine(basePath, "IFire.Model");
        }
    }
}
