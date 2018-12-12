using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
   public class dlt
    {
        public dlt()
        {
            dltLists = new List<dltList>();
        }
        public string Id { get; set; }

        /// <summary>
        /// 大乐透期号
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
        /// 开奖号码
        /// </summary>
        public string OpenCode { get; set; }

        /// <summary>
        /// 出球顺序
        /// </summary>
        public string OutOfOrder { get; set; }


        /// <summary>
        /// 销量
        /// </summary>
        public string SalesVolume { get; set; }
        /// <summary>
        /// 奖池滚存
        /// </summary>
        public string PoolRolling { get; set; }

        public decimal TotalBonus { get; set; }

        public List<dltList> dltLists { get; set; }
    }

    public class dltList
    {
        /// <summary>
        /// 奖项
        /// </summary>
        public string openPrize { get; set; }

        public openPrizeType openPrizeType { get; set; }

        /// <summary>
        /// 中奖注数
        /// </summary>
        public string openWinNumber { get; set; }
        /// <summary>
        /// 单注奖金
        /// </summary>
        public decimal openSingleBonus { get; set; }

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
