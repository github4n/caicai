using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
     public class jq4: LotteryBase
    {
        public jq4() {

            openTeam = new List<jq4Team>();
            openCode = new List<jq4Core>();
            openLotteryDetails = new List<LotteryDetails>();
         
        
        }
        public string Id { get; set; }

        /// <summary>
        /// 进球彩期号
        /// </summary>
        public string expect { get; set; }
        /// <summary>
        /// 球队
        /// </summary>
        public List<jq4Team> openTeam { get; set; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public List<jq4Core> openCode { get; set; }
        /// <summary>
        /// 开奖详情
        /// </summary>
        public List<LotteryDetails> openLotteryDetails { get; set; }
      
    }
}
