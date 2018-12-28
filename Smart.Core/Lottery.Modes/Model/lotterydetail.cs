using EntityModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes.Model
{
   public class lotterydetail: LotteryBase
    {
        public lotterydetail()
        {
            teams = new List<Team>();
            openLotteryDetails = new List<LotteryDetails>();
            ttcx4Details = new List<ttcx4Details>();
            dltLists = new List<dltList>();
        }
        /// <summary>
        /// 球队信息
        /// </summary>
        public List<dltList> dltLists { get; set; }
        /// <summary>
        /// 球队信息
        /// </summary>
        public List<Team> teams { get; set; }
        /// <summary>
        /// 开奖详情
        /// </summary>
        public List<LotteryDetails> openLotteryDetails { get; set; }

        /// <summary>
        /// 天天彩选4开奖详情
        /// </summary>
        public List<ttcx4Details> ttcx4Details { get; set; }
        
    }

    public class dltList : LotteryDetails
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
