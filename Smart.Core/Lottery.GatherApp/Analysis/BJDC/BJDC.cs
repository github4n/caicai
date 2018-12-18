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

namespace Lottery.GatherApp
{
    public class BJDC
    {
        protected ISport_DataService _SportService;
        public BJDC(ISport_DataService Sport_DataService)
        {
            _SportService = Sport_DataService;
        }
        public void GetBjdc()
        {
            var anode = CommonHelper.GetBJDCExpect("http://zx.500.com/zqdc/kaijiang.php");
            string Expect = string.Empty;
            foreach (HtmlNode code in anode)
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
                    _SportService.Add_BJDC(jczqs);
                }
            }
        }
        public void GetJCZQ()
        {
            string date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var tableNode = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?d=" + date).DocumentNode.SelectSingleNode("//table[@class='ld_table']");
            //获取平均欧赔
            var trNodes = CommonHelper.LoadGziphtml("http://zx.500.com/jczq/kaijiang.php?playid=1&d=" + date).DocumentNode.SelectSingleNode("//table[@class='ld_table']").SelectNodes("tr").Skip(1);

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
            _SportService.Add_JCZQ(jczqs);
        }
    }
}
