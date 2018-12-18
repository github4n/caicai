using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EntityModel.Common;
using EntityModel.Model;
using HtmlAgilityPack;
using Lottery.Services;
using Lottery.Services.Abstractions;
using Lottery.Modes.Entity;
namespace Lottery.GatherApp
{
    public class SportData
    {
        protected ISport_DataService _SportService;
        public SportData(ISport_DataService Sport_DataService)
        {
            _SportService = Sport_DataService;
        }
        public void GetBjdc()
        {
            var IssueNo = _SportService.GetNowIssuNo("zqdc");
            var anode = CommonHelper.GetBJDCExpect("http://zx.500.com/zqdc/kaijiang.php");
            string Expect = string.Empty;
            foreach (HtmlNode code in anode)
            {
                if (Convert.ToInt32(code.Attributes["value"].Value) > Convert.ToInt32(IssueNo))
               {
                    Expect = code.Attributes["value"].Value;
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
                            jczq.id = Expect;
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
            }
        }
        public void GetJCZQ()
        {
            DateTime olddate =Convert.ToDateTime(_SportService.GetNowGame());
            string date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var span = (Convert.ToDateTime(date) - olddate).Days;
            for (int h = 0; h < span; h++)
            {
                var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?d=" + olddate.AddDays(h).ToString("yyyy-MM-dd")).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
                //获取平均欧赔
                var trNodes = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?playid=1&d=" + olddate.AddDays(h).ToString("yyyy-MM-dd")).DocumentNode.SelectSingleNode("//table[@class='ld_table']").SelectNodes("tr").Skip(1);

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
                    jczq.id = date;
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
                    jczq.AvgOuCompensation = jczq.AvgOuCompensation.Substring(0, jczq.AvgOuCompensation.Length - 1);
                    jczqs.Add(jczq);
                }
                _SportService.Add_JCZQ(jczqs);
            }
        }
        public void GetJCLQ()
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
            List<jclq_result> jclq_results = new List<jclq_result>();
            jclq_result jclq_result;
            //赛果开奖情况
            foreach (var item in trNode)
            {
                OpIndex++;
                jclq_result = new jclq_result();
                jclq_result.MatchId = date.Replace("-", "");
                jclq_result.JCDate = date;
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
                        if (i == 12)
                        {
                            jclq_result.GG_DXF_Result = item4.SelectNodes("td")[i].InnerHtml;
                        }
                    }
                    break;
                }
                jclq_result.AvgEu_SP = jclq_result.AvgEu_SP.Substring(0, jclq_result.AvgEu_SP.Length - 1);
                jclq_results.Add(jclq_result);
            }
        }
    }
}
