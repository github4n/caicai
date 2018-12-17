using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using SqlSugar;

namespace SkyRulerBusiness.Infrastructure
{
    public class SqlSugarHelper
    {
        public static DbResult<T> TranInvok<T>(Func<T> func,params SqlSugarClient[] clients)
        {
            DbResult<T> dbResult = new DbResult<T>();
            try
            {
                clients.BeginTran();
                dbResult.Data = func();
                dbResult.IsSuccess = true;
                clients.CommitTran();
            }
            catch (Exception ex)
            {
                clients.RollbackTran();
                dbResult.IsSuccess = false;
                dbResult.Data = default(T);
                dbResult.ErrorMessage = ex.Message;
                dbResult.ErrorException = ex;
            }
            return dbResult;
        }


        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(
               new ConnectionConfig()
               {
                   ConnectionString = "Server=10.0.3.6;Port=3306;Database=ecp_newcai;Username=root;Password=!@#123456abc;",
                   DbType = DbType.MySql,
                   IsAutoCloseConnection = true,
                   IsShardSameThread = true, //设为true相同线程是同一个SqlConnection
                    InitKeyType = InitKeyType.Attribute
               });
            return db;
        }
    }
}
