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

    public class jclq_result : EntityBase
    { 
        public jclq_result()
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
        /// 让球数
        ///</summary>

        public string LetBall{ get; set; }
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
        /// 单关胜负赛果
        ///</summary>

        public string SF_Result{ get; set; }
        /// <summary>
        /// 过关胜负赛果
        ///</summary>

        public string GG_SF_Result{ get; set; }
        /// <summary>
        /// 单关让分胜负赛果
        ///</summary>

        public string RFSF_Result{ get; set; }
        /// <summary>
        /// 过关让分胜负赛果
        ///</summary>

        public string GG_RFSF_Result{ get; set; }
        /// <summary>
        /// 单关胜分差赛果
        ///</summary>

        public string SFC_Result{ get; set; }
        /// <summary>
        /// 过关胜分差赛果
        ///</summary>

        public string GG_SFC_Result{ get; set; }
        /// <summary>
        /// 预设总分
        ///</summary>

        public string YSZF{ get; set; }
        /// <summary>
        /// 单关大小分赛果
        ///</summary>

        public string DXF_Result{ get; set; }
        /// <summary>
        /// 过关大小分赛果
        ///</summary>

        public string GG_DXF_Result{ get; set; }
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
