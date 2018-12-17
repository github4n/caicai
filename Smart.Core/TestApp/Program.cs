using System;
using Lottery.Modes.Entity;
using SkyRulerBusiness.Infrastructure;
using SqlSugar;
using StackExchange.Redis;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var option = new ConfigurationOptions();
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379,password=redis123");

            //IDatabase dbr = redis.GetDatabase(0);
            //var batch = dbr.CreateBatch();
            //sub.Publish("test1", "2222");

            ISubscriber sub = redis.GetSubscriber();
            sub.Publish("95", "JCZQ1211101950191836176274");
            string vs = string.Empty;
            string ss = string.Empty;
            sub.Subscribe("messages", (channel, message) => {
                Console.WriteLine((string)message);
            });
            Console.ReadKey();
            return;
            //var db = getdb();
            //var list = db.Queryable<blast_count>().ToList();
            ////var model = list.Find(a => a.id == 1038);
            ////var model1 = list.Find(a => a.id == 1039);
            //blast_count model = new blast_count();
            //blast_count model1 = new blast_count();
            //model.typeid = 1000;
            //model.playedId = 10000;
            //model.createdate = DateTime.Now;
            //model.betCount = 1;
            //model.betAmount = 100;
            //model.zjAmount = 100;
            //try
            //{
            //    db.Ado.BeginTran();
            //    model1.id = 1000000;
            //    db.Insertable<blast_count>(model).ExecuteCommand();
            //    //db.Insertable<blast_count>(model1).ExecuteCommand();
            //    //throw new Exception("3333");
            //    db.Ado.CommitTran();
            //}
            //catch (Exception exp)
            //{

            //    db.Ado.RollbackTran();
            //}
            SqlSugarClient db = SqlSugarHelper.GetInstance();
            var list = db.Queryable<blast_count>().ToList();
            var model = list.Find(a => a.id == 225);
            //var result = db.Ado.UseTran(() =>
            //{
            //    model.betCount = 2;
            //    db.Updateable<blast_count>(model).ExecuteCommand();
            //    db.Ado.ExecuteCommand("delete student");
            //    throw new Exception("error haha");
            //});



            //SqlSugarClient db1 = SqlSugarHelper.GetInstance();
            //blast_admin_log model1 = new blast_admin_log();
            //SqlSugarHelper.TranInvok(() =>
            //{
            //    model.betCount = 2;
            //    db.Updateable<blast_count>(model).ExecuteCommand();
            //    db.Updateable<blast_admin_log>(model1).ExecuteCommand();
            //    return true;
            //}, db, db1);

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }


        private static SqlSugarClient getdb()
        {
            SqlSugarClient db = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = "Server=10.0.3.6;Port=3306;Database=ecp_newcai;Username=root;Password=!@#123456abc;",
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,
                    IsShardSameThread = true, //设为true相同线程是同一个SqlConnection
                    InitKeyType= InitKeyType.Attribute
                });
            return db;
        }
    }
}
