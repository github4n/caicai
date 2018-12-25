using EntityModel.Common;
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
            foreach (var item in anode)
            {
               
                index++;

                if (_ILotteryDetailService.GetNowIssuNo(gameCode) != null)
                {
                    if (item.IssueNo == _ILotteryDetailService.GetNowIssuNo(gameCode).IssueNo)
                    {
                        break;
                    }
                }
                lotterydetail lotterydetail = new lotterydetail();
                lotterydetail.expect = item.IssueNo;
                lotterydetail.Sys_IssueId = _ILotteryDetailService.GetIssue(item.IssueNo).Id;
                try
                {

               
                    var htmlDoc = CommonHelper.LoadGziphtml("http://kaijiang.500.com/shtml/"+gameCode+"/"+item.IssueNo + ".shtml");
                    PubPlsFirstTable(htmlDoc, lotterydetail, lotterydetails, gameCode);
                    PubSecondTable(htmlDoc, lotterydetail, gameCode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                Console.WriteLine(index);
            }
            int count = await _ILotteryDetailService.AddLotteryDetal(lotterydetails, gameCode);
            return count;
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
                        lotterydetail.endTime = Convert.ToDateTime(EndTime).ToString("yyyy-MM-dd"); ;
                        lotterydetails.Add(lotterydetail);
                        break;
                    case 2:
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
                        break;
                    case 3:
                        lotterydetail.SalesVolume = item2.SelectSingleNode("td").SelectSingleNode("span").InnerHtml;

                        if (gameCode != "pls")
                        {
                            lotterydetail.PoolRolling = item2.SelectSingleNode("td").SelectNodes("span")[1].InnerHtml;
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
                if (2 < trIndex && trIndex < lnode)
                {

                    IEnumerable<HtmlNode> getTdList = item3.SelectNodes("td");
                    int tdIndex = 1;
                    var jq4LotteryDetails = new LotteryDetails();
                    if (gameCode == "ttcx4")
                    {
                        foreach (var tdItem in getTdList)
                        {
                            switch (tdIndex)
                            {
                                case 1:
                                    jq4LotteryDetails.openPrize = Regex.Replace(tdItem.InnerHtml, @"\s", "");
                                    break;
                            }
                        }
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
    }
}
