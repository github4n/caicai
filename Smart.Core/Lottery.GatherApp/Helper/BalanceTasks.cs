﻿using log4net;
using Lottery.GatherApp.Analysis;
using Lottery.GatherApp.Analysis.Caike;
using Lottery.GatherApp.Analysis.LotteryDetail;
using Lottery.GatherApp.Analysis.UC;
using Lottery.GatherApp.Helper;
using Lottery.Services;
using Lottery.Services.Abstractions;
using Smart.Core.Logger;
using Smart.Core.NoSql.Redis;
using Smart.Core.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Lottery.GatherApp
{
    public class BalanceTasks
    {
        protected readonly IUsersService _userSvc;
        //log4Net
        private ILog log;
        protected readonly ILogger _logger;
        protected readonly RedisManager _redisManager;
        protected readonly ISport_DataService _sport_DataService;
        protected readonly IDigitalLotteryService _digitalLotteryService;
        protected readonly IXML_DataService _xml_DataService;
        protected readonly ILotteryDetailService _ILotteryDetailService;
        protected readonly IKaiJiangWangService _kaiJiangWangService;
        protected readonly IJddDataService _IJddDataService;
        protected readonly IAgentIPService _agentIPService;
        public BalanceTasks(IUsersService usersSvc, ILogger logger, ISport_DataService sport_DataService, IXML_DataService xml_DataService, IDigitalLotteryService digitalLotteryService, ILotteryDetailService lotteryDetailService,IKaiJiangWangService kaiJiangWangService,IJddDataService jddDataService,IAgentIPService agentIPService)
        {
            this._userSvc = usersSvc;
            this.log = LogManager.GetLogger(Program.repository.Name, typeof(BalanceTasks));
            this._logger = logger;
            _sport_DataService = sport_DataService;
            _xml_DataService = xml_DataService;
            _digitalLotteryService = digitalLotteryService;
            _ILotteryDetailService = lotteryDetailService;
            _IJddDataService = jddDataService;
            _kaiJiangWangService = kaiJiangWangService;
            _agentIPService = agentIPService;
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
                    this._logger.Error($"{nameof(CQSSC)}: {ex.Message}", ex);
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

        public void SportData()
        {
            var manager = new SportData(_sport_DataService);
            manager.Start();
        }

        public void LotteryData()
        {
            var manager = new DigitalLottery(_digitalLotteryService);
            manager.Start();
        }
        public void KaiJiangWang()
        {
            var manager = new KaiJiangWangRequest(_kaiJiangWangService);
            manager.Start();
        }
        public void AgentIP()
        {
            var manager = new AgentIPControl(_agentIPService);
            manager.StartLoadAgentIP();
        }
        private DateTime old_Time { get; set; }
        //辽宁快乐12  广东快乐十分  广西快乐10分 重庆时时彩 是网页版
        //gdklsf(广东快乐十分)  bjsyxw(北京11选5)  kl8(北京快乐8)   bjkzhc(北京快中彩)  bjpkshi(北京PK拾) bjk3(北京快3)  tjsyxw(天津11选5)
        //klsf(天津快乐十分)  tjssc(天津时时彩)  hebsyxw(河北11选5)  hebk3(河北快3)   nmgsyxw(内蒙古11选5)  nmgk3(内蒙古快3)  lnsyxw(辽宁11选5)
        //jlsyxw(吉林11选5)  jlk3(吉林快3)  hljsyxw(黑龙江11选5)  hljklsf(黑龙江快乐十分)   shhsyxw(上海11选5)  shhk3(上海快3)  ssl(上海时时乐) 
        //jssyxw(江苏11选5)  jsk3(江苏快3)  zjsyxw(浙江11选5)  zjkl12(浙江快乐12)  ahsyxw(安微11选5) ahk3(安微快三)  fjsyxw(福建11选5)
        //fjk3(福建快3)   dlc(江西11选5)  jxssc(江西时时彩) jxk3(江西快3)  qyh(山东群英会) shdsyxw(山东十一运夺冠)  shdklpk3(山东快乐扑克3)
        //hensyxw(河南11选5)  henk3(河南快3)  henky481(河南快赢481)  hbsyxw(湖北11选5)  hbk3(湖北快3)  hnklsf(湖南快乐十分) xysc(湖南快乐赛车)
        //gdsyxw(广东11选5)   gxsyxw(广西11选5)  gxk3(广西快3)  chqklsf(重庆快乐十分)  sckl12(四川快乐12)  gzsyxw(贵州11选5)  
        //gzk3(贵州快3)  sxsyxw(陕西11选5)  sxklsf(陕西快乐十分)  gssyxw(甘肃11选5)  gsk3(甘肃快3)  qhk3(青海快3)  xjsyxw(新疆11选5)
        //xjssc(新疆时时彩)   xjxlc(新疆喜乐彩)  shxsyxw(山西11选5)  ytdj(山西泳坛夺金) shxklsf(山西快乐十分)  ynsyxw(云南11选5)
        //ynklsf(云南快乐10分)  ynssc(云南时时彩)
        String[] strNumber = { "1", "3", "4" };
        public async Task XML()
        {
            int count = 0;
            var manager = new XML(_xml_DataService);
            var LotteryDetal = new NormalLotteryDetail(_ILotteryDetailService);
            string info = "";
           

            while (true)
            {
                try
                {


                    var now = DateTime.Now;
                    if (old_Time == null || (now - old_Time).TotalHours > 1.5)
                    {
                        old_Time = now;
                        info = "北京单场期号开始采集";
                        log.Info("url_500zx网" + info);
                        try
                        {
                            count = await manager.GetBjdcAsync();
                        }
                        catch (Exception ex)
                        {
                            log.Error("url_500zx网" + info + ex.Message);
                        }
                        info = "北京单场期号采集完毕.新采集了" + count + "条";
                        log.Info("url_500zx网" + info);
                        info = "北京单场——胜负过关期号开始采集";
                        log.Info("url_500zx网" + info);
                        try
                        {
                            count = await manager.GetSfggAsync();
                        }
                        catch (Exception ex)
                        {

                            log.Error("url_500zx网" + info + ex.Message);
                        }
                        info = "北京单场——胜负过关期号采集完毕.新采集了" + count + "条";

                        log.Info("url_500zx网" + info);

                        info = "福彩3D期号开始采集";
                        log.Info("url_500kaijiang网" + info);
                        try
                        {
                            count = await manager.LoadSDhtml("sd");
                        }
                        catch (Exception ex)
                        {

                            log.Error("url_500kaijiang网" + info + ex.Message);
                        }
                        info = "福彩3D期号采集完毕.新采集了" + count + "条";

                        log.Info("url_500kaijiang网" + info);

                        info = "排列3期号开始采集";
                        log.Info("url_500kaijiang网" + info);
                        try
                        {
                            count = await manager.LoadPlsHtml("pls");
                        }
                        catch (Exception ex)
                        {

                            log.Error("url_500kaijiang网" + info + ex.Message);
                        }
                        info = "排列3期号采集完毕.新采集了" + count + "条";

                        log.Info("url_500kaijiang网" + info);
                        SportData();
                        LotteryData();
                        foreach (var item in _xml_DataService.GetHighFrequency())
                        {
                            try
                            {
                                if (item.HighFrequency == 1)
                                {
                                    info = item.LotteryName + "期号开始采集";
                                    log.Info("url_500kaijiang网" + info);
                                    count = await manager.LoadXml(item.LotteryCode);
                                    info = item.LotteryName + "期号采集完毕.新采集了" + count + "条";

                                    log.Info("url_500kaijiang网" + info);
                                    //Thread.Sleep(new Random().Next(1000, 5000));
                                }
                                if (item.HighFrequency != 1 && item.LotteryCode != "zqdc" && item.LotteryCode != "sd" && item.LotteryCode != "pls" && item.LotteryCode != "jczq" && item.LotteryCode != "jclq" && item.LotteryCode != "zqdcsfgg")
                                {
                                    info = item.LotteryName + "期号开始采集";
                                    log.Info("url_500kaijiang网" + info);
                                    count = await manager.LoadQGDFCXml(item.LotteryCode);
                                    info = item.LotteryName + "期号采集完毕.新采集了" + count + "条";

                                    log.Info("url_500kaijiang网" + info);
                                    //Thread.Sleep(new Random().Next(1000, 5000));
                                }
                                if (item.HighFrequency != 1 && item.LotteryCode != "zqdc" && item.LotteryCode != "jczq" && item.LotteryCode != "jclq" && item.LotteryCode != "sd" && item.LotteryCode != "zqdcsfgg")
                                {
                                    info = item.LotteryName + "详情开始采集";
                                    log.Info("url_500kaijiang网" + info);
                                    count = await LotteryDetal.LoadLotteryDetal(item.LotteryCode);
                                    info = item.LotteryName + "详情采集完毕.新采集了" + count + "条";

                                    log.Info("url_500kaijiang网" + info);
                                    //Thread.Sleep(new Random().Next(1000, 5000));
                                }

                                foreach (var number in strNumber)
                                {
                                    string LotteryCode = LotteryDetal.ChangeLotteryCode(number);
                                    info = LotteryCode + "开始采集";
                                    log.Info("Url_Caike网" + info);
                                    count = await LotteryDetal.LoadWin310LotteryDetal(number);
                                    info = LotteryCode + "采集完毕，新采集了" + count + "条";
                                    log.Info("Url_Caike网" + info);
                                }

                            }
                            catch (Exception ex)
                            {

                                log.Error(info + ex.Message);
                            }
                        }
                    }
                    //KaiJiangWang();
                }
                catch (Exception ex)
                {
                    log.Error(info + ex.Message);
                }
                Thread.Sleep(60 * 1000);
            }
        }

        public async Task Run1122()
        {
            var JddManager = new JDDLottery(_IJddDataService);
            while (true)
            {
                try
                {
                    int count = 0;
                    #region 
                    log.Info("奖多多非高频开始采集");
                    count = await JddManager.LoadJdd("nonhighfreq");
                    log.Info("奖多多非高频共采集" + count + "条");
                    #endregion

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                try
                {
                    KaiJiangWang();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                Thread.Sleep(60 * 1000);
            }
        }


        public async Task RunCaikeBall()
        {
            var service = new CaiKe_SportData(_sport_DataService);
            service.Start();
        }
    }
}
