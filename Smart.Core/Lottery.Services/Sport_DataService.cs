using System;
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
            List<bjdc_result> bjdc_s = new List<bjdc_result>();
            List<bjdc_result> UpdateList = new List<bjdc_result>();
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

            var NoFinish = GetNotFinish(GameCode);
            if (NoFinish != null && NoFinish.Count > 0)
            {
                UpdateList = bjdc_s.Where(x => NoFinish.Contains(x.IssueNo)).ToList();
                foreach (var oldItem in UpdateList)
                {
                    if (!string.IsNullOrEmpty(oldItem.RQSPF_Result) && oldItem.RQSPF_Result != "-" &&
                    !string.IsNullOrEmpty(oldItem.ZJQ_Result) && oldItem.ZJQ_Result != "-" &&
                    !string.IsNullOrEmpty(oldItem.FullScore) && oldItem.FullScore != "-" &&
                    !string.IsNullOrEmpty(oldItem.SXDS_Result) && oldItem.SXDS_Result != "-" &&
                    !string.IsNullOrEmpty(oldItem.BQC_Result) && oldItem.BQC_Result != "-")
                    {
                        oldItem.IsFinish = true;
                    }
                    else
                    {
                        oldItem.IsFinish = false;
                    }
                }
                if (UpdateList != null && UpdateList.Count > 0)
                {
                    bjdc_s.RemoveAll((bjdc_result br) => { return NoFinish.Contains(br.IssueNo); });//删除bjdc列表中存在的数据
                    db.Updateable(UpdateList).ExecuteCommand();
                }
            }
            db.Insertable(bjdc_s).ExecuteCommand();
        }
        /// <summary>
        /// 竞彩篮球
        /// </summary>
        public void Add_JCLQ(List<jclq_result> model, string GameCode = "jclq")
        {
            foreach (var item in model)
            {
                item.MatchId = item.MatchId + item.MatchNumber;
                item.CreateTime = DateTime.Now;
            }
            var Issue = GetJCLQ_JCDate();
            var result = GetJCLQResultsByIssueNo(Issue).Select(x => x.MatchId);
            var temp = model.Where(x => result.Contains(x.MatchId)).ToList();
            if (temp != null && temp.Count > 0)
            {
                model.RemoveAll((jclq_result jr) => { return result.Contains(jr.MatchId); });//移除已经存在的数据
                db.Updateable(temp).ExecuteCommand();
            }
            db.Insertable(model).ExecuteCommand();
        }
        /// <summary>
        /// 竞彩足球
        /// </summary>
        public void Add_JCZQ(List<jczq> model, string GameCode = "jczq")
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
            var IssuNo = GetJCZQ_JCDate();
            var result= GetJCZQResultsByIssueNo(IssuNo).Select(x=>x.MatchId);
            var temp = jczq_Results.Where(x => result.Contains(x.MatchId)).ToList();
            if (temp != null && temp.Count > 0)
            {
                jczq_Results.RemoveAll((jczq_result jr) => { return result.Contains(jr.MatchId); });//移除已经存在的数据
                db.Updateable(temp).ExecuteCommand();
            }
            db.Insertable(jczq_Results).ExecuteCommand();
        }
        /// <summary>
        /// 获取最近三场没有完成的场次(BJDC)
        /// </summary>
        /// <returns></returns>
        public List<string> GetNotFinish(string GameCode)
        {
            var issuNo = GetNow3IssuNo(GameCode);
            return db.Queryable<bjdc_result>().Where(x => x.IsFinish == false && issuNo.Contains(x.IssueNo)).Select(x => x.IssueNo).ToList();
        }
        /// <summary>
        /// 获取最近3期(BJDC)
        /// </summary>
        /// <returns></returns>
        public List<string> GetNow3IssuNo(string GameCode)
        {
            return db.Queryable<sys_issue>().Where(x => x.LotteryCode == GameCode).OrderBy(x => x.IssueNo, OrderByType.Desc).Select(x => x.IssueNo).Take(3).ToList();
        }
        /// <summary>
        /// 获取最迟的彩期
        /// </summary>
        /// <param name="GameCode"></param>
        /// <returns></returns>
        public string GetNowIssuNo(string GameCode)
        {
            return db.Queryable<sys_issue>().Where(x => x.LotteryCode == GameCode).OrderBy(x => x.IssueNo, OrderByType.Desc).Select(x => x.IssueNo).First();
        }
        /// <summary>
        /// 获取数据库最迟的竞彩时间(JCZQ)
        /// </summary>
        /// <returns></returns>
        public string GetJCZQ_JCDate()
        {
            return db.Queryable<jczq_result>().OrderBy(x=>x.JCDate,OrderByType.Desc).Select(x=>x.JCDate).First();
        }
        public List<jczq_result> GetJCZQResultsByIssueNo(string IssueNo)
        {
            return db.Queryable<jczq_result>().Where(x => x.JCDate == IssueNo).ToList();
        }
        public List<jclq_result> GetJCLQResultsByIssueNo(string IssueNo)
        {
            return db.Queryable<jclq_result>().Where(x => x.JCDate == IssueNo).ToList();
        }
        /// <summary>
        /// 获取数据库最迟的竞彩时间(JCLQ)
        /// </summary>
        /// <returns></returns>
        public string GetJCLQ_JCDate()
        {
            return db.Queryable<jclq_result>().OrderBy(x => x.JCDate, OrderByType.Desc).Select(x => x.JCDate).First();
        }
        
    }
}
