using log4net;
using Lottery.GatherApp.Helper;
using Lottery.Modes.OtherModel;
using Lottery.Services.Abstractions;
using Smart.Core.Extensions;
using System;
using System.Collections.Generic;
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
        private void GetJCZQ()
        {
            //DateTime olddate = Convert.ToDateTime(_SportService.GetJCZQ_JCDate()) == null || String.IsNullOrEmpty(_SportService.GetJCZQ_JCDate()) == true ? DateTime.Now.AddMonths(-1) : Convert.ToDateTime(_SportService.GetJCZQ_JCDate());
            //string date = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");
            //var span = (Convert.ToDateTime(date) - olddate).Days;
            //for (int h = 0; h < span; h++)
            //{
            //    var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?d=" + olddate.AddDays(h).ToString("yyyy-MM-dd")).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
            //    if (tableNode == null)
            //    {
            //        Console.WriteLine($"奖期{olddate.AddDays(h).ToString("yyyy-MM-dd")}竞猜足球获取根节点失败");
            //        continue;
            //    }
            //}
        }

        public void GetJCLQ()
        {
            try
            {
                //0.从数据库拿到最后采集的日期，从该日开始采集
                
                //1.彩客数据的日期是往前一天算的(正常11号，彩客10号)
                var result = GetJclqList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取分页的竞猜篮球数据
        /// </summary>
        /// <returns></returns>
        private Caike_Body GetJclqList(string matchDateCode="")
        {
            var jclq_url = "Trade/DrawInfo/jingjiDraw.aspx?lotteryType=10012";
            if (string.IsNullOrEmpty(matchDateCode))
            {
                jclq_url += "&matchDateCode" + matchDateCode;
            }
            var url = Url_Caike + jclq_url;
            var page = "1";
            var result = new Caike_Body() { matchDates = new List<Caike_matchDates>() };
            while (true)
            {
                var str = CommonHelper.Post(url, "action=loaddata;"+ "page="+ page, Encoding.UTF8, CollectionUrlEnum.url_caike);
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
                            page = model.body.afterPage;
                        }
                    }
                }
            }
        }
    }
}
