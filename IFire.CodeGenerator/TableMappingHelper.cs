using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IFire.CodeGenerator {

    public class TableMappingHelper {

        /// <summary>
        /// UserService转成userService
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstLetterLowercase(string instanceName) {
            instanceName = instanceName.ToString();
            if (!string.IsNullOrEmpty(instanceName)) {
                StringBuilder sb = new StringBuilder();
                sb.Append(instanceName[0].ToString().ToLower() + instanceName.Substring(1));
                return sb.ToString();
            } else {
                return instanceName;
            }
        }

        /// <summary>
        /// sys_menu_authorize变成MenuAuthorize
        /// </summary>
        public static string GetClassNamePrefix(string tableName) {
            string[] arr = tableName.Split('_');
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < arr.Length; i++) {
                sb.Append(arr[i][0].ToString().ToUpper() + arr[i].Substring(1));
            }
            return sb.ToString();
        }

        public static string GetPropertyDatatype(string sDatatype, string sIsNullable) {
            sDatatype = sDatatype.ToLower();
            sIsNullable = sIsNullable.ToUpper();
            string sTempDatatype;
            switch (sDatatype) {
                case "int":
                case "number":
                case "integer":
                case "smallint":
                    sTempDatatype = sIsNullable == "N" ? "int" : "int?";
                    break;

                case "bigint":
                    sTempDatatype = sIsNullable == "N" ? "long" : "long?";
                    break;

                case "tinyint":
                    sTempDatatype = "byte?";
                    break;

                case "numeric":
                case "real":
                    sTempDatatype = sIsNullable == "N" ? "Single" : "Single?";
                    break;

                case "float":
                    sTempDatatype = sIsNullable == "N" ? "float" : "float?";
                    break;

                case "decimal":
                case "numer(8,2)":
                    sTempDatatype = sIsNullable == "N" ? "decimal" : "decimal?";
                    break;

                case "bit":
                    sTempDatatype = sIsNullable == "N" ? "bool" : "bool?";
                    break;

                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    sTempDatatype = sIsNullable == "N" ? "DateTime" : "DateTime?";
                    break;

                case "money":
                case "smallmoney":
                    sTempDatatype = sIsNullable == "N" ? "double" : "double?";
                    break;

                case "char":
                case "varchar":
                case "nvarchar2":
                case "text":
                case "nchar":
                case "nvarchar":
                case "ntext":
                default:
                    sTempDatatype = "string";
                    break;
            }
            return sTempDatatype;
        }

        public static DataTable ListToDataTable<T>(List<T> entitys) {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1) {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++) {
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys) {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType) {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++) {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}
