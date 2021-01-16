using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.Data.EFCore.Uow;
using IFire.Framework.Providers;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace IFire.Data.EFCore.Helpers {

    public class SqlHelper {
        public static IIFireUnitOfWork UnitOfWork => IocProvider.Current.Resolve<IIFireUnitOfWork>();

        public static int Execute(string sql, params object[] parameters) {
            return UnitOfWork.CurrentDbContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        public static async Task<int> ExecuteAsync(string sql, params object[] parameters) {
            return await UnitOfWork.CurrentDbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public static IQueryable<T> Query<T>(string sql, params object[] parameters) where T : class {
            return UnitOfWork.CurrentDbContext.Set<T>().FromSqlRaw(sql, parameters);
        }
    }
}
