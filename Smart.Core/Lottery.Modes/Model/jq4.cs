using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
     public class jq4: LotteryBase
    {
        public jq4() {
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
