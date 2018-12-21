using System;
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
        public string LotteryCode { get; set; }
        /// <summary>
        /// 创建时间
        ///</summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        ///</summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 开奖时间
        ///</summary>
        public string OpenTime { get; set; }
        /// <summary>
        /// 开奖号码
        ///</summary>
        public string OpenCode { get; set; }
    }
}
