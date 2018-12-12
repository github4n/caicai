using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityModel.Common;
using EntityModel.Model;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace Consoletest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class flcpController : ControllerBase
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public List<fc3D> GetFc3Ds()
        {
            var anode = CommonHelper.GetExpect("http://kaijiang.500.com/sd.shtml");
            List<fc3D> fc3Ds = new List<fc3D>();
            foreach (HtmlNode item in anode)
            {
                LotteryInfo info = new LotteryInfo();
                info.expect = item.InnerHtml;
                if (Convert.ToInt32(item.InnerHtml) == 2018333)
                {
                    return fc3Ds;
                }
                var html = "http://kaijiang.500.com/shtml/sd/" + item.InnerHtml + ".shtml";
                HtmlWeb web = new HtmlWeb();
                CommonHelper.Gzip(web);
                var htmlDoc = web.Load(html);
                #region 第一个表格
                var GameTime = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0].SelectNodes("tr")[0].SelectSingleNode("//span[@class='span_right']").InnerHtml;
                info.LotteryDate = GameTime.Split("：")[1].Split('兑')[0];
                info.AwardDeadline = GameTime.Split("：")[2];
                info.SalesVolume = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']").FirstOrDefault().SelectNodes("tr")[2].SelectSingleNode("//span[@class='cfont1 ']").InnerHtml.Replace("元","");

                var firstTableNode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0].SelectNodes("tr")[1].SelectSingleNode("td").SelectSingleNode("table").SelectSingleNode("tr").SelectNodes("td");
                //var firstTable_tdnode = firstTableNode.SelectNodes("tr").FirstOrDefault().SelectNodes("td");
                int i = 0;
                foreach (var Subitem in firstTableNode)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    else//第二个开始截取数据
                    {
                        if (i == 1)
                        {
                            foreach (var Sub_subitem in Subitem.SelectNodes("//li[@class='ball_orange']"))
                            {
                                info.opencode += Sub_subitem.InnerHtml + ",";
                            }
                            info.opencode = info.opencode.Remove(info.opencode.Length - 1, 1);
                            i++;
                        }
                        else if (i == 2)
                        {
                            var textNumber = Subitem.SelectSingleNode("div").InnerHtml.Split("：")[1].Replace(' ', ',');
                            if (textNumber.StartsWith(','))
                            {
                                info.TestNumber = textNumber.Remove(0, 1);
                            }
                            else
                            {
                                info.TestNumber = textNumber;
                            }
                            i++;
                        }
                        else
                        {
                            info.numberType = Subitem.SelectSingleNode("//font[@class='cfont1']").InnerHtml;
                            i++;
                        }
                    }
                }
                #endregion
                #region 第二个表格
                var table_tr = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[1].SelectNodes("tr");
                
                for (int m=0;m< table_tr.Count;m++)//遍历tr
                {
                    //table_tr_Subitem in table_tr
                    if (m <= 1)
                    {
                        continue;
                    }
                    else//第二个tr开始爬取数据
                    {
                        if (m < 21)//没数据了
                        {
                            if (table_tr[m].SelectNodes("td")[0].OuterHtml.Contains("rowspan"))
                            {
                                for (int n = 0; n < Convert.ToInt32(table_tr[m].SelectNodes("td")[0].Attributes[0].Value); n++)//获取跨越的行
                                {

                                    if (n == 0)
                                    {
                                        fc3D fc = new fc3D();
                                        fc.Prize = table_tr[m].SelectNodes("td")[0].InnerHtml.TrimStart().TrimEnd();
                                        fc.PrizeType = table_tr[m].SelectNodes("td")[1].InnerHtml.TrimStart().TrimEnd();
                                        fc.BettingCount = table_tr[m].SelectNodes("td")[2].InnerHtml.TrimStart().TrimEnd();
                                        fc.Bonus = table_tr[m].SelectNodes("td")[3].InnerHtml.TrimStart().TrimEnd();
                                        fc.LotteryInfo = info;
                                        fc3Ds.Add(fc);
                                    }
                                    if (n >= 1)
                                    {
                                        fc3D fc = new fc3D();
                                        fc.Prize = table_tr[m].SelectNodes("td")[0].InnerHtml;
                                        fc.PrizeType = table_tr[m + n].SelectNodes("td")[0].InnerHtml.TrimStart().TrimEnd();
                                        fc.BettingCount = table_tr[m + n].SelectNodes("td")[1].InnerHtml.TrimStart().TrimEnd();
                                        fc.Bonus = table_tr[m + n].SelectNodes("td")[2].InnerHtml.TrimStart().TrimEnd();
                                        fc.LotteryInfo = info;
                                        fc3Ds.Add(fc);
                                    }
                                }
                                m = m + Convert.ToInt32(table_tr[m].SelectNodes("td")[0].Attributes[0].Value) - 1;
                            }
                            else
                            {
                                fc3D fc = new fc3D();
                                fc.Prize = table_tr[m].SelectNodes("td")[0].InnerHtml.TrimStart().TrimEnd();
                                fc.BettingCount = table_tr[m].SelectNodes("td")[1].InnerHtml.TrimStart().TrimEnd();
                                fc.Bonus = table_tr[m].SelectNodes("td")[2].InnerHtml.TrimStart().TrimEnd();
                                fc.LotteryInfo = info;
                                fc3Ds.Add(fc);
                            }
                        }
                    }
                }
                #endregion
            }
            return fc3Ds;
        }
       
    }
}