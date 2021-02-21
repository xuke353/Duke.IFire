﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.CodeGenerator.Model;
using IFire.Framework.Extensions;
using IFire.Framework.Helpers;

namespace IFire.CodeGenerator.Template {

    public class SingleTableTemplate {

        public FileConfigModel MapToFileConfig(InputModel inputModel) {
            var config = new FileConfigModel() {
                TableName = inputModel.TableName,
                ClassPrefix = TableMappingHelper.GetClassNamePrefix(inputModel.ClassPrefix.IsNull() ? inputModel.ClassPrefix : inputModel.TableName)
                                                .FirstCharToUpper(),
                Description = inputModel.Description,
                CreatorName = inputModel.CreatorName,
                CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            };
            return config;
        }

        #region BuildEntity

        public string BuildEntity(FileConfigModel fileConfigModel, DataTable dt) {
            var sb = new StringBuilder();
            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine("using IFire.Infrastructure.Framework.Abstractions;");
            sb.AppendLine();

            sb.AppendLine("namespace IFire.Model");
            sb.AppendLine("{");

            SetClassDescription("实体类", fileConfigModel, sb);

            sb.AppendLine("    [Table(\"" + fileConfigModel.TableName + "\")]");
            sb.AppendLine("    public class " + fileConfigModel.EntityName + " : " + GetBaseEntity(dt));
            sb.AppendLine("    {");

            string column = string.Empty;
            string remark = string.Empty;
            string datatype = string.Empty;
            string isNullable = string.Empty;
            foreach (DataRow dr in dt.Rows) {
                column = dr["TableColumn"].ToString();
                if (BaseField.BaseFieldList.Where(p => p == column).Any()) {
                    // 基础字段不需要生成，继承合适的BaseEntity即可。
                    continue;
                }

                remark = dr["Remark"].ToString();
                datatype = dr["Datatype"].ToString();
                isNullable = dr["IsNullable"].ToString();
                datatype = TableMappingHelper.GetPropertyDatatype(datatype, isNullable);

                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// " + remark);
                sb.AppendLine("        /// </summary>");
                sb.AppendLine("        /// <returns></returns>");

                switch (datatype) {
                    case "long?":
                        sb.AppendLine("        [JsonConverter(typeof(StringJsonConverter))]");
                        break;

                    case "long":
                        sb.AppendLine("        [JsonConverter(typeof(StringJsonConverter))]");
                        break;

                    case "DateTime?":
                        sb.AppendLine("        [JsonConverter(typeof(DateTimeJsonConverter))]");
                        break;

                    case "DateTime":
                        sb.AppendLine("        [JsonConverter(typeof(DateTimeJsonConverter))]");
                        break;
                }
                sb.AppendLine("        public " + datatype + " " + column + " { get; set; }");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        #endregion BuildEntity

        #region BuildService

        public string BuildService(FileConfigModel baseConfigModel) {
            StringBuilder sb = new StringBuilder();
            string method = string.Empty;
            sb.AppendLine("using AdmBoots.Domain.Models;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine($"using IFire.Application.{baseConfigModel.ClassPrefix}s.Dto;");
            sb.AppendLine("using IFire.Domain.RepositoryIntefaces;");
            sb.AppendLine("using AutoMapper;");
            sb.AppendLine("using IFire.Infrastructure.Framework.Abstractions;");

            sb.AppendLine();

            sb.AppendLine($"namespace IFire.Application.{baseConfigModel.ClassPrefix}s");
            sb.AppendLine("{");

            SetClassDescription("服务类", baseConfigModel, sb);

            sb.AppendLine($"    public class {baseConfigModel.ServiceName} : IFireAppServiceBase,I{baseConfigModel.ServiceName}");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly IRepository<{baseConfigModel.EntityName}, int> _{baseConfigModel.LowerClassPrefix}Repository;");
            sb.AppendLine($"        public {baseConfigModel.ServiceName}(IRepository<{baseConfigModel.EntityName}, int> {baseConfigModel.LowerClassPrefix}Repository)");
            sb.AppendLine("        {");
            sb.AppendLine($"           _{baseConfigModel.LowerClassPrefix}Repository = {baseConfigModel.LowerClassPrefix}Repository;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        public Task AddOrUpdate{baseConfigModel.ClassPrefix}(int? id, {baseConfigModel.DtoUpdateInputName} input)");
            sb.AppendLine("        {");
            sb.AppendLine("            throw new System.NotImplementedException();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        public Task Delete{baseConfigModel.ClassPrefix}(int[] ids)");
            sb.AppendLine("        {");
            sb.AppendLine("            throw new System.NotImplementedException();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        public  Task<Page<{baseConfigModel.DtoGetOutputName}>> Get{baseConfigModel.ClassPrefix}List({baseConfigModel.DtoGetInputName} input)");
            sb.AppendLine("        {");
            sb.AppendLine("            throw new System.NotImplementedException();");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion BuildService

        #region BuildIService

        public string BuildIService(FileConfigModel baseConfigModel) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine($"using IFire.Application.{baseConfigModel.ClassPrefix}s.Dto;");
            sb.AppendLine("using IFire.Domain.RepositoryIntefaces;");
            sb.AppendLine("using IFire.Infrastructure.Framework.Abstractions;");

            sb.AppendLine();

            sb.AppendLine($"namespace IFire.Application.{baseConfigModel.ClassPrefix}s");
            sb.AppendLine("{");

            SetClassDescription("服务接口", baseConfigModel, sb);

            sb.AppendLine($"    public interface " + $"I{baseConfigModel.ServiceName} :ITransientDependency");
            sb.AppendLine("    {");
            sb.AppendLine($"        Task<Page<{baseConfigModel.DtoGetOutputName}>> Get{baseConfigModel.ClassPrefix}List({baseConfigModel.DtoGetInputName} input);");
            sb.AppendLine();
            sb.AppendLine($"        Task AddOrUpdate{baseConfigModel.ClassPrefix}(int? id, {baseConfigModel.DtoUpdateInputName} input);");
            sb.AppendLine();
            sb.AppendLine($"        Task Delete{baseConfigModel.ClassPrefix}(int[] ids);");
            sb.AppendLine();

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion BuildIService

        #region BuildController

        public string BuildController(FileConfigModel baseConfigModel) {
            string classPrefix = baseConfigModel.ClassPrefix;
            //小写类名
            var lowerClassPrefix = classPrefix.Substring(0, 1).ToLower() + classPrefix.Substring(1);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using IFire.Application.{classPrefix}s;");
            sb.AppendLine($"using IFire.Application.{classPrefix}s.Dto;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using System.Web;");
            sb.AppendLine("using Microsoft.AspNetCore.Authorization;");
            sb.AppendLine("using IFire.Domain;");
            sb.AppendLine("using IFire.Infrastructure.Framework.Abstractions;");
            sb.AppendLine();

            sb.AppendLine("namespace IFire.WebHost.Controllers");
            sb.AppendLine("{");

            SetClassDescription("控制器类", baseConfigModel, sb);

            sb.AppendLine("    [ApiController]");
            sb.AppendLine("    [ApiVersion(\"1.0\")]");
            sb.AppendLine("    [Route(\"api/v{version:apiVersion}/" + lowerClassPrefix + "s\")]");
            sb.AppendLine("    //[Authorize(AdmConsts.POLICY)]");
            sb.AppendLine("    public class " + baseConfigModel.ControllerName + " : ControllerBase");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly I{baseConfigModel.ServiceName} _{lowerClassPrefix}Service;");
            sb.AppendLine();
            sb.AppendLine($"        public {baseConfigModel.ControllerName} (I{baseConfigModel.ServiceName} {lowerClassPrefix}Service)");
            sb.AppendLine("        {");
            sb.AppendLine($"            _{lowerClassPrefix}Service = {lowerClassPrefix}Service;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        public ActionResult Get" + classPrefix + "List([FromQuery]" + baseConfigModel.DtoGetInputName + " input)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var result = _{lowerClassPrefix}Service.Get{classPrefix}List(input);");
            sb.AppendLine($"            return Ok(ResponseBody.From(result));");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpPost(\"{id}\")]");
            sb.AppendLine($"        public async Task<IActionResult> AddOrUpdate{classPrefix}(int? id, [FromBody]{baseConfigModel.DtoUpdateInputName} input)");
            sb.AppendLine("        {");
            sb.AppendLine($"            await _{lowerClassPrefix}Service.AddOrUpdate{classPrefix}(id, input);");
            sb.AppendLine("            return Ok(ResponseBody.From(\"操作成功\"));");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpDelete]");
            sb.AppendLine($"        public async Task<IActionResult> Delete{classPrefix}(int[] ids) ");
            sb.AppendLine("        {");
            sb.AppendLine($"            await _{lowerClassPrefix}Service.Delete{classPrefix}(ids);");
            sb.AppendLine("            return Ok(ResponseBody.From(\"操作成功\"));");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion BuildController

        #region BuildDto

        public string BuildDto(FileConfigModel baseConfigModel, string dtoName) {
            StringBuilder sb = new StringBuilder();
            if (dtoName.Contains(":"))
                sb.AppendLine("using IFire.Infrastructure.Framework.Abstractions;");

            sb.AppendLine();

            sb.AppendLine($"namespace IFire.Application.{baseConfigModel.ClassPrefix}s.Dto");
            sb.AppendLine("{");

            SetClassDescription("Dto", baseConfigModel, sb);

            sb.AppendLine($"    public class " + $"{dtoName}");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion BuildDto

        #region SetClassDescription

        private void SetClassDescription(string type, FileConfigModel baseConfigModel, StringBuilder sb) {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 创 建：" + baseConfigModel.CreatorName);
            sb.AppendLine("    /// 日 期：" + baseConfigModel.CreateDate);
            sb.AppendLine("    /// 描 述：" + baseConfigModel.Description + type);
            sb.AppendLine("    /// </summary>");
        }

        #endregion SetClassDescription

        private string GetBaseEntity(DataTable dt) {
            var entity = string.Empty;
            var columnList = dt.AsEnumerable().Select(p => p["TableColumn"].ToString()).ToList();

            var id = columnList.Where(p => p == "Id" || p == "id").Any();
            var baseIsDelete = columnList.Where(p => p == "ISoftDelete").Any();
            var baseVersion = columnList.Where(p => p == "RowVersion").Any();
            var baseModifyTime = columnList.Where(p => p == "ModifyTime").Any();
            var baseModifierId = columnList.Where(p => p == "ModifierId").Any();
            var baseModifierName = columnList.Where(p => p == "Modifier").Any();
            var baseCreateTime = columnList.Where(p => p == "CreateTime").Any();
            var baseCreatorId = columnList.Where(p => p == "CreatorId").Any();
            var baseCreatorName = columnList.Where(p => p == "CreatorName").Any();
            var baseDeleterId = columnList.Where(p => p == "DeleterId").Any();
            var baseDeleterName = columnList.Where(p => p == "DeleterName").Any();
            var baseDeletionTime = columnList.Where(p => p == "DeletionTime").Any();

            if (!id) {
                throw new Exception("数据库表必须有主键Id字段");
            }
            if (baseModifyTime && baseModifierId && baseModifierName && baseCreateTime && baseCreatorId && baseCreatorName) {
                entity = "AuditEntity";
            } else if (baseCreateTime && baseCreatorId && baseCreatorName) {
                entity = "CreationEntity";
            } else {
                entity = "Entity";
            }

            if (baseDeleterId && baseDeleterName && baseDeletionTime) {
                entity = entity.IsNull() ? "deleteInterface" : $"{entity},deleteInterface";
            }
            return entity;
        }

        #region CreateCode

        public List<KeyValue> CreateCode(InputModel inputModel, DataTable dt) {
            var result = new List<KeyValue>();
            var baseConfigModel = MapToFileConfig(inputModel);

            #region 实体类

            if (inputModel.CreateEntity) {
                var codePath = Path.Combine(baseConfigModel.Output.EntityPath, baseConfigModel.EntityName + ".cs");
                var codeEntity = BuildEntity(baseConfigModel, dt);
                if (!File.Exists(codePath)) {
                    FileHelper.CreateFile(codePath, codeEntity);
                    result.Add(new KeyValue { Key = "实体类", Value = codePath });
                }
            }

            #endregion 实体类

            #region 服务类

            if (inputModel.CreateService) {
                var codeIService = BuildIService(baseConfigModel);
                var codeService = BuildService(baseConfigModel);
                var codeServicePath = Path.Combine(baseConfigModel.Output.ServicePath, baseConfigModel.ServiceName + ".cs");
                var codeIServicePath = Path.Combine(baseConfigModel.Output.ServicePath, "I" + baseConfigModel.ServiceName + ".cs");
                if (!File.Exists(codeServicePath)) {
                    FileHelper.CreateFile(codeServicePath, codeService);
                    result.Add(new KeyValue { Key = "服务类", Value = codeServicePath });
                }
                if (!File.Exists(codeIServicePath)) {
                    FileHelper.CreateFile(codeIServicePath, codeService);
                    result.Add(new KeyValue { Key = "服务接口", Value = codeIServicePath });
                }
            }

            #endregion 服务类

            #region 控制器

            if (inputModel.CreateCotroller) {
                var codeController = BuildController(baseConfigModel);
                var codePath = Path.Combine(baseConfigModel.Output.ControllerPath, baseConfigModel.ControllerName + ".cs");
                if (!File.Exists(codePath)) {
                    FileHelper.CreateFile(codePath, codeController);
                    result.Add(new KeyValue { Key = "控制器", Value = codePath });
                }
            }

            #endregion 控制器

            return result;
        }

        #endregion CreateCode
    }

    public class BaseField {

        public static string[] BaseFieldList = new string[]
        {
            "Id",
            "ModifierId",
            "ModifierName",
            "ModifyTime",
            "CreatorId",
            "CreatorName",
            "CreateTime",
            "RowVersion",
            "DeleterId",
            "DeleterName",
            "DeletionTime"
        };
    }
}
