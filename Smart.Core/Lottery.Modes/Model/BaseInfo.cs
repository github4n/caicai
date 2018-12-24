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
        /// 期号
        /// </summary>
        public string expect { get; set; }
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
