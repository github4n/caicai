using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
    public class jczq
    {
        public jczq()
        {
            gameTypes = new List<GameType>();
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
        /// 赛事类型_颜色
        /// </summary>
        public string League_Color { get; set; }

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
        /// 平均欧赔
        /// </summary>
        public string AvgOuCompensation { get; set; }
        /// <summary>
        /// 比分
        /// </summary>
        public string Score { get; set; }
        public List<GameType> gameTypes { get; set; }
    }

    public enum Game
    {
        让球胜平负 = 1,
        胜平负 = 2,
        总进球数 = 3,
        半全场=4,
        比分=5,
        上下单双=6
    }

    public class GameType
    {
        public Game game { get; set; }
        /// <summary>
        /// 彩果
        /// </summary>
        public string FruitColor { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public string Bonus { get; set; }
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
