using Lottery.GatherApp.Analysis;
using Lottery.GatherApp.Helper;
using Lottery.Services.Abstractions;
using Smart.Core.Logger;
using Smart.Core.NoSql.Redis;
using Smart.Core.Utils;
using System;
using System.Threading.Tasks;

namespace Lottery.GatherApp
{
    public class BalanceTasks
    {
        protected readonly IUsersService _userSvc;
        protected readonly ILogger _logger;
        protected readonly RedisManager _redisManager;
        protected readonly ISport_DataService _sport_DataService;
        protected readonly IXML_DataService _xml_DataService;

        public BalanceTasks(IUsersService usersSvc, ILogger logger,ISport_DataService sport_DataService , IXML_DataService xml_DataService,RedisManager redisManager)
        {
            this._userSvc = usersSvc;
            this._logger = logger;
            this._redisManager = redisManager;
            _sport_DataService = sport_DataService;
            _xml_DataService = xml_DataService;

        }

        public async Task CQSSC()
        {
            //this._redisManager.RedisDb(0).Set("ceshi","11111",100000);
            while (true)
            {
                try
                {
                    this._logger.Info("CQSSC开始采集。。。。");
                    //this._userSvc.TestMethod();
                    //Console.ForegroundColor = ConsoleColor.Blue;
                    //this._logger.Warn("CQSSC未采集到最新的开奖结果");
                    string type = ConfigFileHelper.Get("Analysis:cqssc_config:type");
                    var list = AnalysisManager.CQSSC(type);
                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            var arr = item.Split('|');
                            string lotteryNo = UseFullHelper.FormatIssuseNumber("CQSSC", arr[0]);
                            string lotteryData = arr[1];
                            this._logger.Info($"CQSSC重庆时时彩{lotteryNo}开奖完成{lotteryData}");
                            //var (code, msg) = await this.manager_task.LotteryKaijiang(1, lotteryNo, lotteryData);
                            //if (code == 0)
                            //{
                            //    Console.ForegroundColor = ConsoleColor.DarkCyan;
                            //    Console.WriteLine($"CQSSC重庆时时彩{lotteryNo}开奖完成{lotteryData}");
                            //}
                            //else
                            //{
                            //    Console.ForegroundColor = ConsoleColor.Yellow;
                            //    Console.WriteLine($"CQSSC{lotteryNo}开奖报错{msg}");
                            //    SSCAnalysisManager.DelUpdData($"CQSSC_{type}");
                            //}
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("CQSSC未采集到最新的开奖结果");
                    }
                }
                catch (Exception ex)
                {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    this._logger.Error($"{nameof(CQSSC)}: {ex.Message}",ex);
                }
                await Task.Delay(10000);
            }
        }


        public async Task HK6()
        {
            //this._redisManager.RedisDb(0).Set("ceshi","11111",100000);
            while (true)
            {
                try
                {
                    this._logger.Info("HK6开始采集。。。。");
                    //this._userSvc.TestMethod();
                    //Console.ForegroundColor = ConsoleColor.Blue;
                    //this._logger.Warn("CQSSC未采集到最新的开奖结果");
                    string type = ConfigFileHelper.Get("Analysis:HK6_config:type");
                    var list = AnalysisManager.HK6();
                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            var arr = item.Split('|');
                            string lotteryNo = UseFullHelper.FormatIssuseNumber("HK6", arr[0]);
                            string lotteryData = arr[1];
                            this._logger.Info($"HK6{lotteryNo}开奖完成{lotteryData}");
                            //var (code, msg) = await this.manager_task.LotteryKaijiang(1, lotteryNo, lotteryData);
                            //if (code == 0)
                            //{
                            //    Console.ForegroundColor = ConsoleColor.DarkCyan;
                            //    Console.WriteLine($"CQSSC重庆时时彩{lotteryNo}开奖完成{lotteryData}");
                            //}
                            //else
                            //{
                            //    Console.ForegroundColor = ConsoleColor.Yellow;
                            //    Console.WriteLine($"CQSSC{lotteryNo}开奖报错{msg}");
                            //    SSCAnalysisManager.DelUpdData($"CQSSC_{type}");
                            //}
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("HK6未采集到最新的开奖结果");
                    }
                }
                catch (Exception ex)
                {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    this._logger.Error($"{nameof(CQSSC)}: {ex.Message}", ex);
                }
                await Task.Delay(10000);
            }
        }

        public async Task HK6Issue()
        {
            //var list = AnalysisManager.HK6Issue();

            _redisManager.RedisDb(0).Publish("chan1", "123123123");
            _redisManager.RedisDb(0).Subscribe(("chan1", msg => Console.WriteLine(msg.Body)));
        }
        public async Task BJDC()
        {
            var manager = new SportData(_sport_DataService);
            manager.GetBjdc();
            manager.GetJCZQ();
        }

        public async Task XML()
        {
            var manager = new XML(_xml_DataService);
             manager.GdklsfXml();
        }
    }
}
