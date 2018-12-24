﻿using System;
using System.Collections.Generic;
using System.Text;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System.Threading.Tasks;
using EntityModel.Model;
using System.Linq;

namespace Lottery.Services
{
    /// <summary>
    /// 球类Service///北京单场，竞彩足球,传统足球，竞彩篮球
    /// </summary>
    public class Sport_DataService : Repository<DbFactory>, ISport_DataService
    {
        protected SqlSugarClient db = null;
        public Sport_DataService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }
        /// <summary>
        /// 北京单场
        /// </summary>
        /// <param name="model"></param>
        public void Add_BJDC(List<jczq> model, string GameCode = "zqdc")
        {
            try
            {
                List<bjdc_result> bjdc_s = new List<bjdc_result>();
                foreach (var item in model)
                {
                    bjdc_result resultModel = new bjdc_result
                    {
                        MatchId = item.id + item.TournamentNumber,
                        MatchDate = item.MatchTime,
                        MatchNumber = item.TournamentNumber,
                        HomeTeam = item.HomeTeam,
                        GuestTeam = item.VisitingTeam,
                        LetBall = item.LetBall,
                        HalfScore = item.Score == "-" ? "-" : item.Score.Split(")")[0].Replace("(", "").Replace(")", ""),
                        FullScore = item.Score == "-" ? "-" : item.Score.Split(")")[1].Replace(")", ""),
                        LeagueName = item.TournamentType,
                        CreateTime = DateTime.Now,
                        IssueNo = item.id,
                    };
                    resultModel.League_Color = item.League_Color;
                    foreach (var Sub_item in item.gameTypes)
                    {
                        if (Sub_item.game == Game.让球胜平负)
                        {
                            resultModel.RQSPF_Result = Sub_item.FruitColor;
                            resultModel.RQSPF_SP = Sub_item.Bonus;
                        }
                        else if (Sub_item.game == Game.总进球数)
                        {
                            resultModel.ZJQ_Result = Sub_item.FruitColor;
                            resultModel.ZJQ_SP = Sub_item.Bonus;
                        }
                        else if (Sub_item.game == Game.比分)
                        {
                            //比分彩果FullScore
                            resultModel.BF_SP = Sub_item.Bonus;
                        }
                        else if (Sub_item.game == Game.上下单双)
                        {
                            resultModel.SXDS_Result = Sub_item.FruitColor;
                            resultModel.SXDS_SP = Sub_item.Bonus;
                        }
                        else if (Sub_item.game == Game.半全场)
                        {
                            resultModel.BQC_Result = Sub_item.FruitColor;
                            resultModel.BQC_SP = Sub_item.Bonus;
                        }
                    }
                    resultModel.AvgEu_SP = item.AvgOuCompensation;
                    resultModel.IssueNo = item.id;
                    resultModel.IsFinish = false;
                    if (!string.IsNullOrEmpty(resultModel.RQSPF_Result) && resultModel.RQSPF_Result != "-" &&
                        !string.IsNullOrEmpty(resultModel.ZJQ_Result) && resultModel.ZJQ_Result != "-" &&
                        !string.IsNullOrEmpty(resultModel.FullScore) && resultModel.FullScore != "-" &&
                        !string.IsNullOrEmpty(resultModel.SXDS_Result) && resultModel.SXDS_Result != "-" &&
                        !string.IsNullOrEmpty(resultModel.BQC_Result) && resultModel.BQC_Result != "-")
                    {
                        resultModel.IsFinish = true;
                    }
                    bjdc_s.Add(resultModel);
                }
                int i = 0, m = 0;
                bjdc_s.ForEach((a) =>
                {
                    bjdc_result result = db.Queryable<bjdc_result>().Where(x => x.MatchId == a.MatchId).First();
                    if (result != null)
                    {
                        var Issure = db.Queryable<bjdc_result>().OrderBy(x => x.IssueNo, OrderByType.Desc).GroupBy(p=>p.IssueNo).Select(p=>p.IssueNo).Skip(2).First();
                        if (result.IsFinish == false&&Convert.ToInt32(result.IssueNo)>Convert.ToInt32(Issure))
                        {
                            db.Updateable(a).ExecuteCommand();
                            m++;
                        }
                    }
                    else
                    {
                        db.Insertable(a).ExecuteCommand();
                        i++;
                    }
                });
                ConSoleHelp("U", GameCode, bjdc_s.Select(x => x.IssueNo).FirstOrDefault(), m);
                ConSoleHelp("A", GameCode, bjdc_s.Select(x => x.IssueNo).FirstOrDefault(), i);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 竞彩篮球
        /// </summary>
        public void Add_JCLQ(List<jclq_result> model, string GameCode = "jclq")
        {
            try
            {
                int i = 0;
                model.ForEach((a) => {
                    a.MatchId = a.MatchId + a.MatchNumber;
                    a.CreateTime = DateTime.Now;
                    bool d = db.Queryable<jclq_result>().Where(x => x.MatchId == a.MatchId).Count() > 0 ? true : false;
                    if (!d)
                    {
                        db.Insertable(a).ExecuteCommand();
                        i++;
                    }
                });
                ConSoleHelp("A", GameCode, model.Select(x => x.JCDate).FirstOrDefault(), i);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 竞彩足球
        /// </summary>
        public void Add_JCZQ(List<jczq> model, string GameCode = "jczq")
        {
            try
            {
                List<jczq_result> jczq_Results = new List<jczq_result>();
                List<jclq_result> List = new List<jclq_result>();
                foreach (var item in model)
                {
                    jczq_result jczq = new jczq_result
                    {
                        MatchId = item.id.Replace("-", "") + item.TournamentNumber,
                        MatchDate = item.MatchTime,
                        MatchNumber = item.TournamentNumber,
                        HomeTeam = item.HomeTeam,
                        GuestTeam = item.VisitingTeam,
                        LetBall = item.LetBall,
                        HalfScore = item.Score == "-" ? "-" : item.Score.Split(")")[0].Replace("(", "").Replace(")", ""),
                        FullScore = item.Score == "-" ? "-" : item.Score.Split(")")[1].Replace(")", ""),
                        LeagueName = item.TournamentType,
                        League_Color = item.League_Color,
                        CreateTime = DateTime.Now,
                        JCDate = item.id,
                    };
                    foreach (var Sub_item in item.gameTypes)
                    {
                        if (Sub_item.game == Game.让球胜平负)
                        {
                            jczq.RQSPF_Result = Sub_item.FruitColor;
                            jczq.RQSPF_SP = Sub_item.Bonus;
                        }
                        else if (Sub_item.game == Game.总进球数)
                        {
                            jczq.ZJQ_Result = Sub_item.FruitColor;
                            jczq.ZJQ_SP = Sub_item.Bonus;
                        }
                        else if (Sub_item.game == Game.比分)
                        {
                            //比分彩果FullScore
                            jczq.BF_SP = Sub_item.Bonus;
                        }
                        else if (Sub_item.game == Game.胜平负)
                        {
                            jczq.SPF_Result = Sub_item.FruitColor;
                            jczq.SPF_SP = Sub_item.Bonus;
                        }
                        else if (Sub_item.game == Game.半全场)
                        {
                            jczq.BQC_Result = Sub_item.FruitColor;
                            jczq.BQC_SP = Sub_item.Bonus;
                        }
                    }
                    jczq_Results.Add(jczq);
                }
                int i = 0;
                jczq_Results.ForEach((a) => {
                    bool b = db.Queryable<jczq_result>().Where(x => x.MatchId == a.MatchId).Count() > 0 ? true : false;
                    if (!b)
                    {
                        db.Insertable(a).ExecuteCommand();
                        i++;
                    }
                });
                ConSoleHelp("A", GameCode, jczq_Results.Select(x => x.JCDate).FirstOrDefault(), i);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取最近三场没有完成的场次(BJDC)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> GetNotFinish(string GameCode)
        {
            try
            {
                var issuNo = GetNow3IssuNo(GameCode);
                return db.Queryable<bjdc_result>().Where(x => x.IsFinish == false && issuNo.Contains(x.IssueNo)).ToList().ToDictionary(x => x.MatchId, y => y.IssueNo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取最近3期(BJDC)
        /// </summary>
        /// <returns></returns>
        public List<string> GetNow3IssuNo(string GameCode)
        {
            try
            {
                return db.Queryable<sys_issue>().Where(x => x.LotteryCode == GameCode).OrderBy(x => x.IssueNo, OrderByType.Desc).Select(x => x.IssueNo).Take(3).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取最迟的彩期
        /// </summary>
        /// <param name="GameCode"></param>
        /// <returns></returns>
        public string GetNowIssuNo(string GameCode)
        {
            try
            {
                return db.Queryable<sys_issue>().Where(x => x.LotteryCode == GameCode).OrderBy(x => x.IssueNo, OrderByType.Desc).Select(x => x.IssueNo).First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取数据库最迟的竞彩时间(JCZQ)
        /// </summary>
        /// <returns></returns>
        public string GetJCZQ_JCDate()
        {
            try
            {
                return db.Queryable<jczq_result>().OrderBy(x => x.JCDate, OrderByType.Desc).Select(x => x.JCDate).First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取数据库最迟的竞彩时间(JCLQ)
        /// </summary>
        /// <returns></returns>
        public string GetJCLQ_JCDate()
        {
            try
            {
                return db.Queryable<jclq_result>().OrderBy(x => x.JCDate, OrderByType.Desc).Select(x => x.JCDate).First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ConSoleHelp(string Type,string GameCode,string Issue,int Count)
        {
            if (Type == "U")
            {
                Console.WriteLine($"{GameCode}奖期{Issue}成功更新数据{Count}条");
            }
            else if(Type=="A")
            {
                Console.WriteLine($"{GameCode}奖期{Issue}成功新增数据{Count}条");
            }
        }
    }
}
