
using HtmlAgilityPack;
using Lottery.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lottery.Modes.Entity;
using Lottery.Modes.Model;
using EntityModel.Model;
using System.Linq;
using System.Text.RegularExpressions;
using Lottery.GatherApp.Helper;

namespace Lottery.GatherApp.Analysis.LotteryDetail
{
   public class NormalLotteryDetail
    {
        protected ILotteryDetailService _ILotteryDetailService;
      

        public NormalLotteryDetail(ILotteryDetailService ILotteryDetailService)
        {
            _ILotteryDetailService = ILotteryDetailService;
      
        }

        public async Task<int> LoadLotteryDetal(string gameCode)
        {
            var anode = _ILotteryDetailService.GetLotteryCodeList(gameCode).Take(30);
            List<lotterydetail> lotterydetails = new List<lotterydetail>();
            int index = 0;
            int SfcIndex = 0;
            foreach (var item in anode)
            {

                index++;
                //查询彩种最新一期
                if (_ILotteryDetailService.GetNowIssuNo(gameCode) != null)
                {

                    if (item.IssueNo == _ILotteryDetailService.GetNowIssuNo(gameCode).IssueNo && gameCode!="sfc" && gameCode != "jq4" && gameCode != "zc6")
                    {
                        break;
                    }
                  //数据库查到有胜负彩最新一期，获取采集下来的3期，用来更新数据
                    if (gameCode == "sfc" || gameCode == "jq4" || gameCode == "zc6")
                    {
                        SfcIndex++;
                        if (SfcIndex == 3)
                        {
                            break;
                        }
                    }
                }
                lotterydetail lotterydetail = new lotterydetail();
                lotterydetail.expect = item.IssueNo;
                lotterydetail.Sys_IssueId = _ILotteryDetailService.GetIssue(item.IssueNo).Id;
                try
                {


                    var htmlDoc = CommonHelper.LoadGziphtml("http://kaijiang.500.com/shtml/" + gameCode + "/" + item.IssueNo + ".shtml");

                    if (gameCode == "sfc" || gameCode == "jq4" || gameCode == "zc6")
                    {
                        PubTeamFirstTable(htmlDoc, lotterydetail, lotterydetails, gameCode);
                    }
                    else
                    {
                        PubPlsFirstTable(htmlDoc, lotterydetail, lotterydetails, gameCode);
                    }
                    if (gameCode == "dlt")
                    {
                        PubDltSecondTable(htmlDoc, lotterydetail, gameCode);
                    }
                    else
                    {
                        PubSecondTable(htmlDoc, lotterydetail, gameCode);
                    }
                  
                    if (gameCode == "gdslxq")
                    {
                        PubThirdTable(htmlDoc, lotterydetail, gameCode);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(gameCode+"彩种"+ex.Message);
                    continue;
                }
              
            }
            int count = await _ILotteryDetailService.AddLotteryDetal(lotterydetails, gameCode);
            return count;
        }

        public void PubTeamFirstTable(HtmlDocument htmlDoc, lotterydetail lotterydetail, List<lotterydetail> lotterydetails, string gameCode)
        {
            var firstTableNode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0];
            var firstTable_trnode = firstTableNode.SelectNodes("tr");
            int k = 1;
            bool TF = true;
            foreach (HtmlNode item in firstTable_trnode)  //循环第一个table的tr
            {
             
                switch (k)
                {
                    case 1:
                        var Date = item.SelectSingleNode("//span[@class='span_right']").InnerHtml;
                        string openTime = Date.Split('：')[1].Split('兑')[0];
                        string EndTime = Date.Split('：')[2];

                        lotterydetail.openTime = Convert.ToDateTime(openTime).ToString("yyyy-MM-dd");
                        lotterydetail.endTime = Convert.ToDateTime(EndTime).ToString("yyyy-MM-dd"); ;
                        lotterydetails.Add(lotterydetail);
                        break;

                    case 2:
                        for (int i = 0; i < item.SelectNodes("td").Count; i++)
                        {
                            var teams = new Team();
                            teams.TeamTitle = item.SelectNodes("td")[i].Attributes["title"].Value.Replace("&nbsp;", "");
                            teams.openTeam = item.SelectNodes("td")[i].InnerHtml.Replace("&nbsp;", "");
                            lotterydetail.teams.Add(teams);

                            if (gameCode == "zc6")
                            {
                                teams = new Team();
                                teams.TeamTitle = item.SelectNodes("td")[i].Attributes["title"].Value.Replace("&nbsp;", "");
                                teams.openTeam = item.SelectNodes("td")[i].InnerHtml.Replace("&nbsp;", "");
                                lotterydetail.teams.Add(teams);
                             
                            }

                            }
                        break;

                    case 3:

                        if (gameCode == "zc6")
                        {
                            if (TF == true)
                            {
                                for (int i = 0; i < item.SelectNodes("td").Count; i++)
                                {
                                    var a = i;
                                    lotterydetail.teams[a].halfull = item.SelectNodes("td")[a].InnerHtml;
                                }
                                TF = false;
                                continue;
                            }
                        }

                        for (int i = 0; i < item.SelectNodes("td").Count; i++)
                        {
                            lotterydetail.teams[i].openCode = item.SelectNodes("td")[i].SelectSingleNode("./span").InnerHtml;
                        }                      
                        break;

                    case 4:
                        //取列数据 
                        lotterydetail.SalesVolume = item.SelectSingleNode("td").SelectSingleNode("span").InnerHtml;

                        if (gameCode == "sfc")
                        {
                            lotterydetail.SalesVolume += "|" + item.SelectSingleNode("td").SelectNodes("span")[1].InnerHtml;
                            lotterydetail.PoolRolling = item.SelectSingleNode("td").SelectNodes("span")[2].InnerHtml;
                        }
                        else {
                            lotterydetail.PoolRolling = item.SelectSingleNode("td").SelectNodes("span")[1].InnerHtml;
                        }
                         
                        break;
                }

                k = k + 1;
            }
        }

        public void PubPlsFirstTable(HtmlDocument htmlDoc, lotterydetail lotterydetail, List<lotterydetail> lotterydetails,string gameCode)
        {
            var FirstTableTrNode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0].SelectNodes("tr");
            int k = 1;
            foreach (var item2 in FirstTableTrNode)//遍历第一个table下的tr
            {
                switch (k)
                {
                    case 1:
                        var Date = item2.SelectSingleNode("//span[@class='span_right']").InnerHtml;
                        string openTime = Date.Split('：')[1].Split('兑')[0];//开奖时间 
                        string EndTime = Date.Split('：')[2];//截止兑奖时间
                        lotterydetail.openTime = Convert.ToDateTime(openTime).ToString("yyyy-MM-dd");
                        lotterydetail.endTime = Convert.ToDateTime(EndTime).ToString("yyyy-MM-dd");
                        lotterydetails.Add(lotterydetail);
                        break;
                    case 2:
                        if (gameCode == "dlt" || gameCode == "eexw" || gameCode == "ssq" || gameCode == "qlc")
                        {
                            int j = 1;
                            foreach (var itemTr in item2.SelectSingleNode("td").SelectSingleNode("table").SelectNodes("tr"))
                            {
                                switch (j)
                                {
                                    case 1:
                                        var lilist = itemTr.SelectNodes("td")[1].SelectSingleNode("div").SelectSingleNode("ul").SelectNodes("li");
                                        foreach (var item3 in lilist)
                                        {
                                            lotterydetail.openCode += item3.InnerHtml + ",";

                                        }
                                        lotterydetail.openCode=lotterydetail.openCode.Trim(',');
                                        break;
                                    case 2:
                                        lotterydetail.openCode += (gameCode == "dlt"?"+":"|") + itemTr.SelectNodes("td")[1].InnerHtml.TrimStart();
                                        break;

                                }
                                j++;
                            }
                        }
                        else
                        {
                            var tdindex = item2.SelectSingleNode("td").SelectSingleNode("table").SelectSingleNode("tr").SelectNodes("td");
                            foreach (var item3 in tdindex)
                            {
                                var lilist = tdindex[1].SelectSingleNode("div").SelectSingleNode("ul").SelectNodes("li");
                                foreach (var item4 in lilist)
                                {
                                    lotterydetail.openCode += item4.InnerHtml + ",";
                                }
                                lotterydetail.openCode = lotterydetail.openCode.Trim(',');

                                //排列三有号码类型
                                if (gameCode == "pls")
                                {
                                    var numberType = tdindex[2].SelectSingleNode("//font[@class='cfont1']").InnerHtml;
                                    lotterydetail.NumberType = numberType;
                                }

                                break;
                            }
                        }
                        break;
                    case 3:

                        lotterydetail.SalesVolume = item2.SelectSingleNode("td").SelectSingleNode("span").InnerHtml;

                        if (gameCode == "gdslxq")
                        {
                            lotterydetail.SalesVolume += "|"+item2.SelectSingleNode("td").SelectNodes("span")[1].InnerHtml;
                        }
                        if (gameCode != "pls" & gameCode != "ttcx4")
                        {
                            if (gameCode == "gdslxq")
                            {
                                lotterydetail.PoolRolling = item2.SelectSingleNode("td").SelectNodes("span")[2].InnerHtml;
                            }
                            else if (gameCode == "plw")
                            {
                                lotterydetail.PoolRolling = item2.SelectSingleNode("td").SelectNodes("span")[1].SelectSingleNode("//span[@class='cfont1']").InnerHtml;
                            }
                            else
                            {
                                lotterydetail.PoolRolling = item2.SelectSingleNode("td").SelectNodes("span")[1].InnerHtml;
                            }

                        }
                        break;


                }
                k = k + 1;

            }
        }

        public void PubSecondTable(HtmlDocument htmlDoc, lotterydetail lotterydetail, string gameCode)
        {
            var SecondTablenode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[1];
            var SecondTable_trnode = SecondTablenode.SelectNodes("tr");
            int trIndex = 1;
            foreach (HtmlNode item3 in SecondTable_trnode)  //循环第二个table的tr
            {

                int lnode = SecondTable_trnode.Count();
                int minLnode = 0;
                if (gameCode == "ttcx4")
                {
                    minLnode = 1;
                    lnode = 3;
                }
               else if (gameCode == "pls" || gameCode == "sfc" || gameCode == "jq4" || gameCode == "zc6" || gameCode == "qxc" || gameCode == "plw" || gameCode == "qlc" || gameCode == "ssq")
                {
                    minLnode = 2;
                    lnode = SecondTable_trnode.Count();
                }
                else
                {
                    minLnode = 2;
                    lnode = SecondTable_trnode.Count()+1;

                }
                if (minLnode < trIndex && trIndex < lnode)
                {

                    IEnumerable<HtmlNode> getTdList = item3.SelectNodes("td");
                    int tdIndex = 1;
                    var jq4LotteryDetails = new LotteryDetails();
                    var ttcx4Detail = new ttcx4Details();
                    if (gameCode == "ttcx4")
                    {
                        foreach (var tdItem in getTdList)
                        {
                            switch (tdIndex)
                            {
                                case 1:
                                    ttcx4Detail.Betting = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 2:
                                    ttcx4Detail.openPrize = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 3:
                                    ttcx4Detail.openCode = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 4:
                                    ttcx4Detail.directlySelection = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 5:
                                    ttcx4Detail.GroupSelection24 = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 6:
                                    ttcx4Detail.GroupSelection12 = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 7:
                                    ttcx4Detail.GroupSelection6 = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 8:
                                    ttcx4Detail.GroupSelection4 = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                            }
                            tdIndex = tdIndex + 1;
                        }
                        lotterydetail.ttcx4Details.Add(ttcx4Detail);
                    }
                    else
                    {
                        foreach (var tdItem in getTdList)
                        {
                            switch (tdIndex)
                            {

                                case 1:
                                    jq4LotteryDetails.openPrize = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 2:
                                    jq4LotteryDetails.openWinNumber = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 3:
                                    jq4LotteryDetails.openSingleBonus = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                            }
                            tdIndex = tdIndex + 1;
                        }
                    }
                    lotterydetail.openLotteryDetails.Add(jq4LotteryDetails);


                }
                trIndex = trIndex + 1;
            }
        }

        public void PubDltSecondTable(HtmlDocument htmlDoc, lotterydetail lotterydetail, string gameCode)
        {
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
                if (index % 2 != 0 && index < 12)
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
                            lotterydetail.dltLists.Add(model);
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

                        foreach (var itemPrize in lotterydetail.dltLists)
                        {
                            indexPrize++;
                            if (indexPrize == lotterydetail.dltLists.Count)
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
                            lotterydetail.dltLists.Add(model);
                        }

                    }
                }
                if (index == 12)
                {
                    lotterydetail.TotalBonus = Convert.ToDecimal(item.SelectNodes("td")[3].InnerHtml);

                }
            }

        }

        public void PubThirdTable(HtmlDocument htmlDoc, lotterydetail lotterydetail, string gameCode)
        {
            var ThirdTablenode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[2];
            var ThirdTable_trnode = ThirdTablenode.SelectNodes("tr");
            int trIndex = 1;
            foreach (HtmlNode item3 in ThirdTable_trnode)  //循环第二个table的tr
            {

                int lnode = ThirdTable_trnode.Count()+1;
             
                if (2 < trIndex && trIndex < lnode)
                {
                    IEnumerable<HtmlNode> getTdList = item3.SelectNodes("td");
                    int tdIndex = 1;
                    var jq4LotteryDetails = new LotteryDetails();
                    var ttcx4Detail = new ttcx4Details();
               
                        foreach (var tdItem in getTdList)
                        {
                            switch (tdIndex)
                            {

                                case 1:
                                    jq4LotteryDetails.openPrize = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 2:
                                    jq4LotteryDetails.openWinNumber = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                                case 3:
                                    jq4LotteryDetails.openSingleBonus = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                            }
                            tdIndex = tdIndex + 1;
                        }                    
                    lotterydetail.openLotteryDetails.Add(jq4LotteryDetails);


                }
                trIndex = trIndex + 1;
            }
        }
    }
}
