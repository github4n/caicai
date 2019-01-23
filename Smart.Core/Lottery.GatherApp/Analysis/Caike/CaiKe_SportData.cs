using log4net;
using Lottery.GatherApp.Helper;
using Lottery.Modes.OtherModel;
using Lottery.Services.Abstractions;
using Smart.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Smart.Core.Utils.CommonHelper;

namespace Lottery.GatherApp.Analysis.Caike
{
    public class CaiKe_SportData
    {
        protected ISport_DataService _SportService;
        private ILog log;
        private static string Url_Caike { get; set; }
        public CaiKe_SportData(ISport_DataService Sport_DataService)
        {
            _SportService = Sport_DataService;
            log = LogManager.GetLogger("LotteryRepository", typeof(CaiKe_SportData));
            Url_Caike= Smart.Core.Utils.ConfigFileHelper.Get("Url_Caike");
        }
        public void Start()
        {
            log.Info(Url_Caike+"BJDC开始");
            GetBJDC();
            log.Info(Url_Caike + "BJDC结束");
            log.Info(Url_Caike + "JCZQ开始");
            GetJCZQ();
            log.Info(Url_Caike + "JCZQ结束");
            log.Info(Url_Caike + "JCLQ开始");
            GetJCLQ();
            log.Info(Url_Caike + "JCLQ结束");
        }
        private void GetBJDC()
        {
            var BJDC_url = "Trade/DrawInfo/jingjiDraw.aspx?lotteryType=20011";
            var url = Url_Caike + BJDC_url;
            var str = CommonHelper.Post(url, "action=loaddata&pageIndex=1", Encoding.UTF8, CollectionUrlEnum.url_caike);
            List<Caike_matchDates> matchDates = new List<Caike_matchDates>();
            if (!string.IsNullOrEmpty(str))
            {
                var model = JsonHelper.Deserialize<CaikeCommonCollection>(str);
                if (model != null)
                {
                    matchDates = model.body.matchDates;
                }
            }
            foreach (var item in matchDates)
            {
                var DataList = GetBJDCList(item.name.ToString());
                int m = _SportService.AddCaiKeBJDC(DataList, item.name.ToString());
                log.Info(Url_Caike + $"BJDC更新{m}条数据");
               
            }
        }
        private void GetJCZQ()
        {
            try
            {
                var JCDate = _SportService.GetJCZQ_JCDate();
                var OldDate = Convert.ToDateTime(JCDate).AddDays(-1).ToString("yyyyMMdd");
                TimeSpan ts = DateTime.Now - Convert.ToDateTime(JCDate).AddDays(-1);
                for (int i = 0; i < Math.Ceiling(ts.TotalDays); i++)
                {
                    var OldList = GetJczqList(OldDate);
                    int m = _SportService.AddCaikeJCZQ(OldList, Convert.ToDateTime(JCDate).AddDays(i).ToString("yyyyMMdd"), Convert.ToDateTime(JCDate).AddDays(1));
                    log.Info(Url_Caike + $"JCZQ更新{m}条数据");
              
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void GetJCLQ()
        {
            try
            {
               
                var JCDate = _SportService.GetJCLQ_JCDate();
                var OldDate = Convert.ToDateTime(JCDate).AddDays(-1).ToString("yyyyMMdd");
                TimeSpan ts = DateTime.Now - Convert.ToDateTime(JCDate).AddDays(-1);
                for (int i = 0; i < Math.Ceiling(ts.TotalDays); i++)
                {
                    var OldList = GetJclqList(OldDate);
                    int m = _SportService.AddCaiKeJCLQ(OldList, Convert.ToDateTime(JCDate).AddDays(i).ToString("yyyyMMdd"), Convert.ToDateTime(JCDate).AddDays(1));
                    log.Info(Url_Caike + $"JCLQ更新{m}条数据");
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Caike_Body GetJclqList(string matchDateCode="")
        {
            try
            {
                var jclq_url = "Trade/DrawInfo/jingjiDraw.aspx?lotteryType=10012";
                if (!string.IsNullOrEmpty(matchDateCode))
                {
                    jclq_url += "&matchDateCode=" + matchDateCode;
                }
                var url = Url_Caike + jclq_url;
                int page = 1;
                var result = new Caike_Body() { matchDates = new List<Caike_matchDates>() };
                while (true)
                {
                    var str = CommonHelper.Post(url, "action=loaddata&" + "pageIndex=" + page, Encoding.UTF8, CollectionUrlEnum.url_caike);
                    if (!string.IsNullOrEmpty(str))
                    {
                        var model = JsonHelper.Deserialize<CaikeCommonCollection>(str);
                        if (model != null)
                        {
                            result.matchDates = model.body.matchDates;
                            result.records.AddRange(model.body.records);
                            if (!model.body.hasNext)
                            {
                                return result;
                            }
                            else
                            {
                                page++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ":" + ex.StackTrace);
                return null;
            }
            
        }
        private Caike_Body GetJczqList(string matchDateCode = "")
        {
            try
            {
                var jczq_url = "Trade/DrawInfo/jingjiDraw.aspx?lotteryType=10011";
                if (!string.IsNullOrEmpty(matchDateCode))
                {
                    jczq_url += "&matchDateCode=" + matchDateCode;
                }
                var url = Url_Caike + jczq_url;
                int page = 1;
                var result = new Caike_Body() { matchDates = new List<Caike_matchDates>() };
                while (true)
                {
                    var str = CommonHelper.Post(url, "action=loaddata&" + "pageIndex=" + page, Encoding.UTF8, CollectionUrlEnum.url_caike);
                    if (!string.IsNullOrEmpty(str))
                    {
                        var model = JsonHelper.Deserialize<CaikeCommonCollection>(str);
                        if (model != null)
                        {
                            result.matchDates = model.body.matchDates;
                            result.records.AddRange(model.body.records);
                            if (!model.body.hasNext)
                            {
                                return result;
                            }
                            else
                            {
                                page++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ":" + ex.StackTrace);
                return null;
            }

        }
        private Caike_Body GetBJDCList(string matchDateCode = "")
        {
            try
            {
                var BJDC_url = "Trade/DrawInfo/jingjiDraw.aspx?lotteryType=20011";
                if (!string.IsNullOrEmpty(matchDateCode))
                {
                    BJDC_url += "&matchDateCode=" + matchDateCode;
                }
                var url = Url_Caike + BJDC_url;
                int page = 1;
                var result = new Caike_Body() { matchDates = new List<Caike_matchDates>() };
                while (true)
                {
                    
                    var str = CommonHelper.Post(url, "action=loaddata&" + "pageIndex=" + page, Encoding.UTF8, CollectionUrlEnum.url_caike);
                    if (!string.IsNullOrEmpty(str))
                    {
                        var model = JsonHelper.Deserialize<CaikeCommonCollection>(str);
                        if (model != null)
                        {
                            result.matchDates = model.body.matchDates;
                            result.records.AddRange(model.body.records);
                            if (!model.body.hasNext)
                            {
                                return result;
                            }
                            else
                            {
                                page++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ":" + ex.StackTrace);
                return null;
            }
        }
    }
}
