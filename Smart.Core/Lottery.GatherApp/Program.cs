using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smart.Core.Repository.SqlSugar;
using Smart.Core.Utils;
using SqlSugar;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lottery.GatherApp
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static ILoggerRepository repository { get; set; }
        /*
       时时彩：重庆时时彩， 新疆时时彩，天津时时彩  （台湾时时彩，澳门时时彩）
       11x5:   广东11x5,江西11x5,山东11x5,上海11x5    （台湾11x5，澳门11x5）
       kl8:    北京快乐8  （新韩国快乐8）                             （澳门快乐8，韩国快乐8）
       PK10：  北京PK10                                (澳门pk10,台湾Pk10)
       k3:江苏快三            （澳门快三，台湾快三）
       幸运农场 澳门幸运农场  台湾幸运农场  
       */
        public static async Task Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())   //指定配置文件所在的目录
            //.AddJsonFile("config.json", optional: true, reloadOnChange: true)
            //.Build();  //指定加载的配
            //var services = new ServiceCollection();

            ConfigFileHelper.Set("config.json");
            var connstr = ConfigFileHelper.Get("Lottery:Data:Database:Connection");
            var services = new ServiceCollection();
            //services.AddDataDapper(options =>
            //{
            //    options.ConnString = ConfigFileHelper.Get("Lottery:Data:Database:Connection");
            //    options.DbType = Smart.Data.Dapper.DBProvider.MySQL;
            //});
            ////log4net
            repository = LogManager.CreateRepository("LotteryRepository");
            ////指定配置文件
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            services.AddSqlSugarClient<DbFactory>((sp, op) =>
             {
                 op.ConnectionString = ConfigFileHelper.Get("Lottery:Data:Database:Connection");
                 op.DbType = DbType.MySql;
                 op.IsAutoCloseConnection = true;
                 op.InitKeyType = InitKeyType.Attribute;
                 op.IsShardSameThread = true;
             });
            services.AddServices();

          

            //services.AddCSRedis(options =>
            //{
            //    options.Add(new Smart.Core.NoSql.Redis.RedisConfig() { C_IP = "10.0.3.6", C_Post = 6379, C_Password = "redis123",C_Defaultdatabase=0 });
            //    options.Add(new Smart.Core.NoSql.Redis.RedisConfig() { C_IP = "10.0.3.6", C_Post = 6379, C_Password = "redis123", C_Defaultdatabase = 1 });
            //});
            services.AddConsoleLogger(options => { });
            services.AddSingleton<BalanceTasks>();
            var provider = services.BuildServiceProvider();
            var tasks = provider.GetRequiredService<BalanceTasks>();
            //Task t1 = new Task(() => tasks.AgentIP());
            //t1.Start();
            Task t2 = new Task(() => tasks.Run1122());
            t2.Start();
            Task t3 = new Task(() => tasks.XML());
            t3.Start();
            //Task t3 = new Task(() => tasks.RunCaikeBall());
            //t3.Start();
            Console.WriteLine("Done.");
            Console.ReadLine();

        }
    }
}
