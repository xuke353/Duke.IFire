﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IFire.Framework.Helpers {
    public static class DataTableHelper {
        public static DataTable ListToDataTable<T>(List<T> entitys) {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1) {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            var entityType = entitys[0].GetType();
            var entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            var dt = new DataTable();
            for (var i = 0; i < entityProperties.Length; i++) {
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys) {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType) {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                var entityValues = new object[entityProperties.Length];
                for (var i = 0; i < entityProperties.Length; i++) {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}
