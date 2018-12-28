using Lottery.Services;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using Smart.Core.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lottery.API.Tasks
{
    /// <summary>
    /// 计时任务
    /// </summary>
    public class AutoTask
    {
        public IApi_DataService IApi_Service = null;
        public AutoTask()
        {
            ConnectionConfig conn = new ConnectionConfig()
            {
                ConnectionString = ConfigFileHelper.Get("Lottery:Data:Database:Connection"),
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                IsShardSameThread = false
            };
            var fa = new DbFactory(conn);
            IApi_Service = new Api_DataService(fa);
        }
        public void AutoAddToRedis_LotteryList()
        {
            while (true)
            {
                try
                {
                    IApi_Service.AddRedisLocalLottery();
                }
                catch (Exception)
                {

                }
                try
                {
                    IApi_Service.AddRedisHighLottery();
                }
                catch (Exception)
                {

                }
                try
                {
                    IApi_Service.AddRedisCountryLottery();
                }
                catch (Exception)
                {

                }
                Thread.Sleep(3 * 1000 * 60);
            }
        }
    }
}
