using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
   public class jq4Team
    {
        /// <summary>
        /// 球队
        /// </summary>
        public string openTeam { get; set; }
     
    }

    public class jq4Core
    {
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string openCode { get; set; }
    }
    public class jq4LotteryDetails

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
 
}
