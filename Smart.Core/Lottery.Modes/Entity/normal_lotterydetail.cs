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

    public class normal_lotterydetail : EntityBase
    { 
        public normal_lotterydetail()
        {
        
        }
        /// <summary>
        /// 
        ///</summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public int Id{ get; set; }
        /// <summary>
        /// 彩种Id
        ///</summary>

        public int LotteryId{ get; set; }
        /// <summary>
        /// 彩种码
        ///</summary>

        public string LotteryCode{ get; set; }
        /// <summary>
        /// 期号
        ///</summary>

        public string IssueNo{ get; set; }
        /// <summary>
        /// 创建时间
        ///</summary>

        public DateTime CreateTime{ get; set; }
        /// <summary>
        /// 开奖时间
        ///</summary>

        public string OpenTime{ get; set; }
        /// <summary>
        /// 兑奖截止时间
        ///</summary>

        public string AwardDeadlineTime{ get; set; }
        /// <summary>
        /// 奖池详情
        ///</summary>

        public string PrizePool{ get; set; }
        /// <summary>
        /// 开奖数据详情
        ///</summary>

        public string LotteryDataDetail{ get; set; }
        /// <summary>
        /// 开奖结果详情
        ///</summary>

        public string LotteryResultDetail{ get; set; }
        /// <summary>
        /// 期号Id
        ///</summary>

        public int? Sys_IssueId{ get; set; }
    }
}
