using System;
using Microsoft.Extensions.Configuration;

namespace IFire.Data.EFCore {

    public class DbOptions {

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static SqlDialect Dialect { get; set; }

        /// <summary>
        /// 数据库版本
        /// </summary>
        public static string Version { get; set; }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public static bool InitData { get; set; }

        public static void InitConfiguration(IConfiguration configuration) {
            var dialect = configuration["DataBase:Dialect"];
            Version = configuration["DataBase:Version"];
            InitData = configuration["DataBase:InitData"] == "true";
            ConnectionString = configuration["DataBase:ConnectionString"];
            Dialect = (SqlDialect)Enum.Parse(typeof(SqlDialect), dialect, true);
        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum SqlDialect {

        /// <summary>
        /// SqlServer
        /// </summary>
        SqlServer,

        /// <summary>
        /// MySql
        /// </summary>
        MySql,

        /// <summary>
        /// SQLite
        /// </summary>
        SQLite,

        /// <summary>
        /// PostgreSQL
        /// </summary>
        PostgreSQL,

        /// <summary>
        /// Oracle
        /// </summary>
        Oracle
    }
}
