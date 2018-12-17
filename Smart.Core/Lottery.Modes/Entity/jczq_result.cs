using Smart.Core.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lottery.Modes.Entity
{
    /// <summary>
    /// 
    ///</summary>

    public class jczq_result : EntityBase
    { 
        public jczq_result()
        {
        
        }
        /// <summary>
        /// 比赛Id
        ///</summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public string MatchId{ get; set; }
        /// <summary>
        /// 比赛时间
        ///</summary>

        public string MatchDate{ get; set; }
        /// <summary>
        /// 比赛编号
        ///</summary>

        public string MatchNumber{ get; set; }
        /// <summary>
        /// 主队
        ///</summary>

        public string HomeTeam{ get; set; }
        /// <summary>
        /// 客队
        ///</summary>

        public string GuestTeam{ get; set; }
        /// <summary>
        /// 让球数
        ///</summary>

        public string LetBall{ get; set; }
        /// <summary>
        /// 半场比分
        ///</summary>

        public string HalfScore{ get; set; }
        /// <summary>
        /// 全场比分
        ///</summary>

        public string FullScore{ get; set; }
        /// <summary>
        /// 赛事类型
        ///</summary>

        public string LeagueName{ get; set; }
        /// <summary>
        /// 赛事类型_颜色
        ///</summary>

        public string League_Color{ get; set; }
        /// <summary>
        /// 胜平负赛果
        ///</summary>

        public string SPF_Result{ get; set; }
        /// <summary>
        /// 胜平负奖金
        ///</summary>

        public string SPF_SP{ get; set; }
        /// <summary>
        /// 让球胜平负赛果
        ///</summary>

        public string RQSPF_Result{ get; set; }
        /// <summary>
        /// 让球胜平负奖金
        ///</summary>

        public string RQSPF_SP{ get; set; }
        /// <summary>
        /// 总进球赛果
        ///</summary>

        public string ZJQ_Result{ get; set; }
        /// <summary>
        /// 总进球奖金
        ///</summary>

        public string ZJQ_SP{ get; set; }
        /// <summary>
        /// 比分奖金
        ///</summary>

        public string BF_SP{ get; set; }
        /// <summary>
        /// 半全场赛果
        ///</summary>

        public string BQC_Result{ get; set; }
        /// <summary>
        /// 半全场奖金
        ///</summary>

        public string BQC_SP{ get; set; }
        /// <summary>
        /// 平均欧赔
        ///</summary>

        public string AvgEu_SP{ get; set; }
        /// <summary>
        /// 竞彩日期
        ///</summary>

        public string JCDate{ get; set; }
    }
}
