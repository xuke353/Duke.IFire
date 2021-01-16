using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IFire.Framework.Providers;
using Microsoft.AspNetCore.Hosting;

namespace IFire.CodeGenerator.Model {

    public class FileConfigModel {
        public string ClassPrefix { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string CreateDate { get; set; }

        public string EntityName { get; set; }

        public string ServiceName { get; set; }

        public string ControllerName { get; set; }

        public string DtoGetInputName { get; set; }
        public string DtoGetOutputName { get; set; }
        public string DtoUpdateInputName { get; set; }

        public OutputConfigModel Output => new OutputConfigModel(ClassPrefix);
    }

    public class OutputConfigModel {
        public string EntityPath { get; private set; }
        public string ServicePath { get; private set; }
        public string DtoPath { get; private set; }
        public string ControllerPath { get; private set; }

        public OutputConfigModel(string classPrefix) {
            var basePath = IocProvider.Current.Resolve<IWebHostEnvironment>().ContentRootPath;
            ControllerPath = Path.Combine(basePath, "IFire.WebHost", "Controllers");
            ServicePath = Path.Combine(basePath, "AdmBoots.Application", $"{classPrefix}s");
            DtoPath = Path.Combine(ServicePath, "Dto");
            EntityPath = Path.Combine(basePath, "AdmBoots.Model", "Model");
        }
    }
}
