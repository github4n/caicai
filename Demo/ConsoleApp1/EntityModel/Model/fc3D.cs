using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
    public class fc3D
    {
        /// <summary>
        /// 期号
        /// </summary>
        public string expect { get; set; }
        /// <summary>
        /// 开奖号码 eg:7,3,1,
        /// </summary>
        public string opencode { get; set; }
        /// <summary>
        /// 试机号 eg:2,5,6,
        /// </summary>
        public string TestNumber { get; set; }
        /// <summary>
        /// 号码类型
        /// </summary>
        public string numberType { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public string LotteryDate { get; set; }
        /// <summary>
        /// 兑奖截止时间
        /// </summary>
        public string AwardDeadline { get; set; }
        /// <summary>
        /// 本期销量
        /// </summary>
        public string SalesVolume { get; set; }
        public List<LotteryInfo> SubItemList { get; set; }
    }
    public class LotteryInfo
    {
        /// <summary>
        /// 奖项
        /// </summary>
        public string Prize { get; set; }
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
}
