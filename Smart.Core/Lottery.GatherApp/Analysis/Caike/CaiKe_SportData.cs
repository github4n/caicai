using log4net;
using Lottery.GatherApp.Helper;
using Lottery.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

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

        private void GetJCLQ()
        {
            var jclq_url = "/Trade/DrawInfo/jingjiDraw.aspx?lotteryType=10012";
            var url = Url_Caike + jclq_url;
            //CommonHelper.LoadGziphtml()
        }
    }
}
