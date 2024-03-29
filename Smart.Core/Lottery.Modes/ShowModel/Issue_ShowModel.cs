﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes.ShowModel
{
    public class Issue_ShowModel
    {
        /// <summary>
        /// 期号名称
        ///</summary>
        public string IssueNo { get; set; }
        /// <summary>
        /// 彩种id
        ///</summary>
        public int? LotteryId { get; set; }
        /// <summary>
        /// 彩种码
        ///</summary>
        public string LotteryName { get; set; }
        /// <summary>
        /// 彩种码
        ///</summary>
        public string LotteryCode { get; set; }
        /// <summary>
        /// 开奖时间
        ///</summary>
        public string OpenTime { get; set; }
        /// <summary>
        /// 开奖号码
        ///</summary>
        public string OpenCode { get; set; }

        /// <summary>
        /// 地区名
        ///</summary>

        public string RegionName { get; set; }
        /// <summary>
        /// 开奖日
        ///</summary>

        public string LotteryDay { get; set; }
        /// <summary>
        /// 每日期数
        ///</summary>

        public string NumberPeriods { get; set; }
      
        /// <summary>
        /// 滚存
        /// </summary>
        public string PrizePool { get; set; }
    }
}
