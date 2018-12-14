using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
   public class dlt: LotteryBase
    {
        public dlt()
        {
            dltLists = new List<dltList>();
        }
        public string Id { get; set; }

      
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenCode { get; set; }

        /// <summary>
        /// 出球顺序
        /// </summary>
        public string OutOfOrder { get; set; }

        public decimal TotalBonus { get; set; }

        public List<dltList> dltLists { get; set; }
    }

    public class dltList: LotteryDetails
    {
       

        public openPrizeType openPrizeType { get; set; }

        /// <summary>
        /// 应派奖金合计(元)
        /// </summary>
        public string openSumBonus { get; set; }


    }

    public enum openPrizeType
    {
        /// <summary>
        /// 基本
        /// </summary>
        basic = 1,
        /// <summary>
        /// 追加
        /// </summary>
        Append = 2
    }


}
