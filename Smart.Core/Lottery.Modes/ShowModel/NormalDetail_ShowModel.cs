using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes.ShowModel
{
    public class NormalDetail_ShowModel
    {
        /// <summary>
        /// 彩种名称
        /// </summary>
        public string LotteryName { get; set; }

        /// <summary>
        /// 彩种码
        ///</summary>

        public string LotteryCode { get; set; }
        /// <summary>
        /// 期号
        ///</summary>

        public string IssueNo { get; set; }
        /// <summary>
        /// 创建时间
        ///</summary>

        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 开奖时间
        ///</summary>

        public string OpenTime { get; set; }
        /// <summary>
        /// 兑奖截止时间
        ///</summary>

        public string AwardDeadlineTime { get; set; }
        /// <summary>
        /// 奖池详情
        ///</summary>

        public string PrizePool { get; set; }
        /// <summary>
        /// 开奖数据详情
        ///</summary>

        public string LotteryDataDetail { get; set; }
        /// <summary>
        /// 开奖结果详情
        ///</summary>

        public string LotteryResultDetail { get; set; }


        /// <summary>
        /// 本期销量
        /// </summary>
        public string CurrentSales { get; set; }

        public string OpenCode { get; set; }
    }
}
