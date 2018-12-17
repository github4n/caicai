using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
   public class fc3D:BaseInfo
    {
        public fc3D()
        {
            LotteryInfo = new LotteryInfo();
        }
        /// <summary>
        /// 奖项
        /// </summary>
        public string Prize { get; set; }
     
        public LotteryInfo LotteryInfo { get; set; }
    }
    public class LotteryInfo
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
    }
}
