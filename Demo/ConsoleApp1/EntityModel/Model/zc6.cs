using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
   public class zc6: LotteryBase
    {
        public zc6()
        {
            teams = new List<Team>();
            openLotteryDetails = new List<LotteryDetails>();
        }
        public string Id { get; set; }
        public List<Team> teams { get; set; }
        /// <summary>
        /// 开奖详情
        /// </summary>
        public List<LotteryDetails> openLotteryDetails { get; set; }

    }
}
