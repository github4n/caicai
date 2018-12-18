using System;
using System.Collections.Generic;
using System.Text;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System.Threading.Tasks;
using EntityModel.Model;

namespace Lottery.Services
{
    /// <summary>
    /// 球类Service///北京单场，竞彩足球,传统足球，竞彩篮球
    /// </summary>
    public class Sport_DataService: Repository<DbFactory>, ISport_DataService
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
        public void Add_BJDC(List<jczq> model)
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
                    CreateTime=DateTime.Now
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
                    else if (Sub_item.game==Game.上下单双)
                    {
                        resultModel.SXDS_Result = Sub_item.FruitColor;
                        resultModel.SXDS_SP = Sub_item.Bonus;
                    }
                    else if(Sub_item.game==Game.半全场)
                    {
                        resultModel.BQC_Result = Sub_item.FruitColor;
                        resultModel.BQC_SP = Sub_item.Bonus;
                    }
                }
                resultModel.AvgEu_SP = item.AvgOuCompensation;
                resultModel.IssueNo = item.id;
                resultModel.IsFinish = false;
                if (!string.IsNullOrEmpty(resultModel.RQSPF_Result) && !string.IsNullOrEmpty(resultModel.ZJQ_Result) && !string.IsNullOrEmpty(resultModel.FullScore) && !string.IsNullOrEmpty("") && !string.IsNullOrEmpty(resultModel.BQC_Result))
                {
                    resultModel.IsFinish = true;
                }
                bjdc_s.Add(resultModel);
            }
            db.Insertable(bjdc_s).ExecuteCommand();
        }
        /// <summary>
        /// 竞彩篮球
        /// </summary>
        public void Add_JCLQ()
        {

        }
        /// <summary>
        /// 竞彩足球
        /// </summary>
        public void Add_JCZQ(List<jczq> model)
        {
            List<jczq_result> jczq_Results = new List<jczq_result>();
            foreach (var item in model)
            {
                jczq_result jczq = new jczq_result
                {
                    MatchId = item.id.Replace("-","")+item.TournamentNumber,
                    MatchDate = item.MatchTime,
                    MatchNumber= item.TournamentNumber,
                    HomeTeam=item.HomeTeam,
                    GuestTeam=item.VisitingTeam,
                    LetBall=item.LetBall,
                    HalfScore= item.Score == "-" ? "-" : item.Score.Split(")")[0].Replace("(", "").Replace(")", ""),
                    FullScore = item.Score == "-" ? "-" : item.Score.Split(")")[1].Replace(")", ""),
                    LeagueName=item.TournamentType,
                    League_Color=item.League_Color,
                    CreateTime=DateTime.Now,
                    JCDate = item.MatchTime,
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
            db.Insertable(jczq_Results).ExecuteCommand();
        }
    }
}
