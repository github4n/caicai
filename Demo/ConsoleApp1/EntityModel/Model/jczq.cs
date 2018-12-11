using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
    public class jczq
    {
        public jczq()
        {
            rQSPF = new RQSPF();
            sPF = new SPF();
            zJQS = new ZJQS();
            bQC = new BQC();
           
        }
        public string id { get; set; }

        /// <summary>
        /// 赛事编号
        /// </summary>
        public string TournamentNumber { get; set; }
        /// <summary>
        /// 赛事类型
        /// </summary>
        public string TournamentType { get; set; }

        /// <summary>
        /// 比赛时间
        /// </summary>
        public string MatchTime { get; set; }

        /// <summary>
        /// 主队
        /// </summary>
        public string HomeTeam { get; set; }

        /// <summary>
        /// 让球
        /// </summary>
        public string LetBall { get; set; }
        /// <summary>
        /// 客队
        /// </summary>
        public string VisitingTeam { get; set; }

        /// <summary>
        /// 比分
        /// </summary>
        public string Score { get; set; }

        public RQSPF rQSPF { get; set; }

        public SPF sPF { get; set; }
        public ZJQS zJQS { get; set; }
        public BQC bQC { get; set; }



    }

    /// <summary>
    /// 让球胜平负
    /// </summary>
    public class RQSPF {

        /// <summary>
        /// 彩果
        /// </summary>
        public string FruitColor { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Bonus { get; set; }
    }
    /// <summary>
    /// 胜平负
    /// </summary>
    public class SPF
    {

        /// <summary>
        /// 彩果
        /// </summary>
        public string FruitColor { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Bonus { get; set; }
    }

    /// <summary>
    /// 总进球数
    /// </summary>
    public class ZJQS
    {

        /// <summary>
        /// 彩果
        /// </summary>
        public string FruitColor { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Bonus { get; set; }
    }

    /// <summary>
    /// 半全场
    /// </summary>
    public class BQC
    {

        /// <summary>
        /// 彩果
        /// </summary>
        public string FruitColor { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Bonus { get; set; }
    }

    /// <summary>
    /// 今日总结
    /// </summary>
    public class SummaryInfo
    {
        public SummaryInfo() {
            ResultList = new Dictionary<string, string>();
        }
        /// <summary>
        /// 玩法
        /// </summary>
        public string GameName { get; set; }
        /// <summary>
        /// 平均奖金值	
        /// </summary>
        public string AvgBonus { get; set; }
        /// <summary>
        /// 赛果分布	
        /// </summary>
        public Dictionary<string,string> ResultList { get; set; }
    }

}
