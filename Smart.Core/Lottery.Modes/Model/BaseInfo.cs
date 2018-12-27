using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
    public class BaseInfo
    {
        /// <summary>
        /// 注数
        /// </summary>
        public string BettingCount { get; set; }
        /// <summary>
        /// 奖金
        /// </summary>
        public string Bonus { get; set; }
        /// <summary>
        /// 奖项子项
        /// </summary>
        public string PrizeSubItem { get; set; }
    }

    public class LotteryBase
    {
        /// <summary>
        /// 期号id
        /// </summary>
        public long Sys_IssueId { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string expect { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        public string openCode { get; set; }

        /// <summary>
        /// 开奖时间
        /// </summary>
        public string openTime { get; set; }

        /// <summary>
        /// 兑奖截止日期
        /// </summary>
        public string endTime { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public string SalesVolume { get; set; }
        /// <summary>
        /// 奖池滚存
        /// </summary>
        public string PoolRolling { get; set; }

        /// <summary>
        /// 号码类型
        /// </summary>
        public string NumberType { get; set; }

        /// <summary>
        /// DLT合计金额
        /// </summary>
        public decimal TotalBonus { get; set; }
    }

    public class LotteryDetails
    {

        /// <summary>
        /// 奖项
        /// </summary>
        public string openPrize { get; set; }
        /// <summary>
        /// 中奖注数
        /// </summary>
        public string openWinNumber { get; set; }
        /// <summary>
        /// 单注奖金
        /// </summary>
        public string openSingleBonus { get; set; }

    }

    public class ttcx4Details
    {
        /// <summary>
        /// 投注
        /// </summary>
        public string Betting { get; set; }

        /// <summary>
        /// 奖项
        /// </summary>
        public string openPrize { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        public string openCode { get; set; }

        /// <summary>
        /// 直选
        /// </summary>
        public string directlySelection { get; set; }

        /// <summary>
        /// 组选24
        /// </summary>
        public string GroupSelection24  { get; set; }

        /// <summary>
        /// 组选12
        /// </summary>
        public string GroupSelection12 { get; set; }
        /// <summary>
        /// 组选6
        /// </summary>
        public string GroupSelection6 { get; set; }

        /// <summary>
        /// 组选4
        /// </summary>
        public string GroupSelection4 { get; set; }
    }

    public class Team
    {

        public string TeamTitle { get; set; }
        /// <summary>
        /// 球队
        /// </summary>
        public string openTeam { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        public string openCode { get; set; }

        /// <summary>
        /// 半全场 
        /// </summary>
        public string halfull { get; set; }

    }



}
