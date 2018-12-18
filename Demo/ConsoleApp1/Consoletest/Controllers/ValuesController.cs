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
        /// 6场半全采集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<zc6> Getzc6()
        {
            var anode = CommonHelper.GetExpect("http://kaijiang.500.com/zc6.shtml");

            List<zc6> jq4Lists = new List<zc6>();
            foreach (HtmlNode item in anode)
            {

                var list = GetZc6ByRule(item.InnerText);
                jq4Lists.AddRange(list);

            }
            return jq4Lists;
        }

        public static List<zc6> GetZc6ByRule(string urlNumber)
        {
            List<zc6> jq4Lists = new List<zc6>();
            if (Convert.ToInt32(urlNumber) > 18171)
            {
                var html = "http://kaijiang.500.com/shtml/zc6/" + urlNumber + ".shtml";
                HtmlWeb web = new HtmlWeb();
                CommonHelper.Gzip(web);
                var htmlDoc = web.Load(html);

                var firstTableNode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0];
                var firstTable_trnode = firstTableNode.SelectNodes("tr");
                int k = 1;
                zc6 jq4 = new zc6();
                var listTeams = new List<Team>();
                foreach (HtmlNode item in firstTable_trnode)  //循环第一个table的tr
                {
                    var zc6 = new zc6();
                    switch (k)
                    {
                        case 1:
                            var Date = item.SelectSingleNode("//span[@class='span_right']").InnerHtml;
                            string openTime = Date.Split('：')[1].Split('兑')[0];
                            string EndTime = Date.Split('：')[2];

                            jq4.expect = urlNumber;
                            jq4.openTime = openTime;
                            jq4.endTime = EndTime;
                            jq4Lists.Add(jq4);
                            break;

                        case 2:

                            for (int i = 0; i < item.SelectNodes("td").Count; i++)
                            {
                                var teams = new Team();
                                teams.TeamTitle = item.SelectNodes("td")[i].Attributes["title"].Value.Replace("&nbsp;","");
                                teams.openTeam= item.SelectNodes("td")[i].InnerHtml;
                                listTeams.Add(teams);
                                listTeams.Add(teams);
                            }

                            break;

                        case 3:
                            for (int i = 0; i < item.SelectNodes("td").Count; i++)
                            {
                                listTeams[i].halfull = item.SelectNodes("td")[i].InnerHtml;
                            }
                            break;
                        case 4:
                            for (int i = 0; i < item.SelectNodes("td").Count; i++)
                            {
                                listTeams[i].openCode = item.SelectNodes("td")[i].SelectSingleNode("./span").InnerHtml;
                            }
                            break;
                        case 5:
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
                        var jq4LotteryDetails = new LotteryDetails();
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
                jq4.teams.AddRange(listTeams);
            }
            return jq4Lists;

        }

        /// <summary>
        /// 大乐透开奖信息
        /// </summary>
        /// <returns></returns>
        List<dlt> dltLists = new List<dlt>();
        [HttpGet]
        public List<dlt> GetDlt()
        {
            var anode = CommonHelper.GetExpect("http://kaijiang.500.com/dlt.shtml");
            foreach (HtmlNode item in anode)
            {
                dltLists = GetDltByRule(item.InnerText);
            }
            return dltLists;
        }

        public List<dlt> GetDltByRule(string urlNumber)
        {
           
            if (Convert.ToInt32(urlNumber) > 18144)
            {
                HtmlWeb web = new HtmlWeb();
                CommonHelper.Gzip(web);
                var htmlDoc = web.Load("http://kaijiang.500.com/shtml/dlt/" + urlNumber + ".shtml");
                var FirstTableTrNode= htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0].SelectNodes("tr");
                int k = 1;
                dlt dlt = new dlt();
                foreach (var item in FirstTableTrNode)//遍历第一个table下的tr
                {
                    switch (k)
                    {
                        case 1:
                            var Date = item.SelectSingleNode("//span[@class='span_right']").InnerHtml;
                            string openTime = Date.Split('：')[1].Split('兑')[0];
                            string EndTime = Date.Split('：')[2];

                            dlt.expect = urlNumber;
                            dlt.openTime = openTime;
                            dlt.endTime = EndTime;
                            dltLists.Add(dlt);
                            break;
                        case 2:
                            int j = 1;
                            foreach (var item2 in item.SelectSingleNode("td").SelectSingleNode("table").SelectNodes("tr"))
                            {
                                switch (j)
                                {
                                    case 1:
                                        var lilist = item2.SelectNodes("td")[1].SelectSingleNode("div").SelectSingleNode("ul").SelectNodes("li");
                                        foreach (var item3 in lilist)
                                        {
                                            dlt.OpenCode += item3.InnerHtml + ",";

                                        }
                                        dlt.OpenCode.Trim(',');
                                        break;
                                    case 2:
                                        dlt.OutOfOrder = item2.SelectNodes("td")[1].InnerHtml;
                                        break;

                                }
                                j++;
                            }

                            break;
                        case 3:
                            dlt.SalesVolume = item.SelectSingleNode("td").SelectNodes("span")[0].InnerHtml;
                            dlt.PoolRolling = item.SelectSingleNode("td").SelectNodes("span")[1].InnerHtml;
                            break;

                    }
                    k = k + 1;
                }
                var dltList = new List<dltList>();
                var SecondTableTrNode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[1].SelectNodes("tr").Skip(2);//第二个table

                int index = 0;
                foreach (var item in SecondTableTrNode)
                {
                    if (item.SelectNodes("td")[0].InnerHtml == "派奖")
                    {
                        continue;
                    }
                    index++;
                    if (index % 2 != 0 && index<12)
                    {
                        int j = 0;
                        var model = new dltList();
                        for (int i = 0; i < item.SelectNodes("td").Count; i++)
                        {
                            j++;

                            if (j == 1)
                            {
                                var hhh = item.SelectNodes("td")[i].InnerHtml;
                                model.openPrize = item.SelectNodes("td")[i].InnerHtml;
                                dltList.Add(model);
                            }
                            if (j == 2)
                            {
                                model.openPrizeType = openPrizeType.basic;
                            }
                            if (j == 3)
                            {
                                model.openWinNumber = item.SelectNodes("td")[i].InnerHtml;
                            }
                            if (j == 4)
                            {
                                model.openSingleBonus = item.SelectNodes("td")[i].InnerHtml;
                            }
                            if (j == 5)
                            {
                                model.openSumBonus = item.SelectNodes("td")[i].InnerHtml;
                                dlt.dltLists.Add(model);
                            }
                        }
                    }
                    if (index % 2 == 0 && index < 12)
                    {
                        int j = 0;
                        int indexPrize = 0;
                        var model = new dltList();
                        for (int i = 0; i < item.SelectNodes("td").Count; i++)
                        {
                            j++;
                        
                            foreach (var itemPrize in dlt.dltLists)
                            {
                                indexPrize++;
                                if (indexPrize == dlt.dltLists.Count)
                                {
                                    model.openPrize = itemPrize.openPrize;
                                }
                               
                            }
                            if (j == 1)
                            {
                                model.openPrizeType = openPrizeType.Append;
                            }
                            if (j == 2)
                            {
                                model.openWinNumber = item.SelectNodes("td")[i].InnerHtml;
                            }
                            if (j == 3)
                            {
                                model.openSingleBonus = item.SelectNodes("td")[i].InnerHtml;
                            }
                            if (j == 4)
                            {
                                model.openSumBonus = item.SelectNodes("td")[i].InnerHtml;
                                dlt.dltLists.Add(model);
                            }
                        
                        }
                    }
                    if (index == 12)
                    {
                        dlt.TotalBonus = Convert.ToDecimal(item.SelectNodes("td")[3].InnerHtml);
                     
                    }
                }
            }
            return dltLists;
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
                var listTeams = new List<Team>();
                foreach (HtmlNode item in firstTable_trnode)  //循环第一个table的tr
                {
                    switch (k)
                    {
                        case 1:
                            var Date = item.SelectSingleNode("//span[@class='span_right']").InnerHtml;
                            string openTime = Date.Split('：')[1].Split('兑')[0];
                            string EndTime = Date.Split('：')[2];

                            jq4.expect = urlNumber;
                            jq4.openTime = openTime;
                            jq4.endTime = EndTime;
                            jq4Lists.Add(jq4);
                            break;
                        case 2:
                            for (int i = 0; i < item.SelectNodes("td").Count; i++)
                            {
                                var teams = new Team();
                                teams.TeamTitle = item.SelectNodes("td")[i].Attributes["title"].Value;
                                teams.openTeam = item.SelectNodes("td")[i].InnerHtml;
                                listTeams.Add(teams);
                                
                            }
                            break;

                        case 3:
                            for (int i = 0; i < item.SelectNodes("td").Count; i++)
                            {
                                listTeams[i].openCode= item.SelectNodes("td")[i].SelectSingleNode("./span").InnerHtml;
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
                        var jq4LotteryDetails = new LotteryDetails();
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
                jq4.teams.AddRange(listTeams);
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
            //获取平均欧赔
            var trNodes = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?playid=1&d=" + date).DocumentNode.SelectSingleNode("//table[@class='ld_table']").SelectNodes("tr").Skip(1);

            var trNode = tableNode.SelectNodes("tr").Skip(1);
        
            int OpIndex = 1;
            List<jczq> jczqs = new List<jczq>();
            jczq jczq;

            //赛果开奖情况
            foreach (var item in trNode)
            {
                OpIndex++;
                jczq = new jczq();
                int tdIndex = 1;
                var game = new GameType();
                var GameTypes = new List<GameType>();
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
                            jczq.League_Color = item2.SelectSingleNode("a").Attributes["style"].Value.Replace("background-color:", "");
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
                            game.game = Game.让球胜平负;
                            game.FruitColor = strText;
       
                            break;
                        case 9:
                            game.Bonus = strText;
                            GameTypes.Add(game);
                            break;
                        case 10:
                            game = new GameType();
                            game.game = Game.胜平负;
                            game.FruitColor = strText;
                            break;
                        case 11:
                            game.Bonus = strText;
                            GameTypes.Add(game);
                            break;
                        case 12:
                            game = new GameType();
                            game.game = Game.总进球数;
                            game.FruitColor = strText;
                            break;
                        case 13:
                            game.Bonus = strText;
                            GameTypes.Add(game);
                            break;
                        case 14:
                            game = new GameType();
                            game.game = Game.半全场;
                            game.FruitColor = strText;
                            break;
                        case 15:
                            game.Bonus = strText;
                            GameTypes.Add(game);
                            jczq.gameTypes.AddRange(GameTypes);
                            break;

                    }
                    

                
                    tdIndex = tdIndex + 1;


                }
              
                int indexop = 1;
                foreach (var item3 in trNodes)
                {
                        indexop++;
                        if (indexop != OpIndex)
                        {
                            continue;
                        }
                        for (int i = 0; i < item3.SelectNodes("td").Count(); i++)
                        {
                            if (i > 10 && i < 14)
                            {
                                jczq.AvgOuCompensation += item3.SelectNodes("td")[i].InnerHtml + ",";

                            }
                            if (i >= 14)
                            {
                                break;
                            }
                        }
                        break;
                   
                }
                jczq.AvgOuCompensation=jczq.AvgOuCompensation.Substring(0, jczq.AvgOuCompensation.Length-1);
                jczqs.Add(jczq);
            }

           
            return jczqs;
        }

        /// <summary>
        /// 竞彩足球今日总结
        /// </summary>
        /// <returns></returns>
        public List<SummaryInfo> GetJczqJrzjInfo()
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


        /// <summary>
        /// 获取北京单场期号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<jczq> GetBjdcExpect()
        {
            var anode = CommonHelper.GetBJDCExpect("http://zx.500.com/zqdc/kaijiang.php");

            List<jczq> jq4Lists = new List<jczq>();
            foreach (HtmlNode item in anode)
            {

                var list = GetBjdc(item.Attributes["value"].Value);
                jq4Lists.AddRange(list);

            }
            return jq4Lists;
        }

        /// <summary>
        /// 北京单场开奖SP
        /// </summary>
        /// <returns></returns>
        public List<jczq> GetBjdc(string Expect)
        {

            List<jczq> jczqs = new List<jczq>();
            if (Expect == "181203")
            {
                var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/zqdc/kaijiang.php?&expect=" + Expect).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
                //获取平均欧赔
                var trNodes = CommonHelper.LoadGziphtml("http://zx.500.com/zqdc/kaijiang.php?playid=1&expect=" + Expect).DocumentNode.SelectSingleNode("//table[@class='ld_table']").SelectNodes("tr").Skip(1);

                var trNode = tableNode.SelectNodes("tr").Skip(1);

                int OpIndex = 1;



                jczq jczq;

                //赛果开奖情况
                foreach (var item in trNode)
                {
                    OpIndex++;
                    jczq = new jczq();
                    int tdIndex = 1;
                    var game = new GameType();
                    var GameTypes = new List<GameType>();
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
                                jczq.League_Color = item2.SelectSingleNode("a").Attributes["style"].Value.Replace("background-color:", "");
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
                                game.game = Game.让球胜平负;
                                game.FruitColor = strText;
                                break;
                            case 9:
                                game.Bonus = strText;
                                GameTypes.Add(game);
                                break;
                            case 10:
                                game = new GameType();
                                game.game = Game.总进球数;
                                game.FruitColor = strText;
                                break;
                            case 11:
                                game.Bonus = strText;
                                GameTypes.Add(game);
                                break;
                            case 12:
                                game = new GameType();
                                game.game = Game.比分;
                                game.FruitColor = strText;
                                break;
                            case 13:
                                game.Bonus = strText;
                                GameTypes.Add(game);
                                break;
                            case 14:
                                game = new GameType();
                                game.game = Game.上下单双;
                                game.FruitColor = strText;
                                break;
                            case 15:
                                game.Bonus = strText;
                                GameTypes.Add(game);
                                break;
                            case 16:
                                game = new GameType();
                                game.game = Game.半全场;
                                game.FruitColor = strText;
                                break;
                            case 17:
                                game.Bonus = strText;
                                GameTypes.Add(game);
                                jczq.gameTypes.AddRange(GameTypes);
                                break;

                        }



                        tdIndex = tdIndex + 1;


                    }

                    int indexop = 1;
                    foreach (var item3 in trNodes)
                    {
                        indexop++;
                        if (indexop != OpIndex)
                        {
                            continue;
                        }
                        for (int i = 0; i < trNodes.Count(); i++)
                        {
                            if (i > 10 && i < 14)
                            {
                                jczq.AvgOuCompensation += item3.SelectNodes("td")[i].InnerHtml + ",";

                            }
                            if (i >= 14)
                            {
                                break;
                            }
                        }
                        break;

                    }
                    jczq.AvgOuCompensation = jczq.AvgOuCompensation.Substring(0, jczq.AvgOuCompensation.Length - 1);
                    jczqs.Add(jczq);
                }

            }
            return jczqs;
            
        }


        /// <summary>
        /// 竞彩足球赛果开奖
        /// </summary>
        /// <returns></returns>
        public List<jclq_result> GetJclq()
        {
            string date = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            //竞彩篮球单关数据
            var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/jclq/kaijiang.php??playid=0&ggid=0&d=" + date).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
            //竞彩篮球过关数据
            var GgtableNode = CommonHelper.LoadGziphtml("http://zx.500.com/jclq/kaijiang.php??playid=0&ggid=1&d=" + date).DocumentNode.SelectSingleNode("//table[@class='ld_table']").SelectNodes("tr").Skip(1);
            //获取平均欧赔
            var trNodes = CommonHelper.LoadGziphtml("http://zx.500.com/jclq/kaijiang.php?playid=1&d=" + date).DocumentNode.SelectSingleNode("//table[@class='ld_table']").SelectNodes("tr").Skip(1);

            var trNode = tableNode.SelectNodes("tr").Skip(1);

            int OpIndex = 1;
            List<jclq_result> jczqs = new List<jclq_result>();
            jclq_result jclq_result;

            //赛果开奖情况
            foreach (var item in trNode)
            {
                OpIndex++;
                jclq_result = new jclq_result();
                //单关数据
                int tdIndex = 1;
                foreach (var item2 in item.SelectNodes("td"))
                {

                    string strText = Regex.Replace(item2.InnerHtml, "<[^>]+>", "");//不包含>的任意字符，字符个数不限，但至少一个字符
                    if (strText == "&nbsp;" || strText == "VS" || strText == "" || strText== "--")
                    {
                        continue;
                    }
                    switch (tdIndex)
                    {
                        case 1:
                            jclq_result.MatchNumber = strText;
                            break;
                        case 2:
                            jclq_result.League_Color = item2.Attributes["style"].Value.Replace("background-color:", "");
                            jclq_result.LeagueName = strText;
                            break;
                        case 3:
                            jclq_result.MatchDate = strText;
                            break;
                        case 4:
                            jclq_result.GuestTeam = strText;
                            break;
                        case 5:
                            jclq_result.HomeTeam = strText;
                            break;
                        case 6:
                            jclq_result.FullScore = strText;
                            break;
                        case 7:
                            jclq_result.SF_Result = strText;
                            break;
                        case 8:
                            jclq_result.LetBall = strText;
                            break;
                        case 9:
                            jclq_result.RFSF_Result = strText;
                      
                            break;
                        case 10:
                            jclq_result.SFC_Result = strText;
                         
                            break;
                        case 11:
                            jclq_result.YSZF = strText;
                            break;
                        case 12:
                            jclq_result.DXF_Result = strText;
                            break;
                      
                    }



                    tdIndex = tdIndex + 1;


                }

                //获取篮球平均欧赔
                int indexop = 1;
                foreach (var item3 in trNodes)
                {
                    indexop++;
                    if (indexop != OpIndex)
                    {
                        continue;
                    }
                    for (int i = 0; i < item3.SelectNodes("td").Count(); i++)
                    {
                        if (i > 9 && i < 12)
                        {
                            jclq_result.AvgEu_SP += item3.SelectNodes("td")[i].InnerHtml + ",";

                        }
                        if (i >= 12)
                        {
                            break;
                        }
                    }
                    break;

                }
                //过关数据
                int Ggindex = 1;
                foreach (var item4 in GgtableNode)
                {
                    Ggindex++;
                    if (Ggindex != OpIndex)
                    {
                        continue;
                    }
                    for (int i = 0; i < item4.SelectNodes("td").Count(); i++)
                    {
                        if (i ==7)
                        {
                            jclq_result.GG_SF_Result= item4.SelectNodes("td")[i].InnerHtml;
                        }
                        if (i == 10)
                        {
                            jclq_result.GG_RFSF_Result = item4.SelectNodes("td")[i].InnerHtml;
                        }
                        if (i == 12)
                        {
                            jclq_result.GG_SFC_Result = item4.SelectNodes("td")[i].InnerHtml;
                        }
                        if (i == 12)
                        {
                            jclq_result.GG_DXF_Result = item4.SelectNodes("td")[i].InnerHtml;
                        }
                    }
                    break;
                }


                jclq_result.AvgEu_SP = jclq_result.AvgEu_SP.Substring(0, jclq_result.AvgEu_SP.Length - 1);
                jczqs.Add(jclq_result);
            }


            return jczqs;
        }

    }
}
