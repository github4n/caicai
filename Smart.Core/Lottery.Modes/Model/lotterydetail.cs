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
            openLotteryDetails = new List<LotteryDetails>();
            ttcx4Details = new List<ttcx4Details>();
        }



        /// <summary>
        /// 开奖详情
        /// </summary>
        public List<LotteryDetails> openLotteryDetails { get; set; }

        /// <summary>
        /// 天天彩选4开奖详情
        /// </summary>
        public List<ttcx4Details> ttcx4Details { get; set; }
        
    }
}
