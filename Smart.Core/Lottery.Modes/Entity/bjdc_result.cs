using Smart.Core.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lottery.Modes.Entity
{
    /// <summary>
    // 
    ///</summary>

    public class bjdc_result : EntityBase
    { 
        public bjdc_result()
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

        public string MatchDate { get; set; }
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
        /// 让球
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
        /// 胜平负SP
        ///</summary>

        public string SPF_SP{ get; set; }
        /// <summary>
        /// 让球胜平负赛果
        ///</summary>

        public string RQSPF_Result{ get; set; }
        /// <summary>
        /// 让球胜平负SP
        ///</summary>

        public string RQSPF_SP{ get; set; }
        /// <summary>
        /// 总进球赛果
        ///</summary>

        public string ZJQ_Result{ get; set; }
        /// <summary>
        /// 总进球SP
        ///</summary>

        public string ZJQ_SP{ get; set; }
        /// <summary>
        /// 比分SP
        ///</summary>

        public string BF_SP{ get; set; }
        /// <summary>
        /// 半全场赛果
        ///</summary>

        public string BQC_Result{ get; set; }
        /// <summary>
        /// 半全场SP
        ///</summary>

        public string BQC_SP{ get; set; }
        /// <summary>
        /// 平均欧赔
        ///</summary>

        public string AvgEu_SP{ get; set; }
        /// <summary>
        /// 期号
        ///</summary>

        public string IssueNo{ get; set; }

        /// <summary>
        /// 是否采集完成
        ///</summary>

        public bool IsFinish{ get; set; }
    }
}
