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
        }
        /// <summary>
        /// 开奖详情
        /// </summary>
        public List<LotteryDetails> openLotteryDetails { get; set; }
    }
}
