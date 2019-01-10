using System;
using System.Collections.Generic;
using System.Threading;
using log4net;
using Lottery.GatherApp.Helper;
using Lottery.Modes;
using Lottery.Modes.Model;
using Lottery.Services.Abstractions;

using Newtonsoft.Json;

namespace Lottery.GatherApp.Analysis
{
    public class KaiJiangWangRequest
    {
        //JsonApi
        private string GaoPinUrI = "https://api.950021.com/lottery-client-api/{0}/history?date={1}";
        private string quanguoURL = "https://api.950021.com/lottery-client-api/quanguocai/{0}/history?date={1}";
        private ILog log;
        protected IKaiJiangWangService _kaiJiangWangService;
        public KaiJiangWangRequest(IKaiJiangWangService IkaiJiangWangService)
        {
            _kaiJiangWangService = IkaiJiangWangService;
            log = LogManager.GetLogger("LotteryRepository", typeof(KaiJiangWangRequest));
        }
        public void Start()
        {
            Console.WriteLine("开始高频彩采集");
            RequestJson(KaiJiangWangDic.GaoPindic, GaoPinUrI);
            Console.WriteLine("高频彩采集完毕");
            Console.WriteLine("开始全国彩采集");
            RequestJson(KaiJiangWangDic.QuanguoDic, quanguoURL);
            Console.WriteLine("全国彩采集完毕");
        }
        #region 获取JSON
        private void RequestJson(Dictionary<string, string> dic, string url)
        {

            foreach (var dic_Item in dic)
            {
                try
                {
                    log.Info("1122站点开始采集" + dic_Item.Key + "彩种");
                    DateTime time = default;
                    int span = 0;
                    var Model = _kaiJiangWangService.GetIssue(dic_Item.Key);
                    if (Model == null)
                    {
                        time = DateTime.Now.AddDays(-30);
                        span = 30;
                    }
                    else
                    {
                        time = Convert.ToDateTime(Model.OpenTime);
                        span = (DateTime.Now - time).TotalDays < 0 ? 1 : (DateTime.Now - time).Days;//不满24小时，返回1天
                    }
                    for (var i = 0; i <= span; i++)
                    {
                        var str = CommonHelper.RequestJsonData(string.Format(url, dic_Item.Value, time.AddDays(i).ToString("yyyy-MM-dd")));
                        if (string.IsNullOrEmpty(str))
                        { continue; }
                        var resultList = JsonConvert.DeserializeObject<JsonReuslt>(str);
                        log.Info("1122站点" + dic_Item.Key + "彩种，日期："+ time.AddDays(i).ToString("yyyy-MM-dd")+"采集到："+ resultList.content.Count+"条数据");
                        if (resultList.message == "成功" && resultList.code == 0&& resultList.content.Count>0)
                        {
                            _kaiJiangWangService.AddSys_issue(dic_Item.Key, resultList);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message + ":" + ex.StackTrace);
                }
            }
        }
        #endregion
    }
}
