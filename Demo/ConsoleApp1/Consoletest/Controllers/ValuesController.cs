using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EntityModel.Common;
using EntityModel.Model;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace Consoletest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //  //表示查找所有childnodes；
        //  一个斜杠'/'表示只查找第一层的childnodes（即不查找grandchild）；
        //  点斜杠"./"表示从当前结点而不是根结点开始查找。接上一行代码，比如要查找table所有直接子结点的tr:


        /// <summary>
        /// 4场进球采集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<jq4> GetJq4information()
        {
            var anode= CommonHelper.GetExpect("http://kaijiang.500.com/jq4.shtml");
            DataModel jq4 = new DataModel();
            List<jq4> jq4Lists = new List<jq4>();
            foreach (HtmlNode item in anode)
            {
                jq4.expect = item.InnerText;
                var list = GetByRule(item.InnerText);
                jq4Lists.AddRange(list);
              
            }
            return jq4Lists;
        }

        /// <summary>
        /// 大乐透开奖信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<jq4> GetDlt()
        {
            var anode = CommonHelper.GetExpect("http://kaijiang.500.com/dlt.shtml");
            DataModel jq4 = new DataModel();
            List<jq4> jq4Lists = new List<jq4>();
            foreach (HtmlNode item in anode)
            {
                jq4.expect = item.InnerText;
                var list = GetByRule(item.InnerText);
                jq4Lists.AddRange(list);

            }
            return jq4Lists;
        }


        public static List<jq4> GetByRule(string urlNumber)
        {
            List<jq4> jq4Lists = new List<jq4>();
            if (Convert.ToInt32(urlNumber) > 18171)
            {
                var html = "http://kaijiang.500.com/shtml/jq4/" + urlNumber + ".shtml";
                HtmlWeb web = new HtmlWeb();

                CommonHelper.Gzip(web);
                var htmlDoc = web.Load(html);

                var firstTableNode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0];
                var firstTable_trnode = firstTableNode.SelectNodes("tr");
                int k = 1;
                jq4 jq4 = new jq4();
                foreach (HtmlNode item in firstTable_trnode)  //循环第一个table的tr
                {
                    switch (k)
                    {
                        case 1:
                            var Date = item.SelectSingleNode("//span[@class='span_right']");
                            string Gametime = Date.InnerHtml;
                            string openTime = Gametime.Split('：')[1].Split('兑')[0];
                            string EndTime = Gametime.Split('：')[2];

                            jq4.expect = urlNumber;
                            jq4.openTime = openTime;
                            jq4.endTime = EndTime;
                            jq4Lists.Add(jq4);
                            break;

                        case 2:
                            foreach (var item2 in item.SelectNodes("td"))
                            {
                                string GameTeam = item2.InnerHtml;


                                foreach (var item3 in jq4Lists)
                                {
                                    var jq4Team = new jq4Team();
                                    jq4Team.openTeam = GameTeam.ToString().Replace("&nbsp;", "");
                                    item3.openTeam.Add(jq4Team);
                                }
                            }
                            break;

                        case 3:
                            foreach (var item2 in item.SelectNodes("td"))
                            {
                                string GameCode = item2.SelectSingleNode("./span").InnerHtml;

                                foreach (var item3 in jq4Lists)
                                {
                                    var jq4Code = new jq4Core();
                                    jq4Code.openCode = GameCode;
                                    item3.openCode.Add(jq4Code);
                                }


                            }
                            break;

                        case 4:
                            //取列数据 
                            IEnumerable<HtmlNode> getSpanList = item.SelectSingleNode("td").SelectNodes("span");
                            int spanNumber = 1;
                            foreach (var spanItem in getSpanList)
                            {
                                switch (spanNumber)
                                {
                                    case 1:
                                        jq4.SalesVolume = spanItem.InnerHtml;
                                        break;
                                    case 2:
                                        jq4.PoolRolling = spanItem.InnerHtml;
                                        break;
                                }
                                spanNumber = spanNumber + 1;
                            }
                            string SalesVolume = item.InnerHtml;
                            break;
                    }

                    k = k + 1;
                }
                var SecondTablenode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[1];
                var SecondTable_trnode = SecondTablenode.SelectNodes("tr");
                int trIndex = 1;
                foreach (HtmlNode item in SecondTable_trnode)  //循环第二个table的tr
                {
                    int lnode = SecondTable_trnode.Count();
                    if (2 < trIndex && trIndex < lnode)
                    {

                        IEnumerable<HtmlNode> getTdList = item.SelectNodes("td");
                        int tdIndex = 1;
                        var jq4LotteryDetails = new jq4LotteryDetails();
                        foreach (var tdItem in getTdList)
                        {
                            switch (tdIndex)
                            {

                                case 1:
                                    jq4LotteryDetails.openPrize = tdItem.InnerHtml;
                                    break;
                                case 2:
                                    jq4LotteryDetails.openWinNumber = tdItem.InnerHtml;
                                    break;
                                case 3:
                                    jq4LotteryDetails.openSingleBonus = tdItem.InnerHtml;
                                    break;
                            }
                            tdIndex = tdIndex + 1;
                        }
                        foreach (var item3 in jq4Lists)
                        {
                            item3.openLotteryDetails.Add(jq4LotteryDetails);
                        }

                    }
                    trIndex = trIndex + 1;
                }

            }
            return jq4Lists;

        }



        /// <summary>
        /// 竞彩足球赛果开奖
        /// </summary>
        /// <returns></returns>
        public List<jczq>  GetJczq()
        {
            string date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?d=" + date).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
            
            var trNode = tableNode.SelectNodes("tr");
            int index = 1;
            List<jczq> jczqs = new List<jczq>();
            jczq jczq;
            //赛果开奖情况
            foreach (var item in trNode)
            {
                if (index == 1)
                {
                    index = 0;
                    continue;
                }
                 jczq = new jczq();
                int tdIndex = 1;
                foreach (var item2 in item.SelectNodes("td"))
                {
                 
                    string strText = Regex.Replace(item2.InnerHtml, "<[^>]+>", "");//不包含>的任意字符，字符个数不限，但至少一个字符
                    if (strText == "&nbsp;") 
                    {
                        continue;
                    }
                    switch (tdIndex)
                    {
                        case 1:
                            jczq.TournamentNumber = strText;
                            break;
                        case 2:
                            jczq.TournamentType = strText;
                            break;
                        case 3:
                            jczq.MatchTime = strText;
                            break;
                        case 4:
                            jczq.HomeTeam = strText;
                            break;
                        case 5:
                            jczq.LetBall = strText;
                            break;
                        case 6:
                            jczq.VisitingTeam = strText;
                            break;
                        case 7:
                            jczq.Score = strText;
                            break;
                        case 8:
                            jczq.rQSPF.FruitColor = strText;
                            break;
                        case 9:
                            jczq.rQSPF.Bonus = Convert.ToDecimal(strText);
                            break;
                        case 10:
                            jczq.sPF.FruitColor = strText;
                            break;
                        case 11:
                            jczq.sPF.Bonus = Convert.ToDecimal(strText);
                            break;
                        case 12:
                            jczq.zJQS.FruitColor = strText;
                            break;
                        case 13:
                            jczq.zJQS.Bonus = Convert.ToDecimal(strText);
                            break;
                        case 14:
                            jczq.bQC.FruitColor = strText;
                            break;
                        case 15:
                            jczq.bQC.Bonus = Convert.ToDecimal(strText);
                            break;

                    }
                  
                    tdIndex = tdIndex + 1;


                }
                jczqs.Add(jczq);
            }
           

            return jczqs;
        }

        /// <summary>
        /// 竞彩足球今日总结
        /// </summary>
        /// <returns></returns>
        public List<SummaryInfo> GetJczqJrkjInfo()
        {
            string date = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            //今日总结
            var SummaryTodayNode = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?d=" + date).DocumentNode.SelectSingleNode("//table[@class='pub_table']");

            var StNodeTr = SummaryTodayNode.SelectNodes("tr");
            int StIndex = 1;
            int index = 1;
            var list = new List<SummaryInfo>();
            foreach (var item in StNodeTr)
            {
                if (StIndex == 1)//StIndex=1玩法，否则其它
                {
                    int k = 1;
                    for (int i = 0; i < item.SelectNodes("th").Count; i++)
                    {
                        if (k == 1)
                        {
                            k = 0;
                            continue;
                        }
                        var model = new SummaryInfo();
                        model.GameName = item.SelectNodes("th")[i].InnerHtml;
                        list.Add(model);
                    }
                }
                else
                {
                   
                    if (index == 1)//平均奖金值
                    {
                        
                        for (int i = 0; i < item.SelectNodes("td").Count; i++)
                        {
                            if (i == 0)
                            {
                                continue;
                            }
                            list[i-1].AvgBonus = item.SelectNodes("td")[i].InnerHtml;
                       
                        }
                      
                    }
                    string key = ""; string value = ""; int spanIndex = 1;
                    if (index == 2)//赛果分布第一行
                    {

                        foreach (var item2 in item.SelectNodes("td"))
                        {
                            int i = 0;
                            if (item2.SelectNodes("span") != null)
                            {
                                for (i = 0; i < item.SelectNodes("td").Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        continue;
                                    }
                                    foreach (var lists in CommonHelper.StNode(item2, key, value, spanIndex))
                                    {
                                     
                                        list[i - 1].ResultList.Add(lists.Key, lists.Value);
                                        
                                    }

                                }

                            }
                            if (i == item.SelectNodes("td").Count)//如果相等，跳出item2的循环，继续item循环
                            {
                                break;
                            }
                           
                        }
                        
                    }
                    if (index >= 3)//余下的赛果分布
                    {
                        int i = 0;
                        foreach (var item2 in item.SelectNodes("td"))
                        {
                            i++;
                            if (item2.SelectNodes("span") != null)
                            {
                                foreach (var lists in CommonHelper.StNode(item2, key, value, spanIndex))
                                {
                                    list[i - 1].ResultList.Add(lists.Key, lists.Value);
                                }

                            }
                            if (i == item.SelectNodes("td").Count)
                            {
                                break;
                            }

                        }
                    }
                    index++;

                }
                StIndex = StIndex + 1;
            }

            return list;

        }

   

    }
}
