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
        public string PrizeType { get; set; }
    }
}
