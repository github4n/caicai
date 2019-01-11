using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EntityModel.Model;
using HtmlAgilityPack;
using Lottery.Services;
using Lottery.Services.Abstractions;
using Lottery.Modes.Entity;
using System.Threading;
using Lottery.GatherApp.Helper;
using log4net;

namespace Lottery.GatherApp
{
    public class SportData
    {
        protected ISport_DataService _SportService;
        private ILog log;
        public SportData(ISport_DataService Sport_DataService)
        {
            _SportService = Sport_DataService;
            log = LogManager.GetLogger("LotteryRepository", typeof(XML_DataService));
        }
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            try
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★北京单场开始爬取★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                GetBjdc();
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★北京单场爬取数据完成★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                System.Threading.Tasks.Task.Delay(5000);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★竞彩足球开始爬取★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                GetJCZQ();
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★竞彩足球爬取数据完成★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                System.Threading.Tasks.Task.Delay(5000);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★竞彩篮球开始爬取★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                GetJCLQ();
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★竞彩篮球爬取数据完成★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                System.Threading.Tasks.Task.Delay(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// 北京单场
        /// </summary>
        private void GetBjdc()
        {
            try
            {
                var Past_IssueNo = _SportService.GetNowIssuNo("zqdc");
                var anode = CommonHelper.GetBJDCExpect("http://zx.500.com/zqdc/kaijiang.php");
                var NowIssueNo = anode.Select(x => Convert.ToInt32(x.Attributes["value"].Value)).Where(x => x >= Convert.ToInt32(Past_IssueNo)).ToList();
                foreach (var code in NowIssueNo)
                {
                    List<jczq> jczqs = new List<jczq>();
                    var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/zqdc/kaijiang.php?&expect=" + code, CollectionUrlEnum.url_500zx).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
                    if (tableNode == null)
                    {  
                        Console.WriteLine($"奖期{code}北京单场获取根节点失败");
                        continue;
                    }
                    //获取平均欧赔
                    var trNodes = CommonHelper.LoadGziphtml("http://zx.500.com/zqdc/kaijiang.php?playid=1&expect=" + code, CollectionUrlEnum.url_500zx).DocumentNode.SelectSingleNode("//table[@class='ld_table']").SelectNodes("tr").Skip(1);
                    if (trNodes == null)
                    {
                        Console.WriteLine($"奖期{code}北京单场获取平均欧赔节点失败");
                        continue;
                    }
                    var trNode = tableNode.SelectNodes("tr").Skip(1);

                    int OpIndex = 1;
                    jczq jczq;
                    //赛果开奖情况
                    foreach (var item in trNode)
                    {
                        OpIndex++;
                        jczq = new jczq();
                        jczq.id = code.ToString();
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
                        jczq.AvgOuCompensation = jczq.AvgOuCompensation.Substring(0, jczq.AvgOuCompensation.Length - 1);
                        jczqs.Add(jczq);
                    }
                    _SportService.Add_BJDC(jczqs);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        /// <summary>
        /// 竞彩足球
        /// </summary>
        private void GetJCZQ()
        {
            try
            {
                DateTime olddate = Convert.ToDateTime(_SportService.GetJCZQ_JCDate())==null|| String.IsNullOrEmpty(_SportService.GetJCZQ_JCDate())==true? DateTime.Now.AddMonths(-1): Convert.ToDateTime(_SportService.GetJCZQ_JCDate());
                string date = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");
                var span = (Convert.ToDateTime(date) - olddate).Days;
                for (int h = 0; h < span; h++)
                {
                    var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?d=" + olddate.AddDays(h).ToString("yyyy-MM-dd"), CollectionUrlEnum.url_500zx).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
                    if (tableNode == null)
                    {
                        Console.WriteLine($"奖期{olddate.AddDays(h).ToString("yyyy-MM-dd")}竞猜足球获取根节点失败");
                        continue;
                    }
                    //获取平均欧赔
                    var trNodess = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?playid=4&d=" + olddate.AddDays(h).ToString("yyyy-MM-dd"), CollectionUrlEnum.url_500zx).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
                    if (trNodess == null)
                    {
                        Console.WriteLine($"奖期{olddate.AddDays(h).ToString("yyyy-MM-dd")}竞猜足球获取平均欧赔失败");
                        continue;
                    }
                    var trNodes = trNodess.SelectNodes("tr").Skip(1);
                    var trNode = tableNode.SelectNodes("tr").Skip(1);
                    int OpIndex = 1;
                    List<jczq> jczqs = new List<jczq>();
                    jczq jczq;
                    var game = new GameType();
                    var GameTypes = new List<GameType>();
                    //赛果开奖情况
                    foreach (var item in trNode)
                    {
                        OpIndex++;
                        jczq = new jczq();
                        jczq.id = olddate.AddDays(h).ToString("yyyy-MM-dd");
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
                                    GameTypes.Clear();
                                    game = new GameType();
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
                                if (i == 7)
                                {
                                    game = new GameType();
                                    game.game = Game.比分;
                                    game.FruitColor = item3.SelectNodes("td")[i].InnerHtml;
                                }
                                if (i == 9)
                                {
                                    game.Bonus = item3.SelectNodes("td")[i].InnerHtml;
                                    GameTypes.Add(game);
                                    jczq.gameTypes.AddRange(GameTypes);
                                  
                                }

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
                    _SportService.Add_JCZQ(jczqs);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        /// <summary>
        /// 竞彩篮球
        /// </summary>
        private void GetJCLQ()
        {
            try
            {
                DateTime olddate = Convert.ToDateTime(_SportService.GetJCLQ_JCDate())==null || String.IsNullOrEmpty(_SportService.GetJCLQ_JCDate()) == true ? DateTime.Now.AddMonths(-1) : Convert.ToDateTime(_SportService.GetJCLQ_JCDate()).AddDays(-1);
                DateTime date = DateTime.Now;
                int ts = (date - olddate).Days;
                for (int h = 0; h <= ts; h++)
                {
                    //竞彩篮球单关数据
                    var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/jclq/kaijiang.php??playid=0&ggid=0&d=" + olddate.AddDays(h), CollectionUrlEnum.url_500zx).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
                    if (tableNode == null)
                    {
                        Console.WriteLine($"奖期{olddate.AddDays(h)}竞彩篮球获取根节点失败");
                        continue;
                    }
                    //竞彩篮球过关数据
                    var GgtableNodes = CommonHelper.LoadGziphtml("http://zx.500.com/jclq/kaijiang.php??playid=0&ggid=1&d=" + olddate.AddDays(h), CollectionUrlEnum.url_500zx).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
                    if(GgtableNodes == null)
                    {
                        Console.WriteLine($"奖期{olddate.AddDays(h)}竞彩篮球获取过关数据失败");
                        continue;
                    }
                    var GgtableNode = GgtableNodes.SelectNodes("tr").Skip(1);
                    //获取平均欧赔
                    var trNodess = CommonHelper.LoadGziphtml("http://zx.500.com/jclq/kaijiang.php?playid=1&d=" + olddate.AddDays(h), CollectionUrlEnum.url_500zx).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
                    if (trNodess == null)
                    {
                        Console.WriteLine($"奖期{olddate.AddDays(h)}竞彩篮球获取平均欧赔失败");
                        continue;
                    }
                    var trNodes = trNodess.SelectNodes("tr").Skip(1);
                    var trNode = tableNode.SelectNodes("tr").Skip(1);
                    int OpIndex = 1;
                    List<jclq_result> jclq_results = new List<jclq_result>();
                    jclq_result jclq_result;
                    //赛果开奖情况
                    foreach (var item in trNode)
                    {
                        OpIndex++;
                        jclq_result = new jclq_result();
                        jclq_result.MatchId = olddate.AddDays(h).ToString("yyyy-MM-dd").Replace("-", "");
                        jclq_result.JCDate = olddate.AddDays(h).ToString("yyyy-MM-dd");
                        //单关数据
                        int tdIndex = 1;
                        foreach (var item2 in item.SelectNodes("td"))
                        {
                            string strText = Regex.Replace(item2.InnerHtml, "<[^>]+>", "");//不包含>的任意字符，字符个数不限，但至少一个字符
                            if (strText == "&nbsp;" || strText == "VS" || strText == "" || strText == "--")
                            {
                                continue;
                            }
                            switch (tdIndex)
                            {
                                case 1:
                                    jclq_result.MatchNumber = strText;
                                    break;
                                case 2:
                                    jclq_result.League_Color = item2.Attributes["style"].Value.Replace("background-color:", "").Replace(";", "");
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
                                if (i == 7)
                                {
                                    jclq_result.GG_SF_Result = item4.SelectNodes("td")[i].InnerHtml;
                                }
                                if (i == 10)
                                {
                                    jclq_result.GG_RFSF_Result = item4.SelectNodes("td")[i].InnerHtml;
                                }
                                if (i == 12)
                                {
                                    jclq_result.GG_SFC_Result = item4.SelectNodes("td")[i].InnerHtml;
                                }
                                if (i == 15)
                                {
                                    jclq_result.GG_DXF_Result = item4.SelectNodes("td")[i].InnerHtml;
                                }
                            }
                            break;
                        }
                        jclq_result.AvgEu_SP = jclq_result.AvgEu_SP.Substring(0, jclq_result.AvgEu_SP.Length - 1);
                        jclq_results.Add(jclq_result);
                    }
                    _SportService.Add_JCLQ(jclq_results);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
    public class DigitalLottery
    {
        protected IDigitalLotteryService _digitalLotteryService;
        private ILog log;
        public DigitalLottery(IDigitalLotteryService DigitalLotteryService)
        {
            _digitalLotteryService = DigitalLotteryService;
            log = LogManager.GetLogger("LotteryRepository", typeof(XML_DataService));

        }
        public void Start()
        {
            try
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★福彩3D开始爬取★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                GetFc3Ds();
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★福彩3D爬取数据完成★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★足球单场胜负过关开始爬取★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                GetZqdc_SfggExpect();
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~★足球单场胜负过关爬取数据完成★~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        private void GetFc3Ds()
        {
            var anode = CommonHelper.GetExpect("http://kaijiang.500.com/sd.shtml");
            var Issue = _digitalLotteryService.Getnormal_lotteryIssue();
            List<fc3D> fc3Ds = new List<fc3D>();
            foreach (HtmlNode item in anode)
            {
                int.TryParse(item.InnerHtml, out int result);
                if (result >= Convert.ToInt32(Issue))
                {
                    fc3D fc3D = new fc3D();
                    List<LotteryInfo> lotteries = new List<LotteryInfo>();
                    var htmlDoc = CommonHelper.LoadGziphtml("http://kaijiang.500.com/shtml/sd/" + item.InnerHtml + ".shtml", CollectionUrlEnum.url_500kaijiang);
                    #region 第一个表格
                    var GameTime = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0].SelectNodes("tr")[0].SelectSingleNode("//span[@class='span_right']").InnerHtml;
                    fc3D.expect = item.InnerHtml;
                    fc3D.LotteryDate = GameTime.Split("：")[1].Split('兑')[0];
                    fc3D.AwardDeadline = GameTime.Split("：")[2];
                    fc3D.SalesVolume = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']").FirstOrDefault().SelectNodes("tr")[2].SelectSingleNode("//span[@class='cfont1 ']").InnerHtml.Replace("元", "");

                    var firstTableNode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0].SelectNodes("tr")[1].SelectSingleNode("td").SelectSingleNode("table").SelectSingleNode("tr").SelectNodes("td");
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
                                    fc3D.opencode += Sub_subitem.InnerHtml + ",";
                                }
                                fc3D.opencode = fc3D.opencode.Remove(fc3D.opencode.Length - 1, 1);
                                i++;
                            }
                            else if (i == 2)
                            {
                                var textNumber = Subitem.SelectSingleNode("div").InnerHtml.Split("：")[1].Replace(' ', ',');
                                if (textNumber.StartsWith(','))
                                {
                                    fc3D.TestNumber = textNumber.Remove(0, 1);
                                }
                                else
                                {
                                    fc3D.TestNumber = textNumber;
                                }
                                i++;
                            }
                            else
                            {
                                fc3D.numberType = Subitem.SelectSingleNode("//font[@class='cfont1']").InnerHtml;
                                i++;
                            }
                        }
                    }
                    #endregion
                    #region 第二个表格
                    var table_tr = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[1].SelectNodes("tr");

                    for (int m = 0; m < table_tr.Count; m++)//遍历tr
                    {
                        if (m <= 1)
                        {
                            continue;
                        }
                        else//第二个tr开始爬取数据
                        {
                            if (m < 19)
                            {
                                if (table_tr[m].SelectNodes("td")[0].OuterHtml.Contains("rowspan"))
                                {
                                    for (int n = 0; n < Convert.ToInt32(table_tr[m].SelectNodes("td")[0].Attributes[0].Value); n++)//获取跨越的行
                                    {
                                        if (n == 0)
                                        {
                                            LotteryInfo fc = new LotteryInfo();
                                            fc.Prize = table_tr[m].SelectNodes("td")[0].InnerHtml.Trim();
                                            fc.PrizeSubItem = table_tr[m].SelectNodes("td")[1].InnerHtml.Trim();
                                            fc.BettingCount = table_tr[m].SelectNodes("td")[2].InnerHtml.Trim();
                                            fc.Bonus = table_tr[m].SelectNodes("td")[3].InnerHtml.TrimStart().Trim();
                                            lotteries.Add(fc);
                                        }
                                        if (n >= 1)
                                        {
                                            LotteryInfo fc = new LotteryInfo();
                                            fc.Prize = table_tr[m].SelectNodes("td")[0].InnerHtml;
                                            fc.PrizeSubItem = table_tr[m + n].SelectNodes("td")[0].InnerHtml.Trim();
                                            fc.BettingCount = table_tr[m + n].SelectNodes("td")[1].InnerHtml.Trim();
                                            fc.Bonus = table_tr[m + n].SelectNodes("td")[2].InnerHtml.Trim();
                                            lotteries.Add(fc);
                                        }
                                    }
                                    m = m + Convert.ToInt32(table_tr[m].SelectNodes("td")[0].Attributes[0].Value) - 1;
                                }
                                else
                                {
                                    LotteryInfo fc = new LotteryInfo();
                                    fc.Prize = table_tr[m].SelectNodes("td")[0].InnerHtml.Trim();
                                    fc.BettingCount = table_tr[m].SelectNodes("td")[1].InnerHtml.Trim();
                                    fc.Bonus = table_tr[m].SelectNodes("td")[2].InnerHtml.Trim();
                                    lotteries.Add(fc);
                                }
                            }
                        }
                    }
                    #endregion
                    fc3D.SubItemList = lotteries;
                    fc3Ds.Add(fc3D);
                    _digitalLotteryService.Addnormal_lotterydetail(fc3Ds);
                }
            }
        }
        /// <summary>
        /// 足球单场-胜负过关开奖SP
        /// </summary>
        /// <returns></returns>
        private void GetZqdc_SfggExpect()
        {
            var anode = CommonHelper.GetBJDCExpect("http://zx.500.com/zqdc/kaijiang.php?playid=6");
            var Issue = _digitalLotteryService.GetZqdc_Sfgg();
            foreach (HtmlNode item in anode)
            {
                int.TryParse(item.Attributes["value"].Value, out int result);
                if (result >=Convert.ToInt32(Issue))
                {
                    _digitalLotteryService.AddZqdc_Sfgg(GetZqdc_Sfgg(item.Attributes["value"].Value));
                }
            }
        }
        /// <summary>
        /// 足球单场-胜负过关开奖SP
        /// </summary>
        /// <returns></returns>
        private List<zqdc_sfgg_result> GetZqdc_Sfgg(string expect)
        {
            var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/zqdc/kaijiang.php?playid=6&expect=" + expect, CollectionUrlEnum.url_500zx).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
            var trNode = tableNode.SelectNodes("tr").Skip(1);
            List<zqdc_sfgg_result> zqdc_Sfgg_Results = new List<zqdc_sfgg_result>();
            //赛果开奖情况
            foreach (var item in trNode)
            {
                zqdc_sfgg_result zqdc_Sfgg_Result = new zqdc_sfgg_result();
                int tdIndex = 1;
                var game = new GameType();
                var GameTypes = new List<GameType>();
                foreach (var item2 in item.SelectNodes("td"))
                {
                    string strText = Regex.Replace(item2.InnerHtml, "<[^>]+>", "");//不包含>的任意字符，字符个数不限，但至少一个字符
                    if (strText == "&nbsp;" || strText == "")
                    {
                        continue;
                    }
                    switch (tdIndex)
                    {
                        case 1:
                            zqdc_Sfgg_Result.MatchNumber = strText;
                            break;
                        case 2:
                            zqdc_Sfgg_Result.BallType_Color = item2.SelectSingleNode("a|span").Attributes["style"].Value.Replace("background-color:", "").Split(";",StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                            zqdc_Sfgg_Result.BallType = strText;
                            break;
                        case 3:
                            zqdc_Sfgg_Result.LeagueName = strText;
                            break;
                        case 4:
                            zqdc_Sfgg_Result.MatchDate = strText;
                            break;
                        case 5:
                            zqdc_Sfgg_Result.HomeTeam = strText;
                            break;
                        case 6:
                            zqdc_Sfgg_Result.LetBall = strText;
                            break;
                        case 7:
                            zqdc_Sfgg_Result.GuestTeam = strText;
                            break;
                        case 8:
                            zqdc_Sfgg_Result.FullScore = strText;
                            break;
                        case 9:
                            zqdc_Sfgg_Result.SF_Result = strText;
                            break;
                        case 10:
                            zqdc_Sfgg_Result.SF_SP = strText;
                            break;
                        case 11:
                            zqdc_Sfgg_Result.AvgEu_SP += strText + ",";
                            tdIndex--;
                            break;
                    }
                    tdIndex = tdIndex + 1;
                }
                zqdc_Sfgg_Result.IssueNo = expect;
                zqdc_Sfgg_Result.AvgEu_SP = zqdc_Sfgg_Result.AvgEu_SP.Substring(0, zqdc_Sfgg_Result.AvgEu_SP.Length - 1);
                zqdc_Sfgg_Results.Add(zqdc_Sfgg_Result);
            }
            return zqdc_Sfgg_Results;
        }
    }
}
